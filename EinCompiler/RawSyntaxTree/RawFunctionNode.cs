using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawFunctionNode : RawSyntaxNode
	{
		private List<RawParameterNode> parameters;

		public RawFunctionNode(
			string name, 
			string type, 
			List<RawParameterNode> parameters,
			RawBodyNode body)
		{
			this.Name = name;
			this.Type = type;
			this.parameters = new List<RawParameterNode>( parameters);
			this.Body = body;
		}

		public string Name { get; private set; }

		public string Type { get; private set; }

		public IReadOnlyList<RawParameterNode> Parameters => this.parameters;

		public bool IsExported { get; set; }

		public RawBodyNode Body { get; private set; }

		public override string ToString() => $"fn {Type} {Name} ({string.Join(", ", this.Parameters)})";
	}
}