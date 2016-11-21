using System;
using System.Linq;
using System.Collections.Generic;

namespace EinCompiler
{
	public abstract class RawDeclarationNode : RawSyntaxNode
	{
		private List<AttributeDeclaration> attributes = new List<AttributeDeclaration>();

		protected RawDeclarationNode ()
		{

		}

		public List<AttributeDeclaration> Attributes
		{ 
			get { return this.attributes; } 
			set { this.attributes = value; }
		}
	}

	public sealed class AttributeDeclaration
	{
		private readonly string name;
		private readonly List<string> arguments;

		public AttributeDeclaration(string name, params string[] arguments) : 
			this(name, (IEnumerable<string>)arguments)
		{

		}

		public AttributeDeclaration(string name, IEnumerable<string> arguments)
		{
			if(name == null)
				throw new ArgumentNullException(nameof(name));
			if(arguments == null)
				throw new ArgumentNullException(nameof(arguments));
			this.name = name;
			this.arguments = arguments.ToList();
		}

		public string Name { get { return this.name; } }

		public IReadOnlyList<string> Arguments { get { return this.arguments; } }
	}
}

