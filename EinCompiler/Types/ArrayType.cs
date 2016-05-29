using System;

namespace EinCompiler
{
	public sealed class ArrayType : TypeDescription
	{
		public ArrayType(TypeDescription elementType, int length) : 
			base($"{elementType.Name}[{length}]")
        {
			if (elementType == null)
				throw new ArgumentNullException(nameof(elementType));
			if (length <= 0)
				throw new ArgumentOutOfRangeException(nameof(length));
			this.ElementType = elementType;
			this.Length = length;
		}
		
		public TypeDescription ElementType { get; private set; }

		public int Length { get; private set; }

		public override int Size => this.ElementType.Size * this.Length;

		public override byte[] GetBinary(object value)
		{
			throw new NotSupportedException();
		}

		public override string GetString(object value)
		{
			throw new NotSupportedException();
		}

		protected override object ParseValue(string text)
		{
			throw new NotSupportedException();
		}
	}
}