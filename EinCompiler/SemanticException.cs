using System;
using System.Runtime.Serialization;

namespace EinCompiler
{
	[Serializable]
	internal class SemanticException : Exception
	{
		public SemanticException(Token errorToken) : 
			base($"{errorToken.Text} is invalid here.")
		{
			this.Token = errorToken;
		}

		public SemanticException(Token errorToken, string message) : base(message)
		{
			this.Token = errorToken;
		}

		public SemanticException(Token errorToken, string message, Exception innerException) : base(message, innerException)
		{
			this.Token = errorToken;
		}

		protected SemanticException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.Token = (Token)info.GetValue("Token", typeof(Token));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Token", this.Token, typeof(Token));
		}

		public Token Token { get; private set; }
	}
}