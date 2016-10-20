
namespace EinCompiler.FrontEnds
{
	using System.Text.RegularExpressions;
	public sealed class PsiTokenizer : Tokenizer
	{
		public PsiTokenizer()
		{
			// COMMENT(noemit,singleline) := \/\*.*?\*\/
			this.Add(new TokenCode(PsiTokens.COMMENT, new Regex(@"\/\*.*?\*\/", RegexOptions.Singleline))
			{
				Emitted = false,
			});
			// COMMENT(noemit)       := \/\/[^\n]*
			this.Add(new TokenCode(PsiTokens.COMMENT, new Regex(@"\/\/[^\n]*", RegexOptions.None))
			{
				Emitted = false,
			});
			// RAW_BLOCK(singleline) := \[\[.*?\]\]
			this.Add(new TokenCode(PsiTokens.RAW_BLOCK, new Regex(@"\[\[.*?\]\]", RegexOptions.Singleline))
			{
				Emitted = true,
			});
			// STRING				  := ".*?(?<!\\)"
			this.Add(new TokenCode(PsiTokens.STRING, new Regex(@""".*?(?<!\\)""", RegexOptions.None))
			{
				Emitted = true,
			});
			// WHITESPACE(noemit)    := \s+
			this.Add(new TokenCode(PsiTokens.WHITESPACE, new Regex(@"\s+", RegexOptions.None))
			{
				Emitted = false,
			});
			// O_BRACKET             := \(
			this.Add(new TokenCode(PsiTokens.O_BRACKET, new Regex(@"\(", RegexOptions.None))
			{
				Emitted = true,
			});
			// C_BRACKET             := \)
			this.Add(new TokenCode(PsiTokens.C_BRACKET, new Regex(@"\)", RegexOptions.None))
			{
				Emitted = true,
			});
			// O_CBRACKET            := \{
			this.Add(new TokenCode(PsiTokens.O_CBRACKET, new Regex(@"\{", RegexOptions.None))
			{
				Emitted = true,
			});
			// C_CBRACKET            := \}
			this.Add(new TokenCode(PsiTokens.C_CBRACKET, new Regex(@"\}", RegexOptions.None))
			{
				Emitted = true,
			});
			// O_SBRACKET            := \[
			this.Add(new TokenCode(PsiTokens.O_SBRACKET, new Regex(@"\[", RegexOptions.None))
			{
				Emitted = true,
			});
			// C_SBRACKET            := \]
			this.Add(new TokenCode(PsiTokens.C_SBRACKET, new Regex(@"\]", RegexOptions.None))
			{
				Emitted = true,
			});
			// SEPARATOR             := \,
			this.Add(new TokenCode(PsiTokens.SEPARATOR, new Regex(@"\,", RegexOptions.None))
			{
				Emitted = true,
			});
			// DELIMITER             := \;
			this.Add(new TokenCode(PsiTokens.DELIMITER, new Regex(@"\;", RegexOptions.None))
			{
				Emitted = true,
			});
			// ASSIGNMENT            := \:\=
			this.Add(new TokenCode(PsiTokens.ASSIGNMENT, new Regex(@"\:\=", RegexOptions.None))
			{
				Emitted = true,
			});
			// COLON                 := \:
			this.Add(new TokenCode(PsiTokens.COLON, new Regex(@"\:", RegexOptions.None))
			{
				Emitted = true,
			});
			// ARROW                 := \-\>
			this.Add(new TokenCode(PsiTokens.ARROW, new Regex(@"\-\>", RegexOptions.None))
			{
				Emitted = true,
			});
			// NUMBER                := (?<sign>-?)(?:(?<hex>0x[0-9A-Fa-f]+)|(?<bin>0b[01]+)|(?<float>\d+\.\d*)|(?<int>\d+))
			this.Add(new TokenCode(PsiTokens.NUMBER, new Regex(@"(?<sign>-?)(?:(?<hex>0x[0-9A-Fa-f]+)|(?<bin>0b[01]+)|(?<float>\d+\.\d*)|(?<int>\d+))", RegexOptions.None))
			{
				Emitted = true,
			});
			// CHARACTER             := '\\?.'
			this.Add(new TokenCode(PsiTokens.CHARACTER, new Regex(@"'\\?.'", RegexOptions.None))
			{
				Emitted = true,
			});
			// OPERATOR              := !=|>=|<=|>|<|=|\+|\-|\*|\/|\%
			this.Add(new TokenCode(PsiTokens.OPERATOR, new Regex(@"!=|>=|<=|>|<|=|\+|\-|\*|\/|\%", RegexOptions.None))
			{
				Emitted = true,
			});
			// KEYWORD               := \b(extern|include|var|fn|const|static|private|global|shared|export|naked|inline|return|if|else|while|break)\b
			this.Add(new TokenCode(PsiTokens.KEYWORD, new Regex(@"\b(extern|include|var|fn|const|static|private|global|shared|export|naked|inline|return|if|else|while|break)\b", RegexOptions.None))
			{
				Emitted = true,
			});
			// IDENTIFIER            := \b[A-Za-z_]\w*\b
			this.Add(new TokenCode(PsiTokens.IDENTIFIER, new Regex(@"\b[A-Za-z_]\w*\b", RegexOptions.None))
			{
				Emitted = true,
			});
		}
	}

	public static class PsiTokens
	{
		public static readonly TokenType COMMENT = new TokenType("COMMENT");
		public static readonly TokenType RAW_BLOCK = new TokenType("RAW_BLOCK");
		public static readonly TokenType STRING = new TokenType("STRING");
		public static readonly TokenType WHITESPACE = new TokenType("WHITESPACE");
		public static readonly TokenType O_BRACKET = new TokenType("O_BRACKET");
		public static readonly TokenType C_BRACKET = new TokenType("C_BRACKET");
		public static readonly TokenType O_CBRACKET = new TokenType("O_CBRACKET");
		public static readonly TokenType C_CBRACKET = new TokenType("C_CBRACKET");
		public static readonly TokenType O_SBRACKET = new TokenType("O_SBRACKET");
		public static readonly TokenType C_SBRACKET = new TokenType("C_SBRACKET");
		public static readonly TokenType SEPARATOR = new TokenType("SEPARATOR");
		public static readonly TokenType DELIMITER = new TokenType("DELIMITER");
		public static readonly TokenType ASSIGNMENT = new TokenType("ASSIGNMENT");
		public static readonly TokenType COLON = new TokenType("COLON");
		public static readonly TokenType ARROW = new TokenType("ARROW");
		public static readonly TokenType NUMBER = new TokenType("NUMBER");
		public static readonly TokenType CHARACTER = new TokenType("CHARACTER");
		public static readonly TokenType OPERATOR = new TokenType("OPERATOR");
		public static readonly TokenType KEYWORD = new TokenType("KEYWORD");
		public static readonly TokenType IDENTIFIER = new TokenType("IDENTIFIER");
	}
}
