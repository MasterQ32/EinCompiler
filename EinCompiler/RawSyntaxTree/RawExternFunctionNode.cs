using System;
using System.Collections.Generic;

namespace EinCompiler.RawSyntaxTree
{
	public class RawExternFunctionNode : RawBasicFunctionNode 
	{
		public RawExternFunctionNode(
			Token name,
			RawTypeNode returnType,
			List<RawParameterNode> parameters) : 
		base(name, returnType, parameters)
		{
			
		}
	}
}

