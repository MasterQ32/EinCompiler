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
			AllocateGlobalVariables(module);

			WriteLine();
			WriteLineComment("Functions:");

			foreach (var func in module.Functions)
			{
				if (func.IsNaked)
				{
					if (func.IsInline)
						continue;
					// Yay, naked functions!
					WriteLabel(func.Name);
					Write(func.NakedBody);
				}
				else
				{
					WriteLabel(func.Name);
					WriteFunctionEnter();

					foreach (var local in func.Locals)
					{
						WriteCommand(
							"push 0 ; allocate local {0}",
							local.Name);
					}

					WriteBlock(func, null, func.Body);

					WriteFunctionLeave();
					WriteLine();
				}
			}
		}

		private void AllocateGlobalVariables(ModuleDescription module)
		{
			if (module.Variables.Count == 0)
				return;
			int globalAllocator = 0;
			var maxWidth = Math.Max(
				module.Variables.Max(v => v.Name.Length),
				8);
			var maxTypeWidth = Math.Max(
				module.Variables.Max(v => v.Type.Name.Length),
				4);

			var header = string.Format(
				"| {0} | {1} | {2} | {3} |",
				"Variable".PadRight(maxWidth),
				"Type".PadRight(maxTypeWidth),
				"Address",
				"Size");
			var separator = string.Format(
				"|-{0}-|-{1}-|-{2}-|-{3}-|",
				"--------".PadRight(maxWidth, '-'),
				"----".PadRight(maxTypeWidth, '-'),
				"-------",
				"----");
			WriteLineComment(header);
			WriteLineComment(separator);
			foreach (var var in module.Variables)
			{
				// Allocate storage for each variable given by the types size.
				globals[var] = globalAllocator;

				var desc = string.Format(
					"| {0} | {1} | {2} | {3} |",
					var.Name.PadRight(maxWidth),
					var.Type.Name.PadRight(maxTypeWidth),
					globalAllocator.ToString().PadLeft(7),
					var.Type.Size.ToString().PadLeft(4));
				WriteLineComment(desc);

				globalAllocator += var.Type.Size;
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
			foreach (var instr in body)
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
				WriteCommand("drop [f:no]");

				if (falseBody != null)
					WriteCommand("[ex(z)=1] jmp @{0} ; Jump to false", falseLabel);
				else
					WriteCommand("[ex(z)=1] jmp @{0} ; Jump to end", endLabel);

				WriteBlock(context, breakTarget, trueBody);
				if (falseBody != null)
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

				WriteCommand("drop [f:no]");
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
					-(2 + context.Parameters.Count));
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
					if (var is ParameterDescription)
					{
						var offset = ((ParameterDescription)var).Index;
						var position = -(offset + 2);

						WriteCommand(
							"set {0} {1} [r:push] ; argument {2}",
							position,
							flagText,
							var.Name);
					}
					else if (var is LocalDecription)
					{
						var offset = ((LocalDecription)var).Index;
						var position = 1 + offset;
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
				switch (bin.Operator)
				{
					case BinaryOperator.Addition: WriteCommand("add {0}", flagText); break;
					case BinaryOperator.Subtraction: WriteCommand("sub {0}", flagText); break;
					case BinaryOperator.Multiplication: WriteCommand("mul {0}", flagText); break;
					case BinaryOperator.Division: WriteCommand("div {0}", flagText); break;
					case BinaryOperator.EuclideanDivision: WriteCommand("mod {0}", flagText); break;

					case BinaryOperator.Equals:
					{
						WriteCommand("sub [f:yes] [r:discard]");
						WriteCommand("[ex(z)=0] push 0");
						WriteCommand("[ex(z)=1] push 1");
						if (modifyFlags)
							WriteCommand("[i0:peek] nop [f:yes]");
						break;
					}
					case BinaryOperator.Differs:
					{
						WriteCommand("sub [f:yes] [r:discard]");
						WriteCommand("[ex(z)=0] push 1");
						WriteCommand("[ex(z)=1] push 0");
						if (modifyFlags)
							WriteCommand("[i0:peek] nop [f:yes]");
						break;
					}

					case BinaryOperator.LessOrEqual:
					{
						WriteCommand("sub [f:yes] [r:discard]");
						WriteCommand("[ex(z)=0] [ex(n)=0] push 1");
						WriteCommand("[ex(z)=0] [ex(n)=1] push 0");
						WriteCommand("[ex(z)=1] push 1");
						if (modifyFlags)
							WriteCommand("[i0:peek] nop [f:yes]");
						break;
					}
					case BinaryOperator.GreaterOrEqual:
					{
						WriteCommand("sub [f:yes] [r:discard]");
						WriteCommand("[ex(z)=0] [ex(n)=0] push 0");
						WriteCommand("[ex(z)=0] [ex(n)=1] push 1");
						WriteCommand("[ex(z)=1] push 1");
						if (modifyFlags)
							WriteCommand("[i0:peek] nop [f:yes]");
						break;
					}

					case BinaryOperator.LessThan:
					{
						WriteCommand("sub [f:yes] [r:discard]");
						WriteCommand("[ex(z)=0] [ex(n)=0] push 1");
						WriteCommand("[ex(z)=0] [ex(n)=1] push 0");
						WriteCommand("[ex(z)=1] push 0");
						if (modifyFlags)
							WriteCommand("[i0:peek] nop [f:yes]");
						break;
					}
					case BinaryOperator.GreaterThan:
					{
						WriteCommand("sub [f:yes] [r:discard]");
						WriteCommand("[ex(z)=0] [ex(n)=0] push 0");
						WriteCommand("[ex(z)=0] [ex(n)=1] push 1");
						WriteCommand("[ex(z)=1] push 0");
						if (modifyFlags)
							WriteCommand("[i0:peek] nop [f:yes]");
						break;
					}

					default: throw new NotSupportedException();
				}
			}
			else if (expression is FunctionCallExpression)
			{
				var call = (FunctionCallExpression)expression;
				var fn = call.Function;

				// If we have a return value and no inline naked,
				// push a stub
				if (fn.IsInline == false &&
					fn.ReturnType != TypeDescription.Void)
					WriteCommand("push 0");
				
				foreach (var arg in call.Arguments.Reverse())
				{
					WriteExpression(context, arg);
				}

				if (fn.IsNaked && fn.IsInline)
				{
					Write(fn.NakedBody);
				}
				else
				{
					WriteCommand("cpget");
					WriteCommand("jmp @{0}", fn.Name);

					foreach (var param in fn.Parameters)
					{
						WriteCommand("drop ; {0}", param.Name);
					}
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
				if (var is ParameterDescription)
				{
					var offset = ((ParameterDescription)var).Index;
					var position = -(offset + 2);

					WriteCommand(
						"get {0} {1} ; argument {2}",
						position,
						flagText,
						var.Name);
				}
				else if (var is LocalDecription)
				{
					var offset = ((LocalDecription)var).Index;
					var position = 1 + offset;

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
