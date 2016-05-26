namespace EinCompiler.RawSyntaxTree
{
	internal class FunctionDescription
	{
		public FunctionDescription(
			string name,
			TypeDescription returnType, 
			ParameterDescription[] parameters,
			BodyDescription body)
		{
			this.name = name;
			this.ReturnType = returnType;
			this.Parameters = parameters;
			this.Body = body;
		}

		public BodyDescription Body { get; private set; }

		public string name { get; private set; }

		public ParameterDescription[] Parameters { get; private set; }

		public TypeDescription ReturnType { get; private set; }
	}
}