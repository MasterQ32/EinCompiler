using System;

namespace EinCompiler
{
	public sealed class FloatingPointType : PrimitiveType
	{
		public static readonly FloatingPointType Half = new FloatingPointType("half", 2, 10);

		public static readonly FloatingPointType Single = new FloatingPointType("single", 4, 23);

		public static readonly FloatingPointType Double = new FloatingPointType("double", 8, 52);

		private readonly int size, mantissa, exponent;

		private FloatingPointType (string name, int size, int mantissa) : base(name)
		{
			this.size = size;
			this.mantissa = mantissa;
			this.exponent = 8 * this.size - this.mantissa - 1;

			this.AddSubscript(new SubscriptDescription(
				SubscriptType.Type,
				"mantissa",
				new LiteralExpression(this.mantissa.ToString())));

			this.AddSubscript(new SubscriptDescription(
				SubscriptType.Type,
				"exponent",
				new LiteralExpression(this.exponent.ToString())));
		}

		public  override int Size => this.size;
	}
}

