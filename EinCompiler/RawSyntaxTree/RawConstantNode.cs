namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawConstantNode : RawSyntaxNode
	{
		public RawConstantNode(string name, string type, string value) 
		{
			this.Name = name;
			this.Type = type;
			this.Value = value;
		}

		public string Name { get; private set; }

		public string Type { get; private set; }

		public string Value { get; private set; }
	}
}