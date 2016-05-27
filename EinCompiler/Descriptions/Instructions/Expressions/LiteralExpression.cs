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
		}

		public override TypeDescription Type => this.type;
	}
}