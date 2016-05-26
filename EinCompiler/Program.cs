using EinCompiler.FrontEnds;
using EinCompiler.RawSyntaxTree;
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
			
			var rawTree = Parser.Parse<CFlatParser>(tokens);

			var types = new TypeContainer();
			types.Add(TypeDescription.Void);
			types.Add(new TypeDescription("int"));

			var tree = rawTree.Translate(types);

			Console.ReadLine();
		}
	}
}
