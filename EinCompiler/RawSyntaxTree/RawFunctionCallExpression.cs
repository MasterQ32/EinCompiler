using System;
using System.Collections.Generic;
using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawFunctionCallExpression : RawExpressionNode
	{
		public RawFunctionCallExpression(
			string name, 
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
			return new FunctionCallExpression(
				funcs[this.Name],
				this.Arguments.Select(a => a.Translate(types, vars, funcs)).ToArray());
		}

		public IReadOnlyList<RawExpressionNode> Arguments { get; private set; }

		public string Name { get; private set; }
	}
}