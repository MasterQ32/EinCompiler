namespace EinCompiler
{
	public sealed class ReturnInstruction : InstructionDescription
	{
		public ReturnInstruction(Expression expression)
		{
			this.Expression = expression;
		}

		public Expression Expression { get; private set; }
	}
}