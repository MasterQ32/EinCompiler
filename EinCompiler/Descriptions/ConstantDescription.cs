using System;

namespace EinCompiler
{
	public sealed class ConstantDescription : DeclarationDescription
	{
		public ConstantDescription(
			TypeDescription type,
			string name,
			LiteralDescription initialValue) :
			base(name)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (initialValue == null) throw new ArgumentNullException(nameof(initialValue));
			this.Type = type;
			this.InitialValue = initialValue;
		}

		public TypeDescription Type { get; private set; }

		public LiteralDescription InitialValue { get; private set; }

		public override string ToString() =>
			$"{Type} {Name} = {InitialValue}";
	}
}