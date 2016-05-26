namespace EinCompiler.RawSyntaxTree
{
	public sealed class ParameterVariableDescription : VariableDescription
	{
		public ParameterVariableDescription(
			TypeDescription type,
			string name,
			int position) : 
			base(type, name)
		{
			this.Index = position;
		}

		public int Index { get; private set; }
	}
}