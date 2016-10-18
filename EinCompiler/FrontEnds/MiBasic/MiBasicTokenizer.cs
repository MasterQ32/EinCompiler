
namespace EinCompiler.FrontEnds
{
	using System.Text.RegularExpressions;
	public sealed class MiBasicTokenizer : Tokenizer
	{
		public MiBasicTokenizer()
		{
			// WHITESPACE(noemit)          := \s+
			this.Add(MiBasicTokens.WHITESPACE, new Regex(@"\s+", RegexOptions.None));
			// COMMENT(noemit,singleline)  := #!.*?!#
			this.Add(MiBasicTokens.COMMENT, new Regex(@"#!.*?!#", RegexOptions.Singleline));
			// COMMENT(noemit,multiline)   := #.*$
			this.Add(MiBasicTokens.COMMENT, new Regex(@"#.*$", RegexOptions.Multiline));
			// NUMBER                      := (?:0x[0-9A-Fa-f]+|-?\d*\.?\d+)
			this.Add(new TokenType(MiBasicTokens.NUMBER, new Regex(@"(?:0x[0-9A-Fa-f]+|-?\d*\.?\d+)", RegexOptions.None))
			{
				Emitted = true,
			});
			// IDENTIFIER                  := [A-Za-z_]\w*
			this.Add(new TokenType(MiBasicTokens.IDENTIFIER, new Regex(@"[A-Za-z_]\w*", RegexOptions.None))
			{
				Emitted = true,
			});
			// SUBSCRIPT                   := \.
			this.Add(new TokenType(MiBasicTokens.SUBSCRIPT, new Regex(@"\.", RegexOptions.None))
			{
				Emitted = true,
			});
			// DELIMITER                   := \;
			this.Add(new TokenType(MiBasicTokens.DELIMITER, new Regex(@"\;", RegexOptions.None))
			{
				Emitted = true,
			});
			// COMMA                       := \,
			this.Add(new TokenType(MiBasicTokens.COMMA, new Regex(@"\,", RegexOptions.None))
			{
				Emitted = true,
			});
			// RETVAL                      := \?
			this.Add(new TokenType(MiBasicTokens.RETVAL, new Regex(@"\?", RegexOptions.None))
			{
				Emitted = true,
			});
			// OPERATOR                    := (?:>=|<=|<>|==|[><=+\-*\/@~])
			this.Add(new TokenType(MiBasicTokens.OPERATOR, new Regex(@"(?:>=|<=|<>|==|[><=+\-*\/@~])", RegexOptions.None))
			{
				Emitted = true,
			});
			// STRING                      := ".*?(?<!\\)"
			this.Add(new TokenType(MiBasicTokens.STRING, new Regex(@""".*?(?<!\\)""", RegexOptions.None))
			{
				Emitted = true,
			});
			// O_BRACKET                   := [\{(\[]
			this.Add(new TokenType(MiBasicTokens.O_BRACKET, new Regex(@"[\{(\[]", RegexOptions.None))
			{
				Emitted = true,
			});
			// C_BRACKET                   := [\})\]]
			this.Add(new TokenType(MiBasicTokens.C_BRACKET, new Regex(@"[\})\]]", RegexOptions.None))
			{
				Emitted = true,
			});
		}
	}

	public static class MiBasicTokens
	{
		public static readonly string WHITESPACE = "WHITESPACE";
		public static readonly string COMMENT = "COMMENT";
		public static readonly string NUMBER = "NUMBER";
		public static readonly string IDENTIFIER = "IDENTIFIER";
		public static readonly string SUBSCRIPT = "SUBSCRIPT";
		public static readonly string DELIMITER = "DELIMITER";
		public static readonly string COMMA = "COMMA";
		public static readonly string RETVAL = "RETVAL";
		public static readonly string OPERATOR = "OPERATOR";
		public static readonly string STRING = "STRING";
		public static readonly string O_BRACKET = "O_BRACKET";
		public static readonly string C_BRACKET = "C_BRACKET";
	}
}
