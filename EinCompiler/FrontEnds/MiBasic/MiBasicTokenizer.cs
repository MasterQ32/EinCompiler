
namespace EinCompiler.FrontEnds
{
	using System.Text.RegularExpressions;
	public sealed class MiBasicTokenizer : Tokenizer
	{
		public MiBasicTokenizer()
		{
			// WHITESPACE(noemit)          := \s+
			this.Add(new TokenCode(MiBasicTokens.WHITESPACE, new Regex(@"\s+", RegexOptions.None))
			{
				Emitted = false,
			});
			// COMMENT(noemit,singleline)  := #!.*?!#
			this.Add(new TokenCode(MiBasicTokens.COMMENT, new Regex(@"#!.*?!#", RegexOptions.Singleline))
			{
				Emitted = false,
			});
			// COMMENT(noemit,multiline)   := #.*$
			this.Add(new TokenCode(MiBasicTokens.COMMENT, new Regex(@"#.*$", RegexOptions.Multiline))
			{
				Emitted = false,
			});
			// NUMBER                      := (?:0x[0-9A-Fa-f]+|-?\d*\.?\d+)
			this.Add(new TokenCode(MiBasicTokens.NUMBER, new Regex(@"(?:0x[0-9A-Fa-f]+|-?\d*\.?\d+)", RegexOptions.None))
			{
				Emitted = true,
			});
			// IDENTIFIER                  := [A-Za-z_]\w*
			this.Add(new TokenCode(MiBasicTokens.IDENTIFIER, new Regex(@"[A-Za-z_]\w*", RegexOptions.None))
			{
				Emitted = true,
			});
			// SUBSCRIPT                   := \.
			this.Add(new TokenCode(MiBasicTokens.SUBSCRIPT, new Regex(@"\.", RegexOptions.None))
			{
				Emitted = true,
			});
			// DELIMITER                   := \;
			this.Add(new TokenCode(MiBasicTokens.DELIMITER, new Regex(@"\;", RegexOptions.None))
			{
				Emitted = true,
			});
			// COMMA                       := \,
			this.Add(new TokenCode(MiBasicTokens.COMMA, new Regex(@"\,", RegexOptions.None))
			{
				Emitted = true,
			});
			// RETVAL                      := \?
			this.Add(new TokenCode(MiBasicTokens.RETVAL, new Regex(@"\?", RegexOptions.None))
			{
				Emitted = true,
			});
			// OPERATOR                    := (?:>=|<=|<>|==|[><=+\-*\/@~])
			this.Add(new TokenCode(MiBasicTokens.OPERATOR, new Regex(@"(?:>=|<=|<>|==|[><=+\-*\/@~])", RegexOptions.None))
			{
				Emitted = true,
			});
			// STRING                      := ".*?(?<!\\)"
			this.Add(new TokenCode(MiBasicTokens.STRING, new Regex(@""".*?(?<!\\)""", RegexOptions.None))
			{
				Emitted = true,
			});
			// O_BRACKET                   := [\{(\[]
			this.Add(new TokenCode(MiBasicTokens.O_BRACKET, new Regex(@"[\{(\[]", RegexOptions.None))
			{
				Emitted = true,
			});
			// C_BRACKET                   := [\})\]]
			this.Add(new TokenCode(MiBasicTokens.C_BRACKET, new Regex(@"[\})\]]", RegexOptions.None))
			{
				Emitted = true,
			});
		}
	}

	public static class MiBasicTokens
	{
		public static readonly TokenType WHITESPACE = new TokenType("WHITESPACE");
		public static readonly TokenType COMMENT = new TokenType("COMMENT");
		public static readonly TokenType NUMBER = new TokenType("NUMBER");
		public static readonly TokenType IDENTIFIER = new TokenType("IDENTIFIER");
		public static readonly TokenType SUBSCRIPT = new TokenType("SUBSCRIPT");
		public static readonly TokenType DELIMITER = new TokenType("DELIMITER");
		public static readonly TokenType COMMA = new TokenType("COMMA");
		public static readonly TokenType RETVAL = new TokenType("RETVAL");
		public static readonly TokenType OPERATOR = new TokenType("OPERATOR");
		public static readonly TokenType STRING = new TokenType("STRING");
		public static readonly TokenType O_BRACKET = new TokenType("O_BRACKET");
		public static readonly TokenType C_BRACKET = new TokenType("C_BRACKET");
	}
}
