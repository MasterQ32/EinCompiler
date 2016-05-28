namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawIncludeNode : RawSyntaxNode
	{
		public RawIncludeNode(string fileName)
		{
			this.FileName = fileName;
		}

		public string FileName { get; private set; }
	}
}