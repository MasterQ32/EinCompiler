using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawVariableNode : RawSyntaxNode
	{
		public RawVariableNode(string name, string type, string value, string[] modifiers) 
		{
			this.Name = name;
			this.Type = type;
			this.Value = value;
			this.Modifiers = modifiers.ToArray();
		}

		public string Name { get; private set; }

		public string Type { get; private set; }

		public string Value { get; private set; }

		public string[] Modifiers { get; private set; }

		public override string ToString() => $"{Type} {Name} = {Value}";
	}
}