namespace EinCompiler
{
	public sealed class ParameterDescription : VariableDescription
	{
		public ParameterDescription(
			TypeDescription type,
			string name,
			int position) : 
			base(type, name)
		{
			this.Index = position;
		}

		public int Index { get; private set; }

		public override string ToString()
		{
			return
				"Parameter " +
				this.Type.ToString() + " " +
				this.Name;
		}
	}
}