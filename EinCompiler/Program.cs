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
			Tokenizer tok = Tokenizer.Load("./Grammars/c-minor.tok");
			

			var source = File.ReadAllText("./Examples/c-minor.c");

			var tokens = tok.Tokenize(source);

			foreach (var token in tokens)
			{
				Console.WriteLine("[{0}] {1}", token.Type.Name, token.Text);
			}

			Console.ReadLine();
		}
	}
}
