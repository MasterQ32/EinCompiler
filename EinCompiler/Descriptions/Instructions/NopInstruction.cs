namespace EinCompiler
{
	public sealed class NopInstruction : InstructionDescription
	{
		public NopInstruction()
		{

		}

		public override string ToString() => "nop";
	}
}