using System;

namespace EinCompiler
{
	public sealed class ConstantDescription : IDescription
	{
		public ConstantDescription(
			TypeDescription type,
			string name,
			LiteralDescription initialValue) 
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (name == null) throw new ArgumentNullException(nameof(name));
			if (initialValue == null) throw new ArgumentNullException(nameof(initialValue));
			this.Type = type;
			this.Name = name;
			this.InitialValue = initialValue;
		}

		public string Name { get; private set; }

		public TypeDescription Type { get; private set; }

		public LiteralDescription InitialValue { get; private set; }

		public override string ToString() =>
			$"{Type} {Name} = {InitialValue}";
	}
}