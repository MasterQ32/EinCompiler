using System;
using System.Linq;
using System.Collections.Generic;

namespace EinCompiler
{
	public abstract class TypeDescription : IDescription
	{
		private readonly string name;
		private readonly List<SubscriptDescription> subscripts = new List<SubscriptDescription> ();

		protected TypeDescription (string name)
		{
			this.name = name;
		}

		protected void AddSubscript(SubscriptDescription desc)
		{
			if (this.subscripts.Any (s => s.SubscriptType == desc.SubscriptType && s.Name == desc.Name)) {
				throw new InvalidOperationException ("A subscript with this name already exists.");
			}
			this.subscripts.Add (desc);
		}

		public abstract int Size { get; }

		public string Name => this.name;
	}
}

