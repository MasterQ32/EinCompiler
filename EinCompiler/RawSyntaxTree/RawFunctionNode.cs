using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawFunctionNode : RawBasicFunctionNode
	{
		private List<RawParameterNode> parameters;

		public RawFunctionNode(
			Token name,
			RawTypeNode returnType,
			List<RawParameterNode> parameters,
			List<RawLocal> locals,
			RawBodyNode body) : 
			base(name, returnType, parameters)
		{
			this.Locals = new List<RawLocal>(locals);
			this.Body = body;
		}

		public bool IsExported { get; set; }

		public RawBodyNode Body { get; private set; }

		public IReadOnlyList<RawLocal> Locals { get; private set; }
	}
}