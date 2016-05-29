using System;
using System.Collections.Generic;
using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawFunctionCallExpression : RawExpressionNode
	{
		public RawFunctionCallExpression(
			Token name, 
			RawExpressionNode[] arguments)
		{
			this.Name = name;
			this.Arguments = arguments;
		}

		public override Expression Translate(
			TypeContainer types, 
			VariableContainer vars,
			FunctionContainer funcs)
		{
			var func = funcs[this.Name.Text];
			if (func == null)
				throw new SemanticException(this.Name, "Use of undeclared function.");
			return new FunctionCallExpression(
				func,
				this.Arguments.Select(a => a.Translate(types, vars, funcs)).ToArray());
		}

		public IReadOnlyList<RawExpressionNode> Arguments { get; private set; }

		public Token Name { get; private set; }
	}
}