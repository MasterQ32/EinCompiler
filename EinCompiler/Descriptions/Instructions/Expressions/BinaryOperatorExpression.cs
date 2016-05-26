namespace EinCompiler.RawSyntaxTree
{
	public sealed class BinaryOperatorExpression : Expression
	{
		public BinaryOperatorExpression(
			Expression lhs, 
			BinaryOperator op,
			Expression rhs)
		{
			this.LeftHandSide = lhs;
			this.Operator = op;
			this.RightHandSide = rhs;
		}

		public Expression LeftHandSide { get; private set; }
		public BinaryOperator Operator { get; private set; }
		public Expression RightHandSide { get; private set; }
	}

	public enum BinaryOperator
	{
		Addition,
		Subtraction,
		Multiplication,
		Division,
		EuclideanDivision
	}
}