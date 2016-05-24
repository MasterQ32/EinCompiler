using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EinCompiler
{
	class Program
	{
		static void Main(string[] args)
		{
			Tokenizer tok = Tokenizer.Load("./Grammars/tokens.tok");

			var source = File.ReadAllText("./Examples/tokenizer-test.txt");

			var tokens = tok.Tokenize(source);

			foreach (var token in tokens)
			{
				Console.WriteLine("[{0}] {1}", token.Type.Name, token.Text);
			}
		}
	}
}
