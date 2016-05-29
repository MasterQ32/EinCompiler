using System;

namespace EinCompiler
{
	public sealed class ParameterDescription : VariableDescription
	{
		public ParameterDescription(
			TypeDescription type,
			string name,
			int position) : 
			base(type, name)
		{
			if (position < 0) throw new ArgumentOutOfRangeException(nameof(position));
			this.Index = position;
		}

		public int Index { get; private set; }

		public override string ToString()
		{
			return
				"Parameter " +
				this.Type.ToString() + " " +
				this.Name;
		}
	}
}