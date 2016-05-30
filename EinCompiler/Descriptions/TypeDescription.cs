using System;
using System.Globalization;

namespace EinCompiler
{
	public abstract class TypeDescription : IDescription
	{

		protected TypeDescription(string name)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));
			this.Name = name;
		}

		public ValueDescription CreateValueFromString(string text)
		{
			if (text == null) throw new ArgumentNullException(nameof(text));
			return new ValueDescription(
				this,
				this.ParseValue(text));
		}

		public abstract string GetString(object value);

		public abstract byte[] GetBinary(object value);

		protected abstract object ParseValue(string text);

		public string Name { get; private set; }
		
		public abstract int Size { get; }

		public override string ToString() => this.Name;
	}
}