using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinCompiler.Types
{
	public sealed class IntegerType : TypeDescription
	{
		public IntegerType(string name, bool isSigned, int sizeInBytes) :
			base(name)
		{
			this.SizeInBytes = sizeInBytes;
			this.IsSigned = isSigned;
			switch (sizeInBytes)
			{
				case 1:
				case 2:
				case 4:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(sizeInBytes), "The size in bytes must be 1, 2 or 4");
			}
		}

		protected override byte[] ParseValue(string text)
		{
			Func<string, NumberStyles, byte[]> parse;
			if (this.IsSigned)
			{
				switch (this.SizeInBytes)
				{
					case 1:
						parse = (t, n) => BitConverter.GetBytes(SByte.Parse(t, n));
						break;
					case 2:
						parse = (t, n) => BitConverter.GetBytes(Int16.Parse(t, n));
						break;
					case 4:
						parse = (t, n) => BitConverter.GetBytes(Int32.Parse(t, n));
						break;
					default:
						throw new NotSupportedException();
				}
			}
			else
			{
				switch (this.SizeInBytes)
				{
					case 1:
						parse = (t, n) => BitConverter.GetBytes(Byte.Parse(t, n));
						break;
					case 2:
						parse = (t, n) => BitConverter.GetBytes(UInt16.Parse(t, n));
						break;
					case 4:
						parse = (t, n) => BitConverter.GetBytes(UInt32.Parse(t, n));
						break;
					default:
						throw new NotSupportedException();
				}
			}

			if (text.StartsWith("0x"))
				return parse(text.Substring(2), NumberStyles.HexNumber);
			else
				return parse(text, NumberStyles.Number);
		}

		public int SizeInBytes { get; private set; }
		public bool IsSigned { get; private set; }
	}
}
