﻿namespace EinCompiler
{
	public sealed class ExpressionInstruction : InstructionDescription
	{
		public ExpressionInstruction(Expression expression)
		{
			this.Expression = expression;
		}

		public Expression Expression { get; private set; }

		public override string ToString() => this.Expression.ToString();
	}
}