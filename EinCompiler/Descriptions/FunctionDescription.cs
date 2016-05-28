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

		public FunctionDescription(
			string name,
			TypeDescription returnType,
			ParameterDescription[] parameters,
			string body) :
			this(name, returnType, parameters)
		{
			this.NakedBody = body;
		}

		public BodyDescription Body { get; set; }

		public string Name { get; private set; }

		public bool IsNaked => (this.NakedBody != null);

		public bool IsInline { get; set; }

		public ParameterDescription[] Parameters { get; private set; }

		public TypeDescription ReturnType { get; private set; }

		public string NakedBody { get; private set; }

		public override string ToString() =>
			$"{ReturnType} {Name}({string.Join<ParameterDescription>(", ", Parameters)})";
	}
}