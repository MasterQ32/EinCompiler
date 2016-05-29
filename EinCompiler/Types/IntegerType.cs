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
		private readonly int size;

		public IntegerType(string name, bool isSigned, int sizeInBytes) :
			base(name)
		{
			this.size = sizeInBytes;
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

		public override int Size => this.size;

		public override byte[] GetBinary(object value)
		{
			if (this.IsSigned)
			{
				switch (this.Size)
				{
					case 1:
						return BitConverter.GetBytes((SByte)value);
					case 2:
						return BitConverter.GetBytes((Int16)value);
					case 4:
						return BitConverter.GetBytes((Int32)value);
					default:
						throw new NotSupportedException();
				}
			}
			else
			{
				switch (this.Size)
				{
					case 1:
						return BitConverter.GetBytes((Byte)value);
					case 2:
						return BitConverter.GetBytes((UInt16)value);
					case 4:
						return BitConverter.GetBytes((UInt32)value);
					default:
						throw new NotSupportedException();
				}
			}
		}

		public override string GetString(object value) => Convert.ToString(value, CultureInfo.InvariantCulture);

		protected override object ParseValue(string text)
		{
			Func<string, NumberStyles, object> parse;
			if (this.IsSigned)
			{
				switch (this.Size)
				{
					case 1:
						parse = (t, n) => SByte.Parse(t, n);
						break;
					case 2:
						parse = (t, n) => Int16.Parse(t, n);
						break;
					case 4:
						parse = (t, n) => Int32.Parse(t, n);
						break;
					default:
						throw new NotSupportedException();
				}
			}
			else
			{
				switch (this.Size)
				{
					case 1:
						parse = (t, n) => Byte.Parse(t, n);
						break;
					case 2:
						parse = (t, n) => UInt16.Parse(t, n);
						break;
					case 4:
						parse = (t, n) => UInt32.Parse(t, n);
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
		
		public bool IsSigned { get; private set; }
	}
}
