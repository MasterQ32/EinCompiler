namespace EinCompiler
{
	public sealed class LoopInstruction : InstructionDescription
	{
		public LoopInstruction(Expression condition, BodyDescription body)
		{
			this.Condition = condition;
			this.Body  = body;
		}

		public BodyDescription Body { get; private set; }

		public Expression Condition { get; private set; }
	}
}