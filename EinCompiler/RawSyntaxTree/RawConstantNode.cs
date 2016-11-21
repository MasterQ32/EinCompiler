namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawConstantNode : RawDeclarationNode
	{
		public RawConstantNode(
			Token name, 
			RawTypeNode type, 
			Token value) 
		{
			this.Name = name;
			this.Type = type;
			this.Value = value;
		}

		public Token Name { get; private set; }

		public RawTypeNode Type { get; private set; }

		public Token Value { get; private set; }
	}
}