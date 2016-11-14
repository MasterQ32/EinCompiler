using System;

namespace EinCompiler
{
	public class StringType : TypeDescription
	{
		public static readonly StringType Instance = new StringType();

		private StringType () : base("string")
		{
			this.AddSubscript(new SubscriptDescription(
				SubscriptType.Instance,
				"length",
				null));
			this.AddSubscript(new SubscriptDescription(
				SubscriptType.Instance,
				"data",
				null));
		}
	}
}

