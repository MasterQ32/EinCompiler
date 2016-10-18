﻿
namespace EinCompiler.FrontEnds
{
	using System.Text.RegularExpressions;
	public sealed class PsiTokenizer : Tokenizer
	{
		public PsiTokenizer()
		{
			this.Add(PsiTokens.COMMENT, new Regex(@"\/\*.*?\*\/", RegexOptions.Singleline));
			this.Add(PsiTokens.COMMENT, new Regex(@"\/\/[^\n]*", RegexOptions.None));
			this.Add(PsiTokens.RAW_BLOCK, new Regex(@"\[\[.*?\]\]", RegexOptions.Singleline));
			this.Add(PsiTokens.STRING, new Regex(@""".*?(?<!\\)""", RegexOptions.None));
			this.Add(PsiTokens.WHITESPACE, new Regex(@"\s+", RegexOptions.None));
			this.Add(PsiTokens.O_BRACKET, new Regex(@"\(", RegexOptions.None));
			this.Add(PsiTokens.C_BRACKET, new Regex(@"\)", RegexOptions.None));
			this.Add(PsiTokens.O_CBRACKET, new Regex(@"\{", RegexOptions.None));
			this.Add(PsiTokens.C_CBRACKET, new Regex(@"\}", RegexOptions.None));
			this.Add(PsiTokens.O_SBRACKET, new Regex(@"\[", RegexOptions.None));
			this.Add(PsiTokens.C_SBRACKET, new Regex(@"\]", RegexOptions.None));
			this.Add(PsiTokens.SEPARATOR, new Regex(@"\,", RegexOptions.None));
			this.Add(PsiTokens.DELIMITER, new Regex(@"\;", RegexOptions.None));
			this.Add(PsiTokens.ASSIGNMENT, new Regex(@"\:\=", RegexOptions.None));
			this.Add(PsiTokens.COLON, new Regex(@"\:", RegexOptions.None));
			this.Add(PsiTokens.ARROW, new Regex(@"\-\>", RegexOptions.None));
			this.Add(PsiTokens.NUMBER, new Regex(@"(?<sign>-?)(?:(?<hex>0x[0-9A-Fa-f]+)|(?<bin>0b[01]+)|(?<float>\d+\.\d*)|(?<int>\d+))", RegexOptions.None));
			this.Add(PsiTokens.CHARACTER, new Regex(@"'\\?.'", RegexOptions.None));
			this.Add(PsiTokens.BINARY_OPERATOR, new Regex(@"!=|>=|<=|>|<|=|\+|\-|\*|\/|\%", RegexOptions.None));
			this.Add(PsiTokens.KEYWORD, new Regex(@"\b(include|var|fn|const|static|private|global|shared|export|naked|inline|return|if|else|while|break)\b", RegexOptions.None));
			this.Add(PsiTokens.IDENTIFIER, new Regex(@"\b[A-Za-z_]\w*\b", RegexOptions.None));
		}
	}

	public static class PsiTokens
	{
		public static readonly string COMMENT = "COMMENT";
		public static readonly string RAW_BLOCK = "RAW_BLOCK";
		public static readonly string STRING = "STRING";
		public static readonly string WHITESPACE = "WHITESPACE";
		public static readonly string O_BRACKET = "O_BRACKET";
		public static readonly string C_BRACKET = "C_BRACKET";
		public static readonly string O_CBRACKET = "O_CBRACKET";
		public static readonly string C_CBRACKET = "C_CBRACKET";
		public static readonly string O_SBRACKET = "O_SBRACKET";
		public static readonly string C_SBRACKET = "C_SBRACKET";
		public static readonly string SEPARATOR = "SEPARATOR";
		public static readonly string DELIMITER = "DELIMITER";
		public static readonly string ASSIGNMENT = "ASSIGNMENT";
		public static readonly string COLON = "COLON";
		public static readonly string ARROW = "ARROW";
		public static readonly string NUMBER = "NUMBER";
		public static readonly string CHARACTER = "CHARACTER";
		public static readonly string BINARY_OPERATOR = "BINARY_OPERATOR";
		public static readonly string KEYWORD = "KEYWORD";
		public static readonly string IDENTIFIER = "IDENTIFIER";
	}
}
