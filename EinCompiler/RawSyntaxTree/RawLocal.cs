namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawLocal : RawSyntaxNode
	{
		public RawLocal(string name, string type)
		{
			this.Name = name;
			this.Type = type;
		}

		public string Name { get; private set; }

		public string Type { get; private set; }
	}
}