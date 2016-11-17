using System;

namespace EinCompiler
{
	public sealed class LiteralExpression : Expression
	{
		private TypeDescription type = Types.Invalid;

		public LiteralExpression(string literal)
		{
			if (literal == null) throw new ArgumentNullException(nameof(literal));
			this.Value = literal;
		}

		public string Value { get; private set; }

		public LiteralDescription Literal { get; private set; }

		public override void DeduceAndCheckType(TypeDescription typeHint)
		{
			if (typeHint == null)
				throw new ArgumentNullException(nameof(typeHint), "Literals require a type hint to deduce types.");
			this.type = typeHint;

			this.Literal = new LiteralDescription(this.type, this.Value);
		}

		public override TypeDescription Type => this.type;

		public override string ToString()
		{
			return this.Type + "(" + this.Literal + ")";
		}
	}
}