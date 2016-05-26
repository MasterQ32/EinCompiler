namespace EinCompiler
{
	public sealed class ParameterDescription
	{
		private string name;
		private TypeDescription typeDescription;

		public ParameterDescription(TypeDescription typeDescription, string name)
		{
			this.typeDescription = typeDescription;
			this.name = name;
		}

		public TypeDescription Type { get; private set; }

		public string Name { get; private set; }

		public override string ToString() => $"{Type} {Name}";
	}
}