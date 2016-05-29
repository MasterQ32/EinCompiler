using System.Collections.Generic;
using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawNakedFunctionNode : RawBasicFunctionNode
	{
		public RawNakedFunctionNode(
			Token name,
			RawTypeNode returnType,
			List<RawParameterNode> parameters,
			string body) : 
			base(name, returnType, parameters)
		{
			this.Body = body;
		}

		public bool IsInline { get; set; }

		public string Body { get; private set; }

		public override string ToString() => $"fn {ReturnType} {Name} ({string.Join(", ", this.Parameters)})";
	}
}
