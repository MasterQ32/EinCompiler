using System;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawIndexerExpression : RawExpressionNode
	{
		public RawIndexerExpression(Token token, RawExpressionNode rawExpressionNode)
		{
			this.Variable = token;
			this.Index = rawExpressionNode;
		}

		public Token Variable { get; private set; }

		public RawExpressionNode Index { get; private set; }

		public override Expression Translate(
			TypeContainer types, 
			VariableContainer vars, 
			FunctionContainer funcs)
		{
			var var = vars[this.Variable.Text];
			if ((var.Type is PointerType) == false)
				throw new SemanticException(this.Variable, "Variable must be a pointer variable.");
			return new IndexerExpression(
				var,
				this.Index.Translate(types, vars, funcs));
		}
	}
}