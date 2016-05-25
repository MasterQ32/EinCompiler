using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawFunctionNode : RawSyntaxNode
	{
		private List<RawParameterNode> parameters;

		public RawFunctionNode(string text1, string text2, List<RawParameterNode> parameters)
		{
			this.Name = text1;
			this.Type = text2;
			this.parameters = new List<RawParameterNode>( parameters);
		}

		public string Name { get; private set; }

		public string Type { get; private set; }

		public IReadOnlyList<RawParameterNode> Parameters => this.parameters;

		public bool IsExported { get; internal set; }
	}
}