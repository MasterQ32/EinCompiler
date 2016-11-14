using System;

namespace EinCompiler
{
	public class ReferenceType : TypeDescription
	{
		public ReferenceType (string name, TypeDescription referencedType) : base(name)
		{
			this.ReferencedType = referencedType;
		}

		public TypeDescription ReferencedType  { get; private set; }
	}
}

