using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinCompiler
{
	public static class Types
	{
		public static readonly TypeDescription Void = new InvalidType("void");
		public static readonly TypeDescription Invalid = new InvalidType("<invalid>");

		public static readonly TypeDescription Int8 =  new IntegerType("int8", true, 1);
		public static readonly TypeDescription Int16 = new IntegerType("int16", true, 2);
		public static readonly TypeDescription Int32 = new IntegerType("int32", true, 4);

		public static readonly TypeDescription UInt8 = new IntegerType("uint8", false, 1);
		public static readonly TypeDescription UInt16 = new IntegerType("uint16", false, 2);
		public static readonly TypeDescription UInt32 = new IntegerType("uint32", false, 4);
		

		private sealed class InvalidType : TypeDescription
		{
			public InvalidType(string name) : base(name) { }

			public override int Size => 0;
		}
	}
}
