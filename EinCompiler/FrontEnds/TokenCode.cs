using System.Text.RegularExpressions;
using System;

namespace EinCompiler
{
	/// <summary>
	/// A token code defines the matching information to a given token type.
	/// </summary>
	public sealed class TokenCode
	{
		private readonly TokenType type;
		private readonly Regex regex;

		public TokenCode(TokenType type, Regex regex)
		{
			if (type == null)
				throw new ArgumentNullException (nameof (type));
			if (regex == null)
				throw new ArgumentNullException (nameof (regex));
			this.type = type;
			this.regex = regex;
		}
		
		public TokenCode(TokenType name, string regex) : 
			this(name, new Regex(regex, RegexOptions.Compiled))
		{

		}

		public TokenType Type => this.type;

		public Regex Regex => this.regex;

		public bool Emitted { get; set; } = true;

		public override string ToString() => $"{this.Type} := {this.Regex}";
	}
}