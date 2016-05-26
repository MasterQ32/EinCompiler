namespace EinCompiler.RawSyntaxTree
{
	public abstract class RawInstructionNode : RawSyntaxNode
	{
		
	}

	public sealed class RawExpressionInstructionNode : RawInstructionNode
	{
		public RawExpressionInstructionNode(RawExpressionNode expression)
		{
			this.Expression = expression;
		}

		public RawExpressionNode Expression { get; private set; }

		public override string ToString() => $"{Expression}";
	}
}