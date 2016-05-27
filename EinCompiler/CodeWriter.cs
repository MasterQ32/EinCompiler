using System;
using System.IO;
using System.Text;

namespace EinCompiler
{
	public class CodeWriter : TextWriter
	{
		public override Encoding Encoding => Encoding.ASCII;
	}
}