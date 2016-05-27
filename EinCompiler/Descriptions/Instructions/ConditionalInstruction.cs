namespace EinCompiler
{
	public sealed class ConditionalInstruction : InstructionDescription
	{
		public ConditionalInstruction(Expression condition, BodyDescription trueBody, BodyDescription falseBody)
		{
			this.Condition = condition;
			this.TrueBody = trueBody;
			this.FalseBody = falseBody;
		}

		public Expression Condition { get; private set; }

		public BodyDescription FalseBody { get; private set; }

		public BodyDescription TrueBody { get; private set; }
	}
}