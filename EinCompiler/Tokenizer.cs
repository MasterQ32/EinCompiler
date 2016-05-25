using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EinCompiler
{
	public sealed class Tokenizer : IList<TokenType>
	{
		private static readonly Regex tokenizerFormat = new Regex(@"^(?<key>\w+)(?:\((?<options>\w+(?:,\w+)*)\))?\s*:=\s*(?<value>.*?)\s*$", RegexOptions.Compiled);
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
					if (match.Success == false)
						continue;
					if (match.Index != cursor)
						continue;

					if(this.tokenTypes[i].Emitted)
						tokens.Add(new Token(this.tokenTypes[i], match));

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

				var token = new TokenType(name, regex)
				{
					Emitted = emitted,
				};

				tokenizer.Add(token);
			}
			return tokenizer;
		}

		public int IndexOf(TokenType item)
		{
			return tokenTypes.IndexOf(item);
		}

		public void Insert(int index, TokenType item)
		{
			tokenTypes.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			tokenTypes.RemoveAt(index);
		}

		public void Clear()
		{
			tokenTypes.Clear();
		}

		public bool Contains(TokenType item)
		{
			return tokenTypes.Contains(item);
		}

		public void CopyTo(TokenType[] array, int arrayIndex)
		{
			tokenTypes.CopyTo(array, arrayIndex);
		}

		public bool Remove(TokenType item)
		{
			return tokenTypes.Remove(item);
		}

		public IEnumerator<TokenType> GetEnumerator()
		{
			return ((IList<TokenType>)tokenTypes).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<TokenType>)tokenTypes).GetEnumerator();
		}

		public int Count => tokenTypes.Count;

		public bool IsReadOnly => ((IList<TokenType>)tokenTypes).IsReadOnly;

		public TokenType this[int index]
		{
			get { return tokenTypes[index]; }
			set { tokenTypes[index] = value; }
		}
	}
}