namespace EinCompiler.RawSyntaxTree
{
	internal class ParameterDescription
	{
		private string name;
		private TypeDescription typeDescription;

		public ParameterDescription(TypeDescription typeDescription, string name)
		{
			this.typeDescription = typeDescription;
			this.name = name;
		}
	}
}