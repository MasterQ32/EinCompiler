using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EinCompiler
{
	public sealed class Tokenizer
	{
		private static readonly Regex tokenizerFormat = new Regex(@"^(?<key>\w+)\s*:=\s*(?<value>.*?)\s*$", RegexOptions.Compiled);
		private readonly List<TokenType> tokenTypes;

		public Tokenizer()
		{
			this.tokenTypes = new List<TokenType>();
		}

		public void Add(TokenType token)
		{
			this.tokenTypes.Add(token);
		}

		public void Add(string name, Regex regex)
		{
			this.Add(new TokenType(name, regex));
		}

		public void Add(string name, string regex)
		{
			this.Add(new TokenType(name, regex));
		}

		public Token[] Tokenize(string text)
		{
			var tokens = new List<Token>();
			int cursor = 0;
			while (cursor < text.Length)
			{
				var hadMatch = false;
				for (int i = 0; i < this.tokenTypes.Count; i++)
				{
					var match = this.tokenTypes[i].Regex.Match(text, cursor);
					if (match.Index != cursor)
						continue;

					tokens.Add(new Token(this.tokenTypes[i], match));

					cursor += match.Length;
					hadMatch = true;
					break;
				}
				if(!hadMatch) 
					throw new InvalidOperationException("!!");
			}
			return tokens.ToArray();
		}

		public Token[] Tokenize(TextReader reader)
		{
			return Tokenize(reader.ReadToEnd());
		}

		public Token[] Tokenize(Stream stream, Encoding encoding)
		{
			using (var reader = new StreamReader(stream, encoding))
			{
				return Tokenize(reader);
			}
		}

		public static Tokenizer Load(string fileName)
		{
			return Load(File.ReadAllLines(fileName));
		}

		public static Tokenizer Load(params string[] definitions)
		{
			var tokenizer = new Tokenizer();
			for (int i = 0; i < definitions.Length; i++)
			{
				var match = tokenizerFormat.Match(definitions[i]);
				if (match.Success == false)
					continue;
				var name = match.Groups["key"].Value;
				var regex = match.Groups["value"].Value;
				var token = new TokenType(name, regex);

				tokenizer.Add(token);
			}
			return tokenizer;
		}
	}
}