using System;
using System.Globalization;

namespace EinCompiler
{
	public sealed class TypeDescription : IDescription
	{
		public TypeDescription(string name)
		{
			this.Name = name;
		}

		public string Name { get; private set; }

		public ValueDescription CreateValueFromString(string text)
		{
			object value;
			switch(this.Name)
			{
				case "int":
				{
					if (text.StartsWith("0x"))
						value = UInt32.Parse(text.Substring(2), NumberStyles.HexNumber);
					else
						value = UInt32.Parse(text, NumberStyles.Number);
					break;
				}
				default:
				{
					throw new NotSupportedException();
				}
			}
			return new ValueDescription(
				this,
				value);
		}

		public override string ToString() => this.Name;
	}
}