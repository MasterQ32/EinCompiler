namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawParameterNode : RawSyntaxNode
	{
		public RawParameterNode(Token name, RawTypeNode type)
		{
			this.Name = name;
			this.Type = type;
		}

		public Token Name { get; private set; }
		public RawTypeNode Type { get; private set; }

		public override string ToString() => $"{Type} {Name}";
	}
}