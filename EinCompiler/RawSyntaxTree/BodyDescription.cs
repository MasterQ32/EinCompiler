using System.Collections;
using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class BodyDescription : IReadOnlyList<InstructionDescription>
	{
		private readonly List<InstructionDescription> instructions;

		public BodyDescription(List<InstructionDescription> instructions)
		{
			this.instructions = new List<InstructionDescription>(instructions);
		}

		public InstructionDescription this[int index]
		{
			get
			{
				return instructions[index];
			}
		}

		public int Count
		{
			get
			{
				return instructions.Count;
			}
		}

		public IEnumerator<InstructionDescription> GetEnumerator()
		{
			return ((IReadOnlyList<InstructionDescription>)instructions).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IReadOnlyList<InstructionDescription>)instructions).GetEnumerator();
		}
	}
}