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

	public sealed class RawReturnInstructionNode : RawInstructionNode
	{
		public RawReturnInstructionNode(RawExpressionNode expression)
		{
			this.Expression = expression;
		}

		public RawExpressionNode Expression { get; private set; }

		public override string ToString() => $"return {Expression}";
	}

	public sealed class RawIfInstructionNode : RawInstructionNode
	{
		public RawIfInstructionNode(
			RawExpressionNode condition,
			RawBodyNode trueBody,
			RawBodyNode falseBody)
		{
			this.TrueBody = trueBody;
			this.FalseBody = falseBody;
			this.Condition = condition;
		}

		public RawBodyNode TrueBody { get; private set; }

		public RawExpressionNode Condition { get; private set; }
		public RawBodyNode FalseBody { get; private set; }

		public override string ToString() => $"return ( {Condition} ) {{ {TrueBody} }} else {{ {FalseBody} }}";
    }
}