using System;
using System.Runtime.Serialization;

namespace EinCompiler
{
	[Serializable]
	internal class SemanticException : CompilationException
	{
		public SemanticException(Token token) : base(token, $"{token.Text} is invalid here.")
		{
			
		}

		public SemanticException(Token token, string message) : base(token, message)
		{
			
		}

		public SemanticException(Token token, string message, Exception innerException) : base(token, message, innerException)
		{

		}

		protected SemanticException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}