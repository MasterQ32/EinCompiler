using System;

namespace EinCompiler
{
	public sealed class SubscriptDescription : IDescription
	{
		public SubscriptDescription(SubscriptType subtype, string name, Expression value)
		{
			this.Name = name;
			this.SubscriptType = subtype;
			this.Value = value;
		}

		public string Name { get; private set; }

		public SubscriptType SubscriptType { get; private set; }

		public TypeDescription Type => this.Value.Type;

		public Expression Value { get; private set; }

		public override string ToString ()
		{
			switch (SubscriptType) {
			case SubscriptType.Instance:
				return $".{Name}";
			case SubscriptType.Type:
				return $"'{Name}";
			default:
				throw new NotSupportedException ();
			}
		}
	}

}

