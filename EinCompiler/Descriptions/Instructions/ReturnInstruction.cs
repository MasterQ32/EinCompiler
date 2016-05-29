using System;

namespace EinCompiler
{
	public sealed class ReturnInstruction : InstructionDescription
	{
		public ReturnInstruction(Expression expression)
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));
			this.Expression = expression;
		}

		public Expression Expression { get; private set; }
	}
}