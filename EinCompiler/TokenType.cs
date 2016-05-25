using System.Text.RegularExpressions;

namespace EinCompiler
{
	public sealed class TokenType
	{
		private readonly string name;
		private readonly Regex regex;

		public TokenType(string name, Regex regex)
		{
			this.name = name.ToUpper();
			this.regex = regex;
		}
		
		public TokenType(string name, string regex) : 
			this(name, new Regex(regex, RegexOptions.Compiled))
		{

		}

		public string Name => this.name;

		public Regex Regex => this.regex;

		public bool Emitted { get; set; } = true;

		public override string ToString() => $"{this.Name} := {this.Regex}";
	}
}