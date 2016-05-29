using System;

namespace EinCompiler
{
	public sealed class ValueDescription
	{
		public ValueDescription(
			TypeDescription type,
			object value)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (value == null) throw new ArgumentNullException(nameof(value));
			this.Type = type;
			this.Value = value;
		}

		public byte[] GetBinary()
		{
			return this.Type.GetBinary(this.Value);
		}

		public string GetString()
		{
			return this.Type.GetString(this.Value);
		}

		public TypeDescription Type { get; private set; }

		public object Value { get; private set; }
		
		public override string ToString() => this.Value.ToString();
	}
}