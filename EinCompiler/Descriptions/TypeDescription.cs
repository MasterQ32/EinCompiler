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

		public abstract string GetString(object value);

		public abstract byte[] GetBinary(object value);

		protected abstract object ParseValue(string text);

		public string Name { get; private set; }
		
		public abstract int Size { get; }

		public override string ToString() => this.Name;



		private sealed class VoidType : TypeDescription
		{
			public VoidType() : base("void") { }

			protected override object ParseValue(string text)
			{
				throw new NotSupportedException("Not possible to create a void value.");
			}

			public override byte[] GetBinary(object value)
			{
				throw new NotSupportedException();
			}

			public override string GetString(object value)
			{
				throw new NotSupportedException();
			}

			public override int Size => 0;
		}

		private sealed class InvalidType : TypeDescription
		{
			public InvalidType() : base("<INVALID>") { }

			protected override object ParseValue(string text)
			{
				throw new NotSupportedException("The invalid type does not support value creation.");
			}

			public override byte[] GetBinary(object value)
			{
				throw new NotSupportedException();
			}

			public override string GetString(object value)
			{
				throw new NotSupportedException();
			}

			public override int Size => 0;
		}
	}
}