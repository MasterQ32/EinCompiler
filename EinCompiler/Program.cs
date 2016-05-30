using EinCompiler.BackEnds;
using EinCompiler.FrontEnds;
using EinCompiler.RawSyntaxTree;
using EinCompiler.BuiltInTypes;
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

			var rawTree = Parser.Parse<PsiParser>(tokens);

			var instancer = new ModuleInstancer(
				commonTypes,
				(f) =>
				{
					var uri = new Uri(f, UriKind.RelativeOrAbsolute);
					if (uri.IsAbsoluteUri == false)
						uri = new Uri(root, uri);
					return Compile(uri.AbsolutePath);
				});

			return instancer.CreateInstance(rawTree);
		}

		static Program()
		{
			tokenizer = Tokenizer.Load("./Grammars/psi.tok");

			commonTypes = new TypeContainer();
			commonTypes.Add(Types.Void);
			commonTypes.Add(new IntegerType("ptr", true, 4));
			commonTypes.Add("int", Types.Int32);
			commonTypes.Add("uint", Types.UInt32);
			commonTypes.Add("i8", Types.Int8);
			commonTypes.Add("i16", Types.Int16);
			commonTypes.Add("i32", Types.Int32);
			commonTypes.Add("u8", Types.UInt8);
			commonTypes.Add("u16", Types.UInt16);
			commonTypes.Add("u32", Types.UInt32);

			commonTypes.Boolean = commonTypes["int"];
		}

		static void Main(string[] args)
		{
			var module = Compile("./Examples/work.psi");

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
