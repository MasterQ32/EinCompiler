using System;
using System.Collections.Generic;

namespace EinCompiler
{
	public abstract class DeclarationDescription : IDescription
	{
		private readonly string name;
		private readonly HashSet<AttributeDeclaration> attributes = new HashSet<AttributeDeclaration>();

		protected DeclarationDescription (string name)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));
			this.name = name;	
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get { return this.name; } }

		/// <summary>
		/// Gets a set of attributes attached to this declaration.
		/// </summary>
		/// <value>The attributes.</value>
		public HashSet<AttributeDeclaration> Attributes { get { return this.attributes; } }
	}
}

