using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawBodyNode : RawSyntaxNode
	{
		private readonly List<RawInstructionNode> instructions;

		public RawBodyNode(IEnumerable<RawInstructionNode> instructions)
		{
			 this.instructions = new List<RawInstructionNode>(instructions);
		}

		public IReadOnlyList<RawInstructionNode> Instructions => this.instructions;
	}
}