namespace EinCompiler
{
	public sealed class ConstantDescription : IDescription
	{
		public ConstantDescription(
			TypeDescription type,
			string name,
			ValueDescription initialValue) 
		{
			this.Type = type;
			this.Name = name;
			this.InitialValue = initialValue;
		}

		public string Name { get; private set; }

		public TypeDescription Type { get; private set; }

		public ValueDescription InitialValue { get; private set; }

		public override string ToString() =>
			$"{Type} {Name} = {InitialValue}";
	}
}