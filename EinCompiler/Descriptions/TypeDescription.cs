using System;
using System.Linq;
using System.Collections.Generic;

namespace EinCompiler
{
	public abstract class TypeDescription : IDescription
	{
		private readonly string name;
		private readonly int size;

		protected TypeDescription(string name, int size)
		{
			this.name = name;
			switch (size)
			{
				case 1:
				case 2:
				case 4:
					this.size = size;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(size));
			}
		}

		public int Size { get { return this.size; } }

		public string Name{ get { return this.name; } }

		public override string ToString()
		{
			return this.name;
		}
	}

	public class PointerType : TypeDescription
	{
		public PointerType(TypeDescription elementType) : 
			this(elementType, $"{elementType.Name}*")
		{

		}

		protected PointerType(TypeDescription elementType, string name) : 
			base(name, 4)
		{
			this.ElementType = elementType;
		}

		public TypeDescription ElementType { get; private set; }
	}

	public sealed class ArrayType : PointerType
	{
		public ArrayType(TypeDescription elementType, int? length)
			: base(elementType, elementType.Name + "[" + (length?.ToString() ?? "") + "]")
		{
			this.Length = length;
			if (length != null && length <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length));
			}
		}

		public int? Length { get; private set; }
	}

	public sealed class IntegerType : TypeDescription
	{
		public IntegerType(bool signed, int size)
			: base((signed ? "" : "u") + "int" + (size * 8), size)
		{
			this.IsSigned = signed;
			switch (size)
			{
				case 1:
					this.Mask = 0xFF;
					break;
				case 2:
					this.Mask = 0xFFFF;
					break;
				case 4:
					this.Mask = -1;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(size));
			}
		}

		public int Mask { get; private set; }

		public bool IsSigned { get; private set; }
	}
}

