using System;
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

		public BodyDescription Translate(
			TypeContainer types, 
			VariableContainer variables,
			FunctionContainer funcs)
		{
			var instructions = new List<InstructionDescription>();
			foreach (var instr in this.Instructions)
			{
				instructions.Add(
					instr.Translate(types, variables, funcs));
			}
			return new BodyDescription(instructions);
		}
	}
}