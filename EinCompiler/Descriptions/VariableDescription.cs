using System;

namespace EinCompiler
{
	public class VariableDescription : DeclarationDescription
	{
		public VariableDescription(TypeDescription type, string name) : base(name)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			this.Type = type;
		}

		public VariableDescription(
			TypeDescription type, 
			string name,
			LiteralDescription initialValue) : 
			this(type, name)
		{
			this.InitialValue = initialValue;
		}

		public TypeDescription Type { get; private set; }

		public LiteralDescription InitialValue { get; private set; }

		public StorageModifier Storage { get; set; }

		public override string ToString()
		{
			var str =
				this.Storage.ToString().Replace(",", "") + " " +
				this.Type.ToString() + " " +
				this.Name;
			if (this.InitialValue != null)
				str += " = " + this.InitialValue.ToString();
			return str;
		}
	}

	[Flags]
	public enum StorageModifier
	{
		Global = 0,
		Private = 1,
		Static = 2,
		Shared = 4,
	}
}