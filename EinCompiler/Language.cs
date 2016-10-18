using System;
using EinCompiler.FrontEnds;
using System.Collections.Generic;

namespace EinCompiler
{
	public class Language
	{
		public string ID { get; set; }
		public string Name { get; set; }

		public IReadOnlyList<string> Extensions { get; set; }

		public TypeContainer Types { get; set; }

		public Func<Tokenizer> Tokenizer { get; set; }

		public Func<Parser> Parser { get; set; }
	}
}

