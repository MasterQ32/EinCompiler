using System;

namespace EinCompiler.RawSyntaxTree
{
	public abstract class RawExpressionNode
	{
		public abstract Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs);
	}

	public sealed class RawVariableExpressionNode : RawExpressionNode
	{
		public RawVariableExpressionNode(Token variableName)
		{
			this.Variable = variableName;
		}

		public override Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs)
		{
			var var = vars[this.Variable.Text];
			if (var == null)
				throw new SemanticException(this.Variable, "Variable not declared in this scope.");
			return new VariableExpression(var);
		}

		public Token Variable { get; private set; }

		public override string ToString() => $"({Variable})";
	}

	public sealed class RawLiteralExpressionNode : RawExpressionNode
	{
		public RawLiteralExpressionNode(Token literal)
		{
			this.Literal = literal;
		}

		public override Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs)
		{
			var text = this.Literal.Text;
			if(text.StartsWith("'"))
			{
				if(text.Length == 4)
				{
					switch(text[2])
					{
						case 't': text = ((int)'\t').ToString(); break;
						case 'n': text = ((int)'\n').ToString(); break;
						case 'r': text = ((int)'\r').ToString(); break;
						case 'b': text = ((int)'\b').ToString(); break;
						default: throw new SemanticException(this.Literal, "Unrecognized escape code.");
					}
				}
				else
				{
					text = ((int)text[1]).ToString();
				}
			}

			return new LiteralExpression(text);
		}

		public Token Literal { get; private set; }

		public override string ToString() => $"({Literal})";
	}

	public sealed class RawUnaryOperatorExpressionNode : RawExpressionNode
	{
		public RawUnaryOperatorExpressionNode(Token op, RawExpressionNode expression)
		{
			this.Operator = op;
			this.Expression = expression;
		}

		public override Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs)
		{
			throw new NotImplementedException();
		}

		public RawExpressionNode Expression { get; private set; }

		public Token Operator { get; private set; }

		public override string ToString() => $"({Operator} {Expression})";
	}

	public sealed class RawBinaryOperatorExpressionNode : RawExpressionNode
	{
		public RawBinaryOperatorExpressionNode(
			Token op, 
			RawExpressionNode lhs,
			RawExpressionNode rhs)
		{
			this.Operator = op;
			this.LeftHandSide = lhs;
			this.RightHandSide = rhs;
		}

		public override Expression Translate(TypeContainer types, VariableContainer vars, FunctionContainer funcs)
		{
			var op = this.Operator.Text;
			if(op == ":=")
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

		private BinaryOperator GetOperator(Token token)
		{
			switch(token.Text)
			{
				// Arithmetic
				case "+": return BinaryOperator.Addition;
				case "-": return BinaryOperator.Subtraction;
				case "*": return BinaryOperator.Multiplication;
				case "/": return BinaryOperator.Division;
				case "%": return BinaryOperator.EuclideanDivision;

				// Relational
				case "=": return BinaryOperator.Equals;
				case "!=": return BinaryOperator.Differs;
				case ">=": return BinaryOperator.GreaterOrEqual;
				case "<=": return BinaryOperator.LessOrEqual;
				case "<": return BinaryOperator.LessThan;
				case ">": return BinaryOperator.GreaterThan;
				default: throw new SemanticException(token, "Unrecognized binary operator.");
			}
		}

		public RawExpressionNode LeftHandSide { get; private set; }

		public Token Operator { get; private set; }

		public RawExpressionNode RightHandSide { get; private set; }

		public override string ToString() => $"({LeftHandSide} {Operator} {RightHandSide})";
    }
}