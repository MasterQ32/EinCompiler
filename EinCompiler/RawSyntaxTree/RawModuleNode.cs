using System;
using System.Collections.Generic;
using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawModuleNode : RawSyntaxNode
	{
		private List<RawFunctionNode> functions = new List<RawFunctionNode>();
		private List<RawNakedFunctionNode> nakedFunctions = new List<RawNakedFunctionNode>();
		private List<RawExternFunctionNode> externFunctions = new List<RawExternFunctionNode>();
		private List<RawVariableNode> variables = new List<RawVariableNode>();
		private List<RawConstantNode> constants = new List<RawConstantNode>();
		private List<string> includes = new List<string>();

		public RawModuleNode()
		{

		}

		public void Add(RawSyntaxNode node)
		{
			if (node is RawFunctionNode)
			{
				functions.Add((RawFunctionNode)node);
			}
			else if (node is RawNakedFunctionNode)
			{
				nakedFunctions.Add((RawNakedFunctionNode)node);
			}
			else if (node is RawExternFunctionNode)
			{
				externFunctions.Add((RawExternFunctionNode)node);
			}
			else if (node is RawVariableNode)
			{
				variables.Add((RawVariableNode)node);
			}
			else if (node is RawConstantNode)
			{
				constants.Add((RawConstantNode)node);
			}
			else if(node is RawIncludeNode)
			{
				includes.Add(((RawIncludeNode)node).FileName);
			}
			else
			{
				throw new NotSupportedException($"{node} is not a supported node type.");
			}
		}

		public ICollection<RawFunctionNode> Functions => this.functions;

		public ICollection<RawNakedFunctionNode> NakedFunctions => this.nakedFunctions;

		public ICollection<RawExternFunctionNode> ExternFunctions => this.externFunctions;

		public ICollection<RawVariableNode> Variables => this.variables;

		public ICollection<RawConstantNode> Constants => this.constants;

		public ICollection<string> Includes => this.includes;
	}
}