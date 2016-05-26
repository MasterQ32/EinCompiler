namespace EinCompiler
{
	public sealed class VariableExpression : Expression
	{
		public VariableExpression(VariableDescription variableDescription)
		{
			this.Variable = variableDescription;
		}

		public override bool IsAssignable => true;

		public VariableDescription Variable { get; private set; }
	}
}