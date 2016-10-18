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
		static readonly TypeContainer commonTypes;

		static List<Language> languages = new List<Language> ();
		static Dictionary<string, BackEnd> codegens = new Dictionary<string, BackEnd>();

		static ModuleDescription Compile (Language lang, string fileName)
		{
			var file = Path.GetFullPath (fileName);
			// var path = Path.GetDirectoryName(file);
			var root = new Uri (file);
			var source = File.ReadAllText (file);

			var tokens = lang.Tokenizer().Tokenize (source);

			try {
				var rawTree = Parser.Parse (lang.Parser(), tokens);

				var instancer = new ModuleInstancer (
	                commonTypes,
	                (f) => {
						var uri = new Uri (f, UriKind.RelativeOrAbsolute);
						if (uri.IsAbsoluteUri == false)
							uri = new Uri (root, uri);
						return Compile (lang, uri.AbsolutePath);
					});

				var result = instancer.CreateInstance (rawTree);
				return result;
			}
			catch (SemanticException ex) {
				Console.Error.WriteLine ("{0}:{1}: {2}",
					Path.GetFileName (fileName),
					ex.Token.Start,
					ex.Message);
				throw;
			}
		}

		static Program ()
		{
			commonTypes = new TypeContainer ();
			commonTypes.Add (Types.Void);
			commonTypes.Add (new IntegerType ("ptr", false, 4));
			commonTypes.Add ("int", Types.Int32);
			commonTypes.Add ("uint", Types.UInt32);
			commonTypes.Add ("i8", Types.Int8);
			commonTypes.Add ("i16", Types.Int16);
			commonTypes.Add ("i32", Types.Int32);
			commonTypes.Add ("u8", Types.UInt8);
			commonTypes.Add ("u16", Types.UInt16);
			commonTypes.Add ("u32", Types.UInt32);

			commonTypes.Boolean = commonTypes ["int"];

			languages.Add (new Language {
				ID = "psi",
				Name = "Psi",
				Extensions = new [] { ".psi" },
				Types = commonTypes,
				Tokenizer = () => Tokenizer.Load (PsiTokens.Tokens),
				Parser = () => new PsiParser (),
			});

			codegens.Add ("c", new CCodeBackEnd ());
			codegens.Add ("svma", new SVMABackEnd ());
		}

		static int Main (string[] args)
		{
			if (args.Length == 0)
				args = new []{ "-f", "c", "Examples/work.psi" };

			bool errors = false;
			string output = null;
			BackEnd codegen = null;
			var files = new List<Tuple<string, Language>> ();
			for (int i = 0; i < args.Length; i++) {

				if (args [i].StartsWith ("-")) {
					switch (args [i]) {
					case "-o":
					case "--output":
						output = args [++i];
						break;
					case "-f":
					case "-format":
						if (codegens.TryGetValue (args [++i], out codegen) == false) {
							Console.Error.WriteLine ("Unrecognized format: {0}", args [i]);
							errors = true;
						}
						break;
					default:
						Console.Error.WriteLine ("Unrecognized switch: {0}", args [i]);
						errors = true;
						break;
					}
				} else {
					var ext = Path.GetExtension (args [i]);
					var lang = languages.FirstOrDefault (l => l.Extensions.Contains (ext));
					if (lang == null) {
						Console.Error.WriteLine ("Unrecognized input format: {0}", ext);
						errors = true;
					} else {
						files.Add (Tuple.Create (args [i], lang));
					}
				}
			}

			if (errors) {
				return 1;
			}
			if (codegen == null) {
				Console.Error.WriteLine ("No code generator defined.");
				return 1;
			}

			var result = new ModuleDescription (); 
			try {
				foreach (var input in files) {
					var module = Compile (input.Item2, input.Item1);
					result.Merge (module);
				}
			} catch (SemanticException) {
				return 1;
			}

			TextWriter codedest = null;
			try {

				if(output == null) {
					codedest = Console.Out;
				} else {
					codedest = new CodeWriter();
				}

				BackEnd.GenerateCode(codegen, result, codedest);
			} finally {
				codedest?.Close ();
			}

			return 0;
		}
	}
}
