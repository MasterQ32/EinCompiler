using System;
using System.Runtime.Serialization;

namespace EinCompiler.FrontEnds
{
	[Serializable]
	internal class ParserException : Exception
	{
		public ParserException(Token token) :
			base($"{token.Text} ({token.Type.Name}) was not expected.")
		{
			this.Token = token;
		}

		public ParserException(Token token, string message) : base(message)
		{
			this.Token = token;
		}

		public ParserException(Token token, string message, Exception innerException) : base(message, innerException)
		{
			this.Token = token;
		}

		protected ParserException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.Token = (Token)info.GetValue("token", typeof(Token));
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("token", this.Token, typeof(Token));
		}

		public Token Token { get; private set; }
	}
}