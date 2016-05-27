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

		private string GetUniqueLabelName() => $"__internal__label{labelCounter++}";

		protected override void Generate(ModuleDescription module)
		{
			WriteLineComment("Variables are not supported yet.");
			WriteLine();
			WriteLineComment("Functions:");

			foreach (var func in module.Functions)
			{
				WriteLabel(func.Name);
				WriteFunctionEnter();

				// TODO: Push local variables here....

				WriteBlock(func.Body);

				WriteFunctionLeave();
				WriteLine();
			}
		}

		private void WriteFunctionEnter()
		{
			WriteLine("\tbpget");
			WriteLine("\tspget");
			WriteLine("\tbpset");
		}

		private void WriteFunctionLeave()
		{
			WriteLine("\tbpget");
			WriteLine("\tspset");
			WriteLine("\tbpset");
			WriteLine("\tret");
		}

		private void WriteBlock(BodyDescription body)
		{
			foreach(var instr in body)
			{
				WriteLine("\t; {0}", instr);
				WriteInstruction(instr);
			}
		}

		private void WriteInstruction(InstructionDescription instr)
		{
			if (instr is ExpressionInstruction)
			{
				WriteExpression(((ExpressionInstruction)instr).Expression);
				WriteLine("\tdrop");
			}
			else if (instr is ConditionalInstruction)
			{
				var condition = ((ConditionalInstruction)instr).Condition;
				var trueBody = ((ConditionalInstruction)instr).TrueBody;
				var falseBody = ((ConditionalInstruction)instr).FalseBody;

				Write("if (");
				WriteExpression(condition);
				WriteLine(")");
				WriteBlock(trueBody);
				if (falseBody != null)
				{
					WriteLine("else");
					WriteBlock(falseBody);
				}
			}
			else if (instr is LoopInstruction)
			{
				var condition = ((LoopInstruction)instr).Condition;
				var body = ((LoopInstruction)instr).Body;
				Write("while(");
				WriteExpression(condition);
				WriteLine(")");
				WriteBlock(body);
			}
			else if (instr is ReturnInstruction)
			{
				var expr = ((ReturnInstruction)instr).Expression;
				Write("return ");
				WriteExpression(((ReturnInstruction)instr).Expression);

				WriteLine("\tset ??? ; TODO: Insert return value position here.");
				WriteFunctionLeave();
			}
			else if (instr is NopInstruction)
			{
				WriteLine("\tnop");
			}
			else if (instr is BreakLoopInstruction)
			{
				WriteLine("break;");
			}
		}

		private void WriteExpression(Expression expression)
		{
			if (expression is AssignmentExpression)
			{
				var ass = (AssignmentExpression)expression;

				WriteExpression(ass.Target);
				Write(" = ");
				WriteExpression(ass.Source);
			}
			else if (expression is BinaryOperatorExpression)
			{
				var bin = (BinaryOperatorExpression)expression;

				WriteExpression(bin.LeftHandSide);
				switch (bin.Operator)
				{
					case BinaryOperator.Addition: Write(" + "); break;
					case BinaryOperator.Subtraction: Write(" - "); break;
					case BinaryOperator.Multiplication: Write(" * "); break;
					case BinaryOperator.Division: Write(" / "); break;
					case BinaryOperator.EuclideanDivision: Write(" % "); break;
					default: throw new NotSupportedException();
				}
				WriteExpression(bin.RightHandSide);
			}
			else if (expression is FunctionCallExpression)
			{
				var call = (FunctionCallExpression)expression;

				Write("{0}(", call.Function.Name);

				for (int i = 0; i < call.Arguments.Length; i++)
				{
					if (i > 0)
						Write(", ");
					WriteExpression(call.Arguments[i]);
				}

				Write(")");
			}
			else if (expression is LiteralExpression)
			{
				var expr = (LiteralExpression)expression;
				WriteLine(
					"\tpush {0}", 
					expr.GetValue().GetString());
			}
			else if (expression is VariableExpression)
			{
				WriteLine("\tload ??? ; TODO: Insert variable address");
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
