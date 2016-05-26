namespace EinCompiler
{
	public sealed class UntypedLiteralExpression : Expression
	{
		public UntypedLiteralExpression(string literal)
		{
			this.Literal = literal;
		}

		public string Literal { get; private set; }
	}
}