using EinCompiler.FrontEnds;
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
			Tokenizer tok = Tokenizer.Load("./Grammars/c-flat.tok");
			

			var source = File.ReadAllText("./Examples/c-flat.c");

			var tokens = tok.Tokenize(source);

			foreach (var token in tokens)
			{
				Console.WriteLine("[{0}] {1}", token.Type.Name, token.Text);
			}

			var rawTree = Parser.Parse<CFlatParser>(tokens);

			Console.ReadLine();
		}
	}
}
