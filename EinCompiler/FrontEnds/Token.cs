using System.Text.RegularExpressions;

namespace EinCompiler
{
	public class Token
	{
		public Token(TokenType type, Match match, int lineno) 
		{
			this.Type = type;
			this.Start = match.Index;
			this.Length = match.Length;
			this.Text = match.Value;
			this.LineNumber = lineno;
		}

		public int LineNumber { get; private set; }

		public string Text { get; private set; }

		public int Start { get; private set; }

		public int Length { get; private set; }

		public TokenType Type { get; private set; }

		public override string ToString() => $"[{this.Type.Name}] {this.Text}";
	}
}