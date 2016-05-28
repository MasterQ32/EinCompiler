using System.Collections.Generic;
using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawNakedFunctionNode : RawSyntaxNode
	{
		private List<RawParameterNode> parameters;

		public RawNakedFunctionNode(
			string name,
			string returnType,
			List<RawParameterNode> parameters,
			string body)
		{
			this.Name = name;
			this.ReturnType = returnType;
			this.parameters = new List<RawParameterNode>(parameters);
			this.Body = body;
		}

		public string Name { get; private set; }

		public string ReturnType { get; private set; }

		public IReadOnlyList<RawParameterNode> Parameters => this.parameters;

		public bool IsInline { get; set; }

		public string Body { get; private set; }

		public override string ToString() => $"fn {ReturnType} {Name} ({string.Join(", ", this.Parameters)})";
	}
}
