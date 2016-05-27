﻿using System;

namespace EinCompiler.RawSyntaxTree
{
	public abstract class RawExpressionNode
	{
		public abstract Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs);
	}

	public sealed class RawVariableExpressionNode : RawExpressionNode
	{
		public RawVariableExpressionNode(string variableName)
		{
			this.Variable = variableName;
		}

		public override Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs)
		{
			return new VariableExpression(vars[this.Variable]);
		}

		public string Variable { get; private set; }

		public override string ToString() => $"({Variable})";
	}

	public sealed class RawLiteralExpressionNode : RawExpressionNode
	{
		public RawLiteralExpressionNode(string literal)
		{
			this.Literal = literal;
		}

		public override Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs)
		{
			return new LiteralExpression(this.Literal);
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

		public override Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs)
		{
			throw new NotImplementedException();
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

		public override Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs)
		{
			if(this.Operator == "=")
			{
				return new AssignmentExpression(
					this.LeftHandSide.Translate(types, vars, funcs),
					this.RightHandSide.Translate(types, vars, funcs));
			}
			else
			{
				return new BinaryOperatorExpression(
					this.LeftHandSide.Translate(types, vars, funcs),
					GetOperator(this.Operator),
					this.RightHandSide.Translate(types, vars, funcs));
			}
		}

		private BinaryOperator GetOperator(string text)
		{
			switch(text)
			{
				case "+": return BinaryOperator.Addition;
				case "-": return BinaryOperator.Subtraction;
				case "*": return BinaryOperator.Multiplication;
				case "/": return BinaryOperator.Division;
				case "%": return BinaryOperator.EuclideanDivision;
				default: throw new NotSupportedException();
			}
		}

		public RawExpressionNode LeftHandSide { get; private set; }

		public string Operator { get; private set; }

		public RawExpressionNode RightHandSide { get; private set; }

		public override string ToString() => $"({LeftHandSide} {Operator} {RightHandSide})";
    }
}