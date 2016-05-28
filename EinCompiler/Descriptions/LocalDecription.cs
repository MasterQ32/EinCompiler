namespace EinCompiler
{
	public sealed class LocalDecription : VariableDescription
	{
		public LocalDecription(
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
				"Local " +
				this.Type.ToString() + " " +
				this.Name;
		}
	}
}