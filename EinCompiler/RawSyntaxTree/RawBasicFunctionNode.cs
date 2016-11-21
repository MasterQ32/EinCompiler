using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public abstract class RawBasicFunctionNode : RawDeclarationNode
	{
		private List<RawParameterNode> parameters;

		public RawBasicFunctionNode(
			Token name,
			RawTypeNode returnType,
			List<RawParameterNode> parameters)
		{
			this.Name = name;
			this.ReturnType = returnType;
			this.parameters = new List<RawParameterNode>(parameters);
		}

		public Token Name { get; private set; }

		public RawTypeNode ReturnType { get; private set; }

		public IReadOnlyList<RawParameterNode> Parameters => this.parameters;

		public override string ToString() => $"fn {ReturnType} {Name} ({string.Join(", ", this.Parameters)})";
	}
}