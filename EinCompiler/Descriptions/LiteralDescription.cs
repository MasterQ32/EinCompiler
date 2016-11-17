using System;

namespace EinCompiler
{
	public sealed class LiteralDescription
	{
		public LiteralDescription(
			TypeDescription type,
			string value)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			this.Type = type;
			this.Value = value;

			if (type is IntegerType)
			{
				var itype = (IntegerType)type;
				long val = long.Parse(value);
				if (itype.IsSigned == false && val < 0)
					throw new InvalidCastException();
				if ((val & itype.Mask) != val)
					throw new InvalidCastException();
			}
			else if(type == Types.String)
			{
				
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public TypeDescription Type { get; private set; }

		public string Value { get; private set; }

		public override string ToString() => this.Value.ToString();
	}
}