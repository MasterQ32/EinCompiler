using System;
using System.Globalization;

namespace EinCompiler
{
	public abstract class TypeDescription : IDescription
	{
		public static readonly TypeDescription Void = new VoidType();
		public static readonly TypeDescription Invalid = new InvalidType();

		protected TypeDescription(string name)
		{
			this.Name = name;
		}

		public ValueDescription CreateValueFromString(string text)
		{
			return new ValueDescription(
				this,
				this.ParseValue(text));
		}

		protected abstract byte[] ParseValue(string text);

		public string Name { get; private set; }
		
		public override string ToString() => this.Name;

		private sealed class VoidType : TypeDescription
		{
			public VoidType() : base("void") { }

			protected override byte[] ParseValue(string text)
			{
				throw new NotSupportedException("Not possible to create a void value.");
			}
		}

		private sealed class InvalidType : TypeDescription
		{
			public InvalidType() : base("<INVALID>") { }

			protected override byte[] ParseValue(string text)
			{
				throw new NotSupportedException("The invalid type does not support value creation.");
			}
		}
	}
}