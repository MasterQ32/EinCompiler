using System;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawNopInstructionNode : RawInstructionNode
	{
		public override InstructionDescription Translate(TypeContainer types)
		{
			throw new NotImplementedException();
		}

		public override string ToString() => "nop";
	}
}