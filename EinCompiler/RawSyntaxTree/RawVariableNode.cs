using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawVariableNode : RawSyntaxNode
	{
		public RawVariableNode(
			Token name,
			RawTypeNode type,
			Token value,
			Token[] modifiers) 
		{
			this.Name = name;
			this.Type = type;
			this.Value = value;
			this.Modifiers = modifiers.ToArray();
		}

		public Token Name { get; private set; }

		public RawTypeNode Type { get; private set; }

		public Token Value { get; private set; }

		public Token[] Modifiers { get; private set; }

		public override string ToString() => $"{Type} {Name.Text} = {Value.Text}";
	}
}