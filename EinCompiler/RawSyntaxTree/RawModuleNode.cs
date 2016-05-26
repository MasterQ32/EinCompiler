using System;
using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawModuleNode : RawSyntaxNode
	{
		private List<RawFunctionNode> functions = new List<RawFunctionNode>();
		private List<RawVariableNode> variables = new List<RawVariableNode>();
		private List<RawConstantNode> constants = new List<RawConstantNode>();

		public RawModuleNode()
		{

		}

		public void Add(RawSyntaxNode node)
		{
			if(node is RawFunctionNode)
			{
				functions.Add((RawFunctionNode)node);
			}
			else if (node is RawVariableNode)
			{
				variables.Add((RawVariableNode)node);
			}
			else if (node is RawConstantNode)
			{
				constants.Add((RawConstantNode)node);
			}
			else
			{
				throw new NotSupportedException($"{node} is not a supported node type.");
			}
		}

		public ICollection<RawFunctionNode> Functions => this.functions;

		public ICollection<RawVariableNode> Variables => this.variables;

		public ICollection<RawConstantNode> Constants => this.constants;
	}
}