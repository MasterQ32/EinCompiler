﻿using EinCompiler.BackEnds;
using EinCompiler.FrontEnds;
using EinCompiler.RawSyntaxTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace EinCompiler
{
	class Program
	{
		static readonly TypeContainer commonTypes;

		static List<Language> languages = new List<Language>();
		static Dictionary<string, BackEnd> codegens = new Dictionary<string, BackEnd>();

		static ModuleDescription Compile(ModuleDescription module, Language lang, string fileName)
		{
			var file = Path.GetFullPath(fileName);
			// var path = Path.GetDirectoryName(file);
			var root = new Uri(file);
			var source = File.ReadAllText(file);

			try
			{
				var tokens = lang.Tokenizer().Tokenize(source);

				var rawTree = Parser.Parse(lang.Parser(), tokens);

				var instancer = new ModuleInstancer(
					                commonTypes,
					                (f) =>
					{
						var uri = new Uri(f, UriKind.RelativeOrAbsolute);
						if (uri.IsAbsoluteUri == false)
							uri = new Uri(root, uri);
						return Compile(module, lang, uri.AbsolutePath);
					});

				return instancer.CreateInto(module, rawTree);
			}
			catch (CompilationException ex)
			{
				Console.Error.WriteLine("{0}:{1}: {2}",
					Path.GetFileName(fileName),
					ex.Token.LineNumber,
					ex.Message);
				throw;
			}
		}

		static Program()
		{
			commonTypes = new TypeContainer();
			commonTypes.Add(Types.Void);
			commonTypes.Add("int", Types.Int32);
			commonTypes.Add("uint", Types.UInt32);
			commonTypes.Add("i8", Types.Int8);
			commonTypes.Add("i16", Types.Int16);
			commonTypes.Add("i32", Types.Int32);
			commonTypes.Add("u8", Types.UInt8);
			commonTypes.Add("u16", Types.UInt16);
			commonTypes.Add("u32", Types.UInt32);
			commonTypes.Add("ptr", Types.Pointer);
			commonTypes.Add("string", Types.String);

			commonTypes.Boolean = commonTypes["int"];

			languages.Add(new Language
				{
					ID = "psi",
					Name = "Psi",
					Extensions = new [] { ".psi" },
					Types = commonTypes,
					Tokenizer = () => new PsiTokenizer(),
					Parser = () => new PsiParser(),
				});

			
			{
				var stdlib = Compile(null, languages[0], Program.Root + "StandardLib/c.psi");
				codegens.Add("c", new CCodeBackEnd()
					{
						BuiltIns = { stdlib }
					});
			}
			{
				var stdlib = Compile(null, languages[0], Program.Root + "StandardLib/svma.psi");
				codegens.Add("svma", new SVMABackEnd()
					{
						BuiltIns = { stdlib }
					});
			}
		}

		static int Main(string[] args)
		{
			if (args.Length == 0 && Debugger.IsAttached)
				args = new []{ "-f", "svma", "Examples/work.psi" };

			bool errors = false;
			string output = null;
			BackEnd codegen = null;
			var files = new List<Tuple<string, Language>>();
			for (int i = 0; i < args.Length; i++)
			{

				if (args[i].StartsWith("-"))
				{
					switch (args[i])
					{
						case "-o":
						case "--output":
							output = args[++i];
							break;
						case "-f":
						case "--format":
							if (codegens.TryGetValue(args[++i], out codegen) == false)
							{
								Console.Error.WriteLine("Unrecognized format: {0}", args[i]);
								errors = true;
							}
							break;
						default:
							Console.Error.WriteLine("Unrecognized switch: {0}", args[i]);
							errors = true;
							break;
					}
				}
				else
				{
					var ext = Path.GetExtension(args[i]);
					var lang = languages.FirstOrDefault(l => l.Extensions.Contains(ext));
					if (lang == null)
					{
						Console.Error.WriteLine("Unrecognized input format: {0}", ext);
						errors = true;
					}
					else
					{
						files.Add(Tuple.Create(args[i], lang));
					}
				}
			}

			if (errors)
			{
				return 1;
			}
			if (codegen == null)
			{
				Console.Error.WriteLine("No code generator defined.");
				return 1;
			}

			var result = new ModuleDescription
			{
				Types = commonTypes,
			};

			foreach (var builtin in codegen.BuiltIns)
			{
				result.Merge(builtin);
			}

			try
			{
				foreach (var input in files)
				{
					Compile(result, input.Item2, input.Item1);
				}
			}
			catch (SemanticException)
			{
				return 1;
			}

			TextWriter codedest = null;
			try
			{
				if (output == null)
				{
					codedest = Console.Out;
				}
				else
				{
					codedest = new StreamWriter(output, false, Encoding.ASCII);
				}

				BackEnd.GenerateCode(codegen, result, codedest);
			}
			finally
			{
				codedest?.Close();
			}

			return 0;
		}

		public static string Root => Path.GetDirectoryName(new Uri(typeof(Language).Assembly.CodeBase).AbsolutePath) + "/";
	}
}
