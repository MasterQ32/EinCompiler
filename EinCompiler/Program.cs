using EinCompiler.BackEnds;
using EinCompiler.FrontEnds;
using EinCompiler.RawSyntaxTree;
using EinCompiler.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EinCompiler
{
	class Program
	{
		static readonly Tokenizer tokenizer;
		static readonly TypeContainer commonTypes;

		static ModuleDescription Compile(string fileName)
		{
			var file = Path.GetFullPath(fileName);
			var path = Path.GetDirectoryName(file);
			var root = new Uri(file);
			var source = File.ReadAllText(file);

			var tokens = tokenizer.Tokenize(source);

			var rawTree = Parser.Parse<CFlatParser>(tokens);

			return rawTree.Translate(
				commonTypes, 
				(f) =>
				{
					var uri = new Uri(f, UriKind.RelativeOrAbsolute);
					if (uri.IsAbsoluteUri == false)
						uri = new Uri(root, uri);
					return Compile(uri.AbsolutePath);
				});
		}

		static Program()
		{
			tokenizer = Tokenizer.Load("./Grammars/c-flat.tok");

			commonTypes = new TypeContainer();
			commonTypes.Add(TypeDescription.Void);
			commonTypes.Add(new IntegerType("ptr", true, 4));
			commonTypes.Add(new IntegerType("int", true, 4));
			commonTypes.Add(new IntegerType("uint", true, 4));
			commonTypes.Boolean = commonTypes["int"];
		}

		static void Main(string[] args)
		{
			var module = Compile("./Examples/work.e");

			// BackEnd.GenerateCode<CCodeBackEnd>(module, Console.Out);
			BackEnd.GenerateCode<SVMABackEnd>(module, Console.Out);

			using (var sw = new StreamWriter("output.asm", false, Encoding.UTF8))
			{
				BackEnd.GenerateCode<SVMABackEnd>(module, sw);
			}

			Console.ReadLine();
		}
	}
}
