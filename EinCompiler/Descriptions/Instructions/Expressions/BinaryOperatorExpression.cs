using System;

namespace EinCompiler
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

		public override void DeduceAndCheckType(TypeDescription typeHint)
		{
			this.LeftHandSide.DeduceAndCheckType(typeHint);
			this.RightHandSide.DeduceAndCheckType(typeHint);

			if (this.LeftHandSide.Type != this.RightHandSide.Type)
				throw new InvalidOperationException();
		}

		public override TypeDescription Type => this.LeftHandSide.Type;

		public override string ToString()
		{
			return this.LeftHandSide + " [" + this.Operator + "] " + this.RightHandSide;
		}
	}

	public enum BinaryOperator
	{
		Addition,
		Subtraction,
		Multiplication,
		Division,
		EuclideanDivision,

		Equals,
		Differs,
		LessOrEqual,
		GreaterOrEqual,
		LessThan,
		GreaterThan
	}
}