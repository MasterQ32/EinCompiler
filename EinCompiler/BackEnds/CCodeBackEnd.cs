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
		readonly Dispatcher<InstructionDescription, object> writeInstruction = new Dispatcher<InstructionDescription, object>();
		readonly Dispatcher<Expression, object> writeExpression = new Dispatcher<Expression, object>();

		public CCodeBackEnd()
		{
			writeInstruction.Register<ExpressionInstruction> (WriteExpressionInstruction);



			writeExpression.Register<AssignmentExpression> (WriteAssignment);
			writeExpression.Register<VariableExpression> (WriteVariable);
			writeExpression.Register<LiteralExpression> (WriteLiteral);
			writeExpression.Register<FunctionCallExpression> (WriteCall);
			writeExpression.Register<IndexerExpression> (WriteIndexer);
		}

		protected override void WriteLineComment(string text)
		{
			WriteLine("// {0}", text);
		}

		private string GetTypeName(TypeDescription type)
		{
			return "int";
		}

		protected override void Generate(ModuleDescription module)
		{
			WriteLine ("#include <stdio.h>");
			WriteLine ("#include <stdlib.h>");
			WriteLine ("#include <stdint.h>");
			WriteLine ("#include <stddef.h>");


			foreach (var func in module.Functions) {

				WriteHeader (func);
				WriteLine ();
				if (func.NakedBody != null) {
					WriteLine ("{");
					IncreaseIndentation ();
					WriteLine (func.NakedBody);
					DecreaseIndentation ();
					WriteLine ("}");
				} else {
					WriteBody (func.Body);
				}
			}
		}

		private void WriteHeader(FunctionDescription func)
		{
			Write (GetTypeName (func.ReturnType));
			Write (" ");
			Write (func.Name);
			Write ("(");

			for (int i = 0; i < func.Parameters.Count; i++) {
				if (i > 0)
					Write (", ");
				var p = func.Parameters [i];
				Write (GetTypeName (p.Type));
				Write (" ");
				Write (p.Name);
			}

			Write (")");
		}

		private void WriteBody(BodyDescription body)
		{
			WriteLine ("{");
			IncreaseIndentation ();
			foreach (var i in body) {
				writeInstruction.Invoke (i, null);
			}
			DecreaseIndentation ();
			WriteLine ("}");
		}

		private void WriteExpressionInstruction(ExpressionInstruction instr, object obj)
		{
			writeExpression.Invoke (instr.Expression, obj);
			WriteLine (";");
		}

		private void WriteAssignment(AssignmentExpression expr, object obj)
		{
			writeExpression.Invoke (expr.Target, obj);
			Write (" = ");
			writeExpression.Invoke (expr.Source, obj);
		}

		private void WriteVariable(VariableExpression expr, object obj)
		{
			Write (expr.Variable.Name);
		}

		private void WriteLiteral(LiteralExpression expr, object obj)
		{
			Write (expr.Literal);
		}

		private void WriteCall(FunctionCallExpression expr, object obj)
		{
			Write (expr.Function.Name);
			Write ("(");
			for (int i = 0; i < expr.Arguments.Length; i++) {
				if (i > 0)
					Write (", ");
				writeExpression.Invoke (expr.Arguments [i], obj);
			}

			Write (")");
		}

		private void WriteIndexer(IndexerExpression expr, object obj)
		{
			Write (expr.Array.Name);
			Write ("[");
			writeExpression.Invoke (expr.Index, obj);
			Write ("]");
		}
	}
}
