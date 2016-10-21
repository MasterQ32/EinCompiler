using System;
using System.Runtime.Serialization;

namespace EinCompiler
{
	[Serializable]
	public abstract class CompilationException : Exception
	{
		public CompilationException(Token token, string message) : base(message)
		{
			this.Token = token;
		}

		public CompilationException(Token token, string message, Exception innerException) : base(message, innerException)
		{
			this.Token = token;
		}

		protected CompilationException(SerializationInfo info, StreamingContext context) : base(info, context)
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

