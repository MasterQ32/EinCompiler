using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace EinCompiler
{
	[Serializable]
	public sealed class TokenizerException : CompilationException
	{
		public TokenizerException(Token token) : base(token, $"{token.Text} ({token.Type.Name}) is not a valid token.")
		{
			
		}

		public TokenizerException(Token token, string message) : base(token, message)
		{
			
		}

		public TokenizerException(Token token, string message, Exception innerException) : base(token, message, innerException)
		{

		}

		private TokenizerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}
	}
}