using System;

namespace EinCompiler
{
	/// <summary>
	/// A distinct type of a token. Each instance is considered a different type.
	/// </summary>
	public sealed class TokenType
	{
		public TokenType (string name)
		{
			if (name == null)
				throw new ArgumentNullException (nameof (name));
			this.Name = name;
		}

		public string Name { get; private set; }

		public override string ToString () => this.Name;
	}
}

