namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawLocal : RawSyntaxNode
	{
		public RawLocal(Token name, RawTypeNode type)
		{
			this.Name = name;
			this.Type = type;
		}

		public Token Name { get; private set; }

		public RawTypeNode Type { get; private set; }
	}
}