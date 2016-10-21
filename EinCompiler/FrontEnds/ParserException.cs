using System;
using System.Runtime.Serialization;

namespace EinCompiler.FrontEnds
{
	[Serializable]
	public sealed class ParserException : CompilationException
	{
		public ParserException(Token token) : base(token, $"{token.Text} ({token.Type.Name}) was not expected.")
		{
			
		}

		public ParserException(Token token, string message) : base(token, message)
		{
			
		}

		public ParserException(Token token, string message, Exception innerException) : base(token, message, innerException)
		{
			
		}

		private ParserException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			
		}
	}
}