using EinCompiler.RawSyntaxTree;
using System.Linq;

namespace EinCompiler.FrontEnds
{
	public abstract class Parser
	{
		private Token[] tokens;
		private int cursor = 0;

		protected Parser()
		{

		}

		public static RawSyntaxNode Parse<T>(Token[] tokens)
			where T : Parser, new()
		{
			var tree = new RawModuleNode();
			var parser = new T();
			parser.tokens = tokens;

			while (parser.cursor < parser.tokens.Length)
			{
				var node = parser.ReadNext();

				tree.Add(node);
			}

			return tree;
		}

		/// <summary>
		/// Returns the current token and moves the cursor one step further.
		/// </summary>
		/// <returns></returns>
		protected Token ReadToken() => this.tokens[this.cursor++];

		/// <summary>
		/// Peeks the current token. Does not modify the cursor.
		/// </summary>
		/// <returns></returns>
		protected Token PeekToken() => this.tokens[this.cursor];

		/// <summary>
		/// Reads a token of a specified type. If the type of the peeked token does not match, an exception is thrown.
		/// </summary>
		/// <param name="validType"></param>
		/// <returns></returns>
		protected Token PeekToken(params string[] validType)
		{
			var tok = this.PeekToken();
			if (validType.Contains(tok.Type.Name) == false)
				throw new ParserException(tok, $"Expected {string.Join(",", validType)}.");
			return tok;
		}

		/// <summary>
		/// Gets a token of a specified type. If the type of the peeked token does not match, an exception is thrown.
		/// </summary>
		/// <param name="validType"></param>
		/// <returns></returns>
		protected Token ReadToken(params string[] validType)
		{
			var tok = this.ReadToken();
			if (validType.Contains(tok.Type.Name) == false)
				throw new ParserException(tok, $"Expected {string.Join(",", validType)}.");
			return tok;
		}

		protected abstract RawSyntaxNode ReadNext();
	}
}