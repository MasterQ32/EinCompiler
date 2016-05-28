using System;

namespace EinCompiler
{
	public sealed class LiteralExpression : Expression
	{
		private TypeDescription type = TypeDescription.Invalid;

		public LiteralExpression(string literal)
		{
			this.Literal = literal;
		}

		public string Literal { get; private set; }

		public override void DeduceAndCheckType(TypeDescription typeHint)
		{
			if (typeHint == null)
				throw new ArgumentNullException(nameof(typeHint), "Literals require a type hint to deduce types.");
			this.type = typeHint;

			// Check if the literal is convertible
			this.type.CreateValueFromString(this.Literal);
		}

		public ValueDescription GetValue() => this.type.CreateValueFromString(this.Literal);

		public override TypeDescription Type => this.type;

		public override string ToString()
		{
			return this.Type + "(" + this.Literal + ")";
		}
	}
}