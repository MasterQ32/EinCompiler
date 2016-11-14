using System;

namespace EinCompiler
{
	public sealed class PointerType : PrimitiveType
	{
		public PointerType(string name, TypeDescription referencedType) : base(name)
		{
			this.ReferencedType = referencedType;
		}

		public TypeDescription ReferencedType { get; private set; }
	}
}

