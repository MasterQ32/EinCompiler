using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinCompiler.BackEnds
{
	public sealed class SVMABackEnd : BackEnd
	{
		private int labelCounter = 0;

		private string GenUniqueLabelName() => $"__internal__label{labelCounter++}";

		private Dictionary<VariableDescription, int> globals = new Dictionary<VariableDescription, int>();

		protected override void Generate(ModuleDescription module)
		{
			int globalAllocator = 0;
			foreach(var var in module.Variables)
			{
				globals[var] = globalAllocator;
				globalAllocator += 4;
			}

			WriteLineComment("Variables are not supported yet.");
			WriteLine();
			WriteLineComment("Functions:");

			foreach (var func in module.Functions)
			{
				WriteLabel(func.Name);
				if (func.IsNaked)
				{
					// Yay, naked functions!
					Write(func.NakedBody);
				}
				else
				{
					WriteFunctionEnter();

					// TODO: Push local variables here....

					WriteBlock(func, null, func.Body);

					WriteFunctionLeave();
					WriteLine();
				}
			}
		}

		private void WriteFunctionEnter()
		{
			WriteCommand("bpget");
			WriteCommand("spget");
			WriteCommand("bpset");
		}

		private void WriteFunctionLeave()
		{
			WriteCommand("bpget");
			WriteCommand("spset");
			WriteCommand("bpset");
			WriteCommand("ret");
		}

		private void WriteBlock(
			FunctionDescription context, 
			string breakTarget, 
			BodyDescription body)
		{
			foreach(var instr in body)
			{
				WriteCommand("; {0}", instr);
				WriteInstruction(context, breakTarget, instr);
				WriteLine();
			}
		}

		private void WriteInstruction(
			FunctionDescription context, 
			string breakTarget,
			InstructionDescription instr)
		{
			if (instr is ExpressionInstruction)
			{
				WriteExpression(context, ((ExpressionInstruction)instr).Expression);
				WriteCommand("drop");
			}
			else if (instr is ConditionalInstruction)
			{
				var condition = ((ConditionalInstruction)instr).Condition;
				var trueBody = ((ConditionalInstruction)instr).TrueBody;
				var falseBody = ((ConditionalInstruction)instr).FalseBody;

				var falseLabel = this.GenUniqueLabelName();
				var endLabel = this.GenUniqueLabelName();

				WriteCommand("; if condition");
				WriteExpression(context, condition, true);

				if(falseBody != null)
					WriteCommand("[ex(z)=0] jmp @{0} ; Jump to false", falseLabel);
				else
					WriteCommand("[ex(z)=0] jmp @{0} ; Jump to end", endLabel);

				WriteBlock(context, breakTarget, trueBody);
				if(falseBody != null)
				{
					WriteCommand("jmp @{0}; jump to end", endLabel);
					WriteLabel(falseLabel);
					WriteBlock(context, breakTarget, falseBody);
				}
				
				WriteLabel(endLabel);
			}
			else if (instr is LoopInstruction)
			{
				var condition = ((LoopInstruction)instr).Condition;
				var body = ((LoopInstruction)instr).Body;

				var loopStart = this.GenUniqueLabelName();
				var loopEnd = this.GenUniqueLabelName();

				WriteCommand("; begin while");
				WriteLabel(loopStart);
				
				WriteExpression(context, condition, true);
				
				WriteCommand("[ex(z)=1] jmp @{0}", loopEnd);

				WriteBlock(context, loopEnd, body);

				WriteCommand("jmp @{0} ; end while", loopStart);

				WriteLabel(loopEnd);
			}
			else if (instr is ReturnInstruction)
			{
				var expr = ((ReturnInstruction)instr).Expression;
				WriteExpression(context, ((ReturnInstruction)instr).Expression);

				WriteCommand(
					"set {0}",
					-(2 + context.Parameters.Length));
				WriteFunctionLeave();
			}
			else if (instr is NopInstruction)
			{
				WriteCommand("nop");
			}
			else if (instr is BreakLoopInstruction)
			{
				if (breakTarget == null)
					throw new InvalidOperationException("Invalid break detected.");
				WriteCommand(
					"jmp @{0}; break",
					breakTarget);
			}
		}

		private void WriteCommand(string v, params object[] args) => this.WriteLine("\t" + v, args);

		private void WriteExpression(FunctionDescription context, Expression expression, bool modifyFlags = false)
		{
			var flagText = modifyFlags ? " [f:yes] " : "";
			if (expression is AssignmentExpression)
			{
				var ass = (AssignmentExpression)expression;
				
				WriteExpression(context, ass.Source);
				if (ass.Target is VariableExpression)
				{
					var var = ((VariableExpression)ass.Target).Variable;
					if (var is ParameterVariableDescription)
					{
						var offset = ((ParameterVariableDescription)var).Index;
						var position = -(offset + 1);

						WriteCommand(
							"set {0} {1} [r:push] ; local {2}",
							position,
							flagText,
							var.Name);
					}
					else
					{
						var location = globals[var];
						WriteCommand(
							"store {0} {1} [r:push] ; global {2}",
							location,
							flagText,
							var.Name);
					}
				}
				else
					throw new NotSupportedException();
			}
			else if (expression is BinaryOperatorExpression)
			{
				var bin = (BinaryOperatorExpression)expression;
				
				WriteExpression(context, bin.RightHandSide);
				WriteExpression(context, bin.LeftHandSide);
				Write("\t");
				switch (bin.Operator)
				{
					case BinaryOperator.Addition: Write("add"); break;
					case BinaryOperator.Subtraction: Write("sub"); break;
					case BinaryOperator.Multiplication: Write("mul"); break;
					case BinaryOperator.Division: Write("div"); break;
					case BinaryOperator.EuclideanDivision: Write("mod"); break;
					default: throw new NotSupportedException();
				}
				WriteLine(flagText);
			}
			else if (expression is FunctionCallExpression)
			{
				var call = (FunctionCallExpression)expression;
				var fn = call.Function;
				
				// If we have a return value, push a stub
				if(fn.ReturnType != TypeDescription.Void)
					WriteCommand("push 0");

				// TODO: Check correct argument pushing order
				foreach (var arg in call.Arguments)
				{
					WriteExpression(context, arg);
				}

				WriteCommand("cpget");
				WriteCommand("jmp @{0}", fn.Name);

				foreach (var param in fn.Parameters.Reverse())
				{
					WriteCommand("drop ; {0}", param.Name);
				}

				// If we don't have a return value, push a stub result
				if (fn.ReturnType == TypeDescription.Void)
					WriteCommand("push 0");
			}
			else if (expression is LiteralExpression)
			{
				var expr = (LiteralExpression)expression;
				WriteCommand(
					"push {0} {1}",
					expr.GetValue().GetString(),
					flagText);
			}
			else if (expression is VariableExpression)
			{
				var var = ((VariableExpression)expression).Variable;
				if (var is ParameterVariableDescription)
				{
					var offset = ((ParameterVariableDescription)var).Index;
					var position = -(offset + 2);

					WriteCommand(
						"get {0} {1} ; local {2}",
						position,
						flagText,
						var.Name);
				}
				else
				{
					var location = globals[var];
					WriteCommand(
						"load {0} {1} ; global {2}",
						location,
						flagText,
						var.Name);
				}
			}
		}

		private void WriteLabel(string name)
		{
			WriteLine("{0}:", name);
		}

		protected override void WriteLineComment(string text)
		{
			WriteLine("; {0}", text);
		}
	}
}
