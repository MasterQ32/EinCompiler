namespace EinCompiler
{
	public sealed class ExpressionInstruction : InstructionDescription
	{
		private Expression expression;

		public ExpressionInstruction(Expression expression)
		{
			this.expression = expression;
		}

		public Expression Expression { get; private set; }
	}
}