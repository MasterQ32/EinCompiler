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

		public static readonly TypeDescription Int8 = new IntegerType(true, 1);
		public static readonly TypeDescription Int16 = new IntegerType(true, 2);
		public static readonly TypeDescription Int32 = new IntegerType(true, 4);

		public static readonly TypeDescription UInt8 = new IntegerType(false, 1);
		public static readonly TypeDescription UInt16 = new IntegerType(false, 2);
		public static readonly TypeDescription UInt32 = new IntegerType(false, 4);

		public static readonly TypeDescription Pointer = new IntegerType(false, 4);

		public static readonly TypeDescription String = new ArrayType(Types.UInt8, null);

		private sealed class InvalidType : TypeDescription
		{
			public InvalidType(string name)
				: base(name, 1)
			{

			}
		}
	}
}
