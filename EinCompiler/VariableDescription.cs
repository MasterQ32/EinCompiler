namespace EinCompiler
{
	public sealed class VariableDescription : IDescription
	{
		public VariableDescription(TypeDescription type, string name)
		{
			this.Type = type;
			this.Name = name;
		}

		public VariableDescription(
			TypeDescription type, 
			string name,
			ValueDescription initialValue) : 
			this(type, name)
		{
			this.InitialValue = initialValue;
		}

		public string Name { get; private set; }

		public TypeDescription Type { get; private set; }

		public ValueDescription InitialValue { get; private set; }
	}
}