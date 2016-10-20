using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EinCompiler
{
	public abstract class Tokenizer : IReadOnlyList<TokenCode>
	{
		// private static readonly Regex tokenizerFormat = new Regex(@"^(?<key>\w+)(?:\((?<options>\w+(?:,\w+)*)\))?\s*:=\s*(?<value>.*?)\s*$", RegexOptions.Compiled);
		private readonly List<TokenCode> tokenCodes = new List<TokenCode> ();

		protected Tokenizer()
		{
			this.tokenCodes = new List<TokenCode>();
		}

		protected void Add(TokenCode token)
		{
			this.tokenCodes.Add(token);
		}

		protected void Add(TokenType type, Regex regex)
		{
			this.Add(new TokenCode(type, regex));
		}

		protected void Add(TokenType type, string regex)
		{
			this.Add(new TokenCode(type, regex));
		}

		public Token[] Tokenize(string text)
		{
			var tokens = new List<Token>();
			int cursor = 0;
			int lineno = 1;
			while (cursor < text.Length)
			{
				var hadMatch = false;
				for (int i = 0; i < this.tokenCodes.Count; i++)
				{
					var match = this.tokenCodes[i].Regex.Match(text, cursor);
					if (match.Success == false)
						continue;
					if (match.Index != cursor)
						continue;

					if(this.tokenCodes[i].Emitted)
						tokens.Add(new Token(this.tokenCodes[i].Type, match, lineno));

					lineno += match.Value.Count (c => c == '\n');

					cursor += match.Length;
					hadMatch = true;
					break;
				}
				if (!hadMatch)
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

		/*
		public static Tokenizer Load(string fileName)
		{
			return Load(File.ReadAllLines(fileName));
		}
		*/

		private sealed class RuntimeTokenizer : Tokenizer
		{
			public RuntimeTokenizer() { }
		}

		/*
		public static Tokenizer Load(params string[] definitions)
		{
			var tokenizer = new RuntimeTokenizer();
			var types = new Dictionary<string, TokenType> ();
			for (int i = 0; i < definitions.Length; i++)
			{
				var match = tokenizerFormat.Match(definitions[i]);
				if (match.Success == false)
					continue;
				var name = match.Groups["key"].Value;
				var options = match.Groups["options"].Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				var regextext = match.Groups["value"].Value;
				var emitted = true;
				
				RegexOptions regexoptions = RegexOptions.Compiled;
				foreach(var option in options)
				{
					if (option == "noemit")
					{
						emitted = false;
						continue;
					}
					var value = (RegexOptions)Enum.Parse(typeof(RegexOptions), option, true);
					regexoptions |= value;
				}

				var regex = new Regex(regextext, regexoptions);

				var token = new TokenCode(name, regex)
				{
					Emitted = emitted,
				};

				tokenizer.Add(token);
			}
			return tokenizer;
		}
		*/

		public int IndexOf(TokenCode item)
		{
			return tokenCodes.IndexOf(item);
		}

		public bool Contains(TokenCode item)
		{
			return tokenCodes.Contains(item);
		}

		public void CopyTo(TokenCode[] array, int arrayIndex)
		{
			tokenCodes.CopyTo(array, arrayIndex);
		}

		public bool Remove(TokenCode item)
		{
			return tokenCodes.Remove(item);
		}

		public IEnumerator<TokenCode> GetEnumerator()
		{
			return ((IList<TokenCode>)tokenCodes).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<TokenCode>)tokenCodes).GetEnumerator();
		}

		public int Count => tokenCodes.Count;

		public TokenCode this[int index]
		{
			get { return tokenCodes[index]; }
		}
	}
}