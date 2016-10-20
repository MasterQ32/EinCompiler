using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EinCompiler.BuiltInTypes;

namespace EinCompiler.BackEnds
{
	public sealed class CCodeBackEnd : BackEnd
	{
		protected override void WriteLineComment(string text)
		{
			WriteLine("// {0}", text);
		}

		private readonly Dictionary<TypeDescription, string> typeNames = new Dictionary<TypeDescription, string>();

		static string TypeName(TypeDescription type)
		{
			if (type == Types.Void) {
				return "void";
			}

			if (type is IntegerType) {
				string name = "";
				var itype = (IntegerType)type;
				if (!itype.IsSigned) {
					name += "u";
				}
				name += "int";
				switch (itype.Size) {
				case 1:
					name += "8";
					break;
				case 2:
					name += "16";
					break;
				case 4:
					name += "32";
					break;
				case 8:
					name += "64";
					break;
				default:
					throw new NotSupportedException ();
				}
				name += "_t";
				return name;
			}
			if (type is ArrayType) {
				var atype = (ArrayType)type;
				return TypeName(atype.ElementType) + $"[{atype.Length}]";
			}

			throw new NotSupportedException();
		}

		protected override void Generate(ModuleDescription module)
		{
			WriteLine ("#include <stdio.h>");
			WriteLine ("#include <stdlib.h>");
			WriteLine ("#include <stdint.h>");
			WriteLine ("#include <stddef.h>");

			WriteLineComment ("Types:");
			foreach (var type in module.Types) {

				typeNames [type.Value] = TypeName (type.Value);

			}
			WriteLineComment("Constants:");
			foreach (var @const in module.Constants)
			{
				WriteConstant(@const);
			}

			WriteLine();

			WriteLineComment("Variables:");
			foreach (var var in module.Variables)
			{
				WriteVariable(var);
			}

			WriteLine();

			WriteLineComment("Function Prototypes:");
			foreach (var func in module.Functions)
			{
				WriteFunctionHeader(func);
				WriteLine(";");
			}

			WriteLine();

			WriteLineComment("Functions:");
			foreach (var func in module.Functions)
			{
				WriteFunctionHeader(func);
				if (func.NakedBody != null) {
					WriteLine ("{");
					WriteLine (func.NakedBody);
					WriteLine ("}");
				} else { 
					WriteLine ();
					WriteBlock (func.Body);
					WriteLine ();
				}
			}
		}

		private void WriteBlock(BodyDescription body)
		{
			WriteLine("{");
			IncreaseIndentation();

			foreach (var instr in body)
			{
				WriteInstruction(instr);
			}

			DecreaseIndentation();
			WriteLine("}");
		}

		private void WriteInstruction(InstructionDescription instr)
		{
			if (instr is ExpressionInstruction) {
				WriteExpression (((ExpressionInstruction)instr).Expression);
				WriteLine (";");
			} else if (instr is ConditionalInstruction) {
				var condition = ((ConditionalInstruction)instr).Condition;
				var trueBody = ((ConditionalInstruction)instr).TrueBody;
				var falseBody = ((ConditionalInstruction)instr).FalseBody;

				Write ("if (");
				WriteExpression (condition);
				WriteLine (")");
				WriteBlock (trueBody);
				if (falseBody != null) {
					WriteLine ("else");
					WriteBlock (falseBody);
				}
			} else if (instr is LoopInstruction) {
				var condition = ((LoopInstruction)instr).Condition;
				var body = ((LoopInstruction)instr).Body;
				Write ("while(");
				WriteExpression (condition);
				WriteLine (")");
				WriteBlock (body);
			} else if (instr is ReturnInstruction) {
				var expr = ((ReturnInstruction)instr).Expression;
				Write ("return ");
				WriteExpression (((ReturnInstruction)instr).Expression);
				WriteLine (";");
			} else if (instr is NopInstruction) {
				WriteLine (";");
			} else if (instr is BreakLoopInstruction) {
				WriteLine ("break;");
			} else {
				throw new NotSupportedException ($"Instruction {instr.GetType().Name} is not supported yet.");
			}
		}

		private void WriteExpression(Expression expression)
		{
			if (expression is AssignmentExpression) {
				var ass = (AssignmentExpression)expression;

				WriteExpression (ass.Target);
				Write (" = ");
				WriteExpression (ass.Source);
			} else if (expression is BinaryOperatorExpression) {
				var bin = (BinaryOperatorExpression)expression;

				WriteExpression (bin.LeftHandSide);
				switch (bin.Operator) {
				case BinaryOperator.Addition:
					Write (" + ");
					break;
				case BinaryOperator.Subtraction:
					Write (" - ");
					break;
				case BinaryOperator.Multiplication:
					Write (" * ");
					break;
				case BinaryOperator.Division:
					Write (" / ");
					break;
				case BinaryOperator.EuclideanDivision:
					Write (" % ");
					break;
				default:
					throw new NotSupportedException ();
				}
				WriteExpression (bin.RightHandSide);
			} else if (expression is FunctionCallExpression) {
				var call = (FunctionCallExpression)expression;

				Write ("{0}(", call.Function.Name);
				
				for (int i = 0; i < call.Arguments.Length; i++) {
					if (i > 0)
						Write (", ");
					WriteExpression (call.Arguments [i]);
				}

				Write (")");
			} else if (expression is LiteralExpression) {
				Write (((LiteralExpression)expression).Literal);
			} else if (expression is VariableExpression) {
				Write (((VariableExpression)expression).Variable.Name);
			} else if (expression is IndexerExpression) {
				var expr = (IndexerExpression)expression;
				Write ("{0}[", expr.Array.Name);
				WriteExpression (expr.Index);
				Write ("]");
			} else {
				throw new NotSupportedException ($"Expression {expression.GetType().Name} is not supported yet.");
			}
		}

		private void WriteFunctionHeader(FunctionDescription func)
		{
			Write("{0} {1}({2})",
				typeNames[func.ReturnType],
				func.Name,
				String.Join(", ", func.Parameters.Select(p => $"{typeNames[p.Type]} {p.Name}")));
		}

		private void WriteConstant(ConstantDescription @const)
		{
			WriteLine("const {0} {1} = {2};", typeNames[@const.Type], @const.Name, @const.InitialValue.GetString());
		}

		private void WriteVariable(VariableDescription var)
		{
			if (var.InitialValue != null)
				WriteLine("{0} {1} = {2};", typeNames[var.Type], var.Name, var.InitialValue.GetString());
			else
				WriteLine("{0} {1};", typeNames[var.Type], var.Name);
		}
	}
}
