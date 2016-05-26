namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawParameterNode : RawSyntaxNode
	{
		public RawParameterNode(string name, string type)
		{
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; private set; }
		public string Type { get; private set; }

		public override string ToString() => $"{Type} {Name}";
	}
}