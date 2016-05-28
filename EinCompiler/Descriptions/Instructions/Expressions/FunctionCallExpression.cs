using System;

namespace EinCompiler
{
	public sealed class FunctionCallExpression : Expression
	{
		public FunctionCallExpression(
			FunctionDescription function,
			Expression[] arguments)
		{
			this.Function = function;
			this.Arguments = arguments;

			if (this.Arguments.Length != this.Function.Parameters.Count)
				throw new InvalidOperationException();
		}

		public override bool IsTopLevelPossible => true;

		public FunctionDescription Function { get; private set; }

		public Expression[] Arguments { get; private set; }

		public override void DeduceAndCheckType(TypeDescription typeHint)
		{
			for(int i = 0; i < this.Arguments.Length; i++)
			{
				// Deduce each argument type.
				this.Arguments[i].DeduceAndCheckType(this.Function.Parameters[i].Type);
				if (this.Arguments[i].Type != this.Function.Parameters[i].Type)
					throw new InvalidOperationException("Parameter mismatch!");
			}
		}

		public override TypeDescription Type => this.Function.ReturnType;

		public override string ToString()
		{
			return this.Function.Name + "(" +
				string.Join<Expression>(", ", this.Arguments) +
				")";
		}
	}
}