using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawFunctionNode : RawSyntaxNode
	{
		private List<RawParameterNode> parameters;

		public RawFunctionNode(
			string name,
			string returnType,
			List<RawParameterNode> parameters,
			List<RawLocal> locals,
			RawBodyNode body)
		{
			this.Name = name;
			this.ReturnType = returnType;
			this.parameters = new List<RawParameterNode>(parameters);
			this.Locals = new List<RawLocal>(locals);
			this.Body = body;
		}

		public string Name { get; private set; }

		public string ReturnType { get; private set; }

		public IReadOnlyList<RawParameterNode> Parameters => this.parameters;

		public bool IsExported { get; set; }

		public RawBodyNode Body { get; private set; }

		public IReadOnlyList<RawLocal> Locals { get; private set; }

		public override string ToString() => $"fn {ReturnType} {Name} ({string.Join(", ", this.Parameters)})";
	}
}