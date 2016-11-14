using System;

namespace EinCompiler
{
	public abstract class ArrayType : TypeDescription
	{
		protected ArrayType (string name, TypeDescription elementType) : base(name)
		{
			this.ElementType = elementType;
		}

		public TypeDescription ElementType { get; private set; }
	}

	// int[10]
	public sealed class ArrayValueType : ArrayType
	{
		public ArrayValueType(TypeDescription elementType, int length) : base($"{elementType.Name}[{length}]", elementType)
		{
			this.Length = length;
		}

		public override int Size => this.Length * this.ElementType.Size;

		public int Length { get; private set; }
	}

	// int[]
	public sealed class ArrayReferenceType : ArrayType
	{
		public ArrayReferenceType(TypeDescription elementType) : base($"{elementType.Name}[]", elementType)
		{
			
		}
	}
}

