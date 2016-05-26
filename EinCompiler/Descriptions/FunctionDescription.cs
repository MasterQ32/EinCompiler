namespace EinCompiler
{
	public sealed class FunctionDescription : IDescription
	{
		public FunctionDescription(
			string name,
			TypeDescription returnType,
			ParameterDescription[] parameters)
		{
			this.Name = name;
			this.ReturnType = returnType;
			this.Parameters = parameters;
		}

		public FunctionDescription(
			string name,
			TypeDescription returnType, 
			ParameterDescription[] parameters,
			BodyDescription body) : 
			this(name, returnType, parameters)
		{
			this.Body = body;
		}

		public BodyDescription Body { get; set; }

		public string Name { get; private set; }

		public ParameterDescription[] Parameters { get; private set; }

		public TypeDescription ReturnType { get; private set; }

		public override string ToString() =>
			$"{ReturnType} {Name}({string.Join<ParameterDescription>(", ", Parameters)})";
	}
}