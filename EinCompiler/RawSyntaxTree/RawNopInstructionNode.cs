using System;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawNopInstructionNode : RawInstructionNode
	{
		public override InstructionDescription Translate(
			TypeContainer types, 
			VariableContainer vars,
			FunctionContainer funcs)
		{
			return new NopInstruction();
		}

		public override string ToString() => "nop";
	}
}