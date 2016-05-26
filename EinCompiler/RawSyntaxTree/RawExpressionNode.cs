namespace EinCompiler.RawSyntaxTree
{
	public abstract class RawExpressionNode
	{

	}

	public sealed class RawVariableExpressionNode : RawExpressionNode
	{
		public RawVariableExpressionNode(string variableName)
		{
			this.Variable = variableName;
		}

		public string Variable { get; private set; }

		public override string ToString() => $"({Variable})";
	}

	public sealed class RawLiteraltExpressionNode : RawExpressionNode
	{
		public RawLiteraltExpressionNode(string literal)
		{
			this.Literal = literal;
		}

		public string Literal { get; private set; }

		public override string ToString() => $"({Literal})";
	}

	public sealed class RawUnaryOperatorExpressionNode : RawExpressionNode
	{
		public RawUnaryOperatorExpressionNode(string op, RawExpressionNode expression)
		{
			this.Operator = op;
			this.Expression = expression;
		}

		public RawExpressionNode Expression { get; private set; }

		public string Operator { get; private set; }

		public override string ToString() => $"({Operator} {Expression})";
	}

	public sealed class RawBinaryOperatorExpressionNode : RawExpressionNode
	{
		public RawBinaryOperatorExpressionNode(
			string op, 
			RawExpressionNode lhs,
			RawExpressionNode rhs)
		{
			this.Operator = op;
			this.LeftHandSide = lhs;
			this.RightHandSide = rhs;
		}

		public RawExpressionNode LeftHandSide { get; private set; }

		public string Operator { get; private set; }

		public RawExpressionNode RightHandSide { get; private set; }

		public override string ToString() => $"({LeftHandSide} {Operator} {RightHandSide})";
    }
}