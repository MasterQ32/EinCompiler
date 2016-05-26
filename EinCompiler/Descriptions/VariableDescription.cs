using System;

namespace EinCompiler
{
	public class VariableDescription : IDescription
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