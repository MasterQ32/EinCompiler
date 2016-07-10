using System;

namespace EinCompiler
{
	public static class PsiTokens
	{
		public static readonly string[] Tokens = new [] {
			@"COMMENT(noemit,singleline) := \/\*.*?\*\/",
			@"COMMENT(noemit)       := \/\/[^\n]*",
			@"RAW_BLOCK(singleline) := \[\[.*?\]\]",
			@"STRING				  := "".*?(?<!\\)""",
			@"WHITESPACE(noemit)    := \s+",
			@"O_BRACKET             := \(",
			@"C_BRACKET             := \)",
			@"O_CBRACKET            := \{",
			@"C_CBRACKET            := \}",
			@"O_SBRACKET            := \[",
			@"C_SBRACKET            := \]",
			@"SEPARATOR             := \,",
			@"DELIMITER             := \;",
			@"ASSIGNMENT            := \:\=",
			@"COLON                 := \:",
			@"ARROW                 := \-\>",
			@"NUMBER                := (?<sign>-?)(?:(?<hex>0x[0-9A-Fa-f]+)|(?<bin>0b[01]+)|(?<float>\d+\.\d*)|(?<int>\d+))",
			@"CHARACTER             := '\\?.'",
			@"BINARY_OPERATOR       := !=|>=|<=|>|<|=|\+|\-|\*|\/|\%",
			@"KEYWORD               := \b(include|var|fn|const|static|private|global|shared|export|naked|inline|return|if|else|while|break)\b",
			@"IDENTIFIER            := \b[A-Za-z_]\w*\b",
		};
	}
}

