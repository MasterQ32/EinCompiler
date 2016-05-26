namespace EinCompiler
{
	public sealed class ValueDescription
	{
		public ValueDescription(
			TypeDescription type,
			object value)
		{
			this.Type = type;
			this.Value = value;
		}

		public TypeDescription Type { get; private set; }

		public object Value { get; private set; }
	}
}