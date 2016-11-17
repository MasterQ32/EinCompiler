using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinCompiler.BackEnds
{
	public sealed class CCodeBackEnd : BackEnd
	{
		readonly Dispatcher<InstructionDescription, object> writeInstruction = new Dispatcher<InstructionDescription, object>();
		readonly Dispatcher<Expression, object> writeExpression = new Dispatcher<Expression, object>();
		readonly Dictionary<BinaryOperator, string> binaryOperators = new Dictionary<BinaryOperator, string>()
		{
			{ BinaryOperator.Addition, "+" },
			{ BinaryOperator.Subtraction, "-" },
			{ BinaryOperator.Multiplication, "*" },
			{ BinaryOperator.Division, "/" },
			{ BinaryOperator.EuclideanDivision, "%" },
			{ BinaryOperator.Equals, "==" },
			{ BinaryOperator.Differs, "!=" },
			{ BinaryOperator.GreaterOrEqual, ">=" },
			{ BinaryOperator.GreaterThan, ">" },
			{ BinaryOperator.LessOrEqual, "<=" },
			{ BinaryOperator.LessThan, "<" },
		};

		public CCodeBackEnd()
		{
			writeInstruction.Register<ExpressionInstruction>(WriteExpressionInstruction);
			writeInstruction.Register<ReturnInstruction>(WriteReturn);
			writeInstruction.Register<ConditionalInstruction>(WriteIf);
			writeInstruction.Register<LoopInstruction>(WriteLoop);
			writeInstruction.Register<BreakLoopInstruction>(WriteBreak);

			writeExpression.Register<AssignmentExpression>(WriteAssignment);
			writeExpression.Register<VariableExpression>(WriteVariable);
			writeExpression.Register<LiteralExpression>(WriteLiteral);
			writeExpression.Register<FunctionCallExpression>(WriteCall);
			writeExpression.Register<IndexerExpression>(WriteIndexer);
			writeExpression.Register<BinaryOperatorExpression>(WriteBinaryOp);
		}

		protected override void WriteLineComment(string text)
		{
			WriteLine("// {0}", text);
		}

		private string GetTypeName(TypeDescription type)
		{
			if (type == Types.Void)
				return "void";
			else if (type == Types.Int8)
				return "int8_t";
			else if (type == Types.Int16)
				return "int16_t";
			else if (type == Types.Int32)
				return "int32_t";
			else if (type == Types.UInt8)
				return "uint8_t";
			else if (type == Types.UInt16)
				return "uint16_t";
			else if (type == Types.UInt32)
				return "uint32_t";
			else if (type is ArrayType)
			{
				var atype = (ArrayType)type;
				return GetTypeName(atype.ElementType) + "*";
			}
			throw new InvalidOperationException($"Invalid type: {type}");
		}

		protected override void Generate(ModuleDescription module)
		{
			WriteLine("#define putc _putc // DIRTY HACKS");
			WriteLine("#include <stdio.h>");
			WriteLine("#include <stdlib.h>");
			WriteLine("#include <stdint.h>");
			WriteLine("#include <stddef.h>");
			WriteLine("#undef putc");

			foreach (var c in module.Constants)
			{
				Write("const ");
				Write(GetTypeName(c.Type));
				Write(" ");
				Write(c.Name);
				Write(" = ");
				Write(c.InitialValue.Value.ToString());
				WriteLine(";");
			}

			foreach (var v in module.Variables)
			{
				if (v.Storage.HasFlag(StorageModifier.Static))
				{
					Write("static ");
				}
				Write(GetTypeName(v.Type));
				Write(" ");
				Write(v.Name);
				if (v.Type is ArrayType)
				{
					throw new NotSupportedException();
					/**
					 * var atype = (ArrayType)v.Type;
					Write (
						"= ({0})({1}[{2}]){{}}",
						GetTypeName (v.Type),
						GetTypeName (atype.ElementType),
						atype.Length);
						*/

				}
				else
				{
					if (v.InitialValue != null)
					{
						Write(" = ");
						Write(v.InitialValue.Value.ToString());
					}
				}
				WriteLine(";");

			}
			foreach (var func in module.Functions)
			{

				WriteHeader(func);
				WriteLine(";");
			}

			foreach (var func in module.Functions)
			{

				if (func.IsExtern)
					continue;

				WriteHeader(func);
				WriteLine();

				if (func.NakedBody != null)
				{
					WriteLine("{");
					IncreaseIndentation();
					WriteLine(func.NakedBody);
					DecreaseIndentation();
					WriteLine("}");
				}
				else
				{
					WriteLine("{");
					IncreaseIndentation();

					foreach (var var in func.Locals)
					{
						WriteLine("{0} {1};", GetTypeName(var.Type), var.Name);
					}

					WriteBody(func.Body, true);
					DecreaseIndentation();
					WriteLine("}");
				}
			}
		}

		private void WriteHeader(FunctionDescription func)
		{
			Write(GetTypeName(func.ReturnType));
			Write(" ");
			Write(func.Name);
			Write("(");

			for (int i = 0; i < func.Parameters.Count; i++)
			{
				if (i > 0)
					Write(", ");
				var p = func.Parameters[i];
				Write(GetTypeName(p.Type));
				Write(" ");
				Write(p.Name);
			}

			Write(")");
		}

		private void WriteBody(BodyDescription body, bool suppressBrackets = false)
		{
			if (!suppressBrackets)
			{
				WriteLine("{");
				IncreaseIndentation();
			}
			foreach (var i in body)
			{
				writeInstruction.Invoke(i, null);
			}
			if (!suppressBrackets)
			{
				DecreaseIndentation();
				WriteLine("}");
			}
		}

		private void WriteExpressionInstruction(ExpressionInstruction instr, object obj)
		{
			writeExpression.Invoke(instr.Expression, obj);
			WriteLine(";");
		}

		private void WriteReturn(ReturnInstruction instr, object obj)
		{
			Write("return ");
			writeExpression.Invoke(instr.Expression, obj);
			WriteLine(";");
		}

		private void WriteIf(ConditionalInstruction instr, object obj)
		{
			Write("if(");
			writeExpression.Invoke(instr.Condition, obj);
			WriteLine(")");
			WriteBody(instr.TrueBody);
			if (instr.FalseBody != null)
			{
				WriteLine("else");
				WriteBody(instr.FalseBody);
			}
			WriteLine();
		}

		private void WriteLoop(LoopInstruction instr, object obj)
		{
			Write("while(");
			writeExpression.Invoke(instr.Condition, obj);
			WriteLine(")");
			WriteBody(instr.Body);
		}

		private void WriteBreak(BreakLoopInstruction instr, object obj)
		{
			WriteLine("break;");
		}

		private void WriteAssignment(AssignmentExpression expr, object obj)
		{
			writeExpression.Invoke(expr.Target, obj);
			Write(" = ");
			writeExpression.Invoke(expr.Source, obj);
		}

		private void WriteVariable(VariableExpression expr, object obj)
		{
			Write(expr.Variable.Name);
		}

		private void WriteLiteral(LiteralExpression expr, object obj)
		{
			Write(expr.Literal.Value);
		}

		private void WriteCall(FunctionCallExpression expr, object obj)
		{
			Write(expr.Function.Name);
			Write("(");
			for (int i = 0; i < expr.Arguments.Length; i++)
			{
				if (i > 0)
					Write(", ");
				writeExpression.Invoke(expr.Arguments[i], obj);
			}

			Write(")");
		}

		private void WriteIndexer(IndexerExpression expr, object obj)
		{
			Write(expr.Array.Name);
			Write("[");
			writeExpression.Invoke(expr.Index, obj);
			Write("]");
		}

		private void WriteBinaryOp(BinaryOperatorExpression expr, object obj)
		{
			Write("(");
			writeExpression.Invoke(expr.LeftHandSide, obj);
			Write(")");
			string val;
			if (binaryOperators.TryGetValue(expr.Operator, out val))
			{
				Write(val);
			}
			else
			{
				throw new NotSupportedException($"Operator {expr.Operator} is not supported.");
			}
			Write("(");
			writeExpression.Invoke(expr.RightHandSide, obj);
			Write(")");
		}
	}
}
