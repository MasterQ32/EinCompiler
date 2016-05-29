namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawTypeNode : RawSyntaxNode
	{
		public RawTypeNode(
			Token name,
			Token arraySize)
		{
			this.Name = name;
			this.ArraySize = arraySize;
		}

		public Token ArraySize { get; private set; }

		public Token Name { get; private set; }

		public override string ToString()
		{
			if (ArraySize != null)
				return $"{Name.Text}[{ArraySize.Text}]";
			else
				return Name.Text;
		}
	}
}