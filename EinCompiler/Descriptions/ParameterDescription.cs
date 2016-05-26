namespace EinCompiler
{
	public sealed class ParameterDescription
	{
		public ParameterDescription(TypeDescription typeDescription, string name)
		{
			this.Type = typeDescription;
			this.Name = name;
		}

		public TypeDescription Type { get; private set; }

		public string Name { get; private set; }

		public override string ToString() => $"{Type} {Name}";
	}
}