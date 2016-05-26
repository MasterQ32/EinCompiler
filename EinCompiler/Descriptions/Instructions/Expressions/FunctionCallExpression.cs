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

			if (this.Arguments.Length != this.Function.Parameters.Length)
				throw new InvalidOperationException();
		}

		public override bool IsTopLevelPossible => true;

		public FunctionDescription Function { get; private set; }

		public Expression[] Arguments { get; private set; }
	}
}