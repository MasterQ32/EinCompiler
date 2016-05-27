using System;

namespace EinCompiler
{
	public sealed class ValueDescription
	{
		public ValueDescription(
			TypeDescription type,
			byte[] value)
		{
			this.Type = type;
			this.Value = value;
		}

		public TypeDescription Type { get; private set; }

		public byte[] Value { get; private set; }

		public override string ToString() => BitConverter.ToString(this.Value);
	}
}