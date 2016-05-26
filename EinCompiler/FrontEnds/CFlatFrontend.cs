using EinCompiler.RawSyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EinCompiler.FrontEnds
{
	public sealed class CFlatParser : Parser
	{
		Token ReadValue() => this.ReadToken("NUMBER", "STRING");

		protected override RawSyntaxNode ReadNext()
		{
			var @class = this.ReadToken("KEYWORD");

			switch (@class.Text)
			{
				case "const":
				{
					var name = this.ReadToken("IDENTIFIER");
					this.ReadToken("COLON");
					var type = this.ReadToken("IDENTIFIER");
					this.ReadToken("ASSIGNMENT");
					var value = this.ReadValue();
					this.ReadToken("DELIMITER");
					return new RawConstantNode(name.Text, type.Text, value.Text);
				}
				case "private":
				case "static":
				case "shared":
				case "global":
				case "var":
				{
					var classes = new List<string>();
					while (@class.Text != "var")
					{
						classes.Add(@class.Text);
						@class = this.ReadToken("KEYWORD");
					}
					var name = this.ReadToken("IDENTIFIER");
					this.ReadToken("COLON");
					var type = this.ReadToken("IDENTIFIER");
					var option = this.PeekToken("DELIMITER", "ASSIGNMENT");
					Token value = null;
					if (option.Type.Name == "ASSIGNMENT")
					{
						this.ReadToken("ASSIGNMENT");
						value = this.ReadValue();
					}
					this.ReadToken("DELIMITER");

					return new RawVariableNode(
						name.Text,
						type.Text,
						value?.Text,
						classes.ToArray());
				}
				case "export":
				case "fn":
				{
					var isExported = @class.Text == "export";
					if (isExported)
					{
						@class = this.ReadToken("KEYWORD");
						if (@class.Text != "fn")
							throw new ParserException(@class);
					}

					var parameters = new List<RawParameterNode>();
					var name = this.ReadToken("IDENTIFIER");
					this.ReadToken("O_BRACKET");
					while (this.PeekToken().Type.Name != "C_BRACKET")
					{
						var pname = this.ReadToken("IDENTIFIER");
						this.ReadToken("COLON");
						var ptype = this.ReadToken("IDENTIFIER");
						parameters.Add(new RawParameterNode(pname.Text, ptype.Text));
						if (this.PeekToken().Type.Name != "C_BRACKET")
							this.ReadToken("SEPARATOR");
					}
					this.ReadToken("C_BRACKET");
					Token returnType = null;
					if (this.PeekToken().Type.Name == "ARROW")
					{
						this.ReadToken("ARROW");
						returnType = this.ReadToken("IDENTIFIER");
					}

					var body = this.ReadBody();

					return new RawFunctionNode(
						name.Text,
						returnType?.Text,
						parameters,
						body)
					{
						IsExported = isExported,
					};
				}
				default:
				{
					throw new ParserException(@class);
				}
			}

		}

		private RawBodyNode ReadBody()
		{
			var body = new List<RawInstructionNode>();

			this.ReadToken("O_CBRACKET");
			while (this.PeekToken().Type.Name != "C_CBRACKET")
			{
				var instruction = this.ReadInstruction();
				body.Add(instruction);
			}
			this.ReadToken("C_CBRACKET");

			return new RawBodyNode(body);
		}

		private RawInstructionNode ReadInstruction()
		{
			var tok = this.PeekToken();

			if (tok.Type.Name == "KEYWORD")
			{
				switch (tok.Text)
				{
					case "break":
					{
						this.ReadToken("KEYWORD");
						return new RawBreakInstructionNode();
					}
					case "return":
					{
						this.ReadToken("KEYWORD");
						var items = ReadTokensUntil("DELIMITER");
						return new RawReturnInstructionNode(
							ConvertToExpression(items));
					}
					case "if":
					{
						this.ReadToken("KEYWORD");
						this.ReadToken("O_BRACKET");
						var condition = ConvertToExpression(
							this.ReadTokensUntil("C_BRACKET"));
						var trueBlock = this.ReadBody();
						RawBodyNode falseBlock = null;
						if(
							this.PeekToken().Type.Name == "KEYWORD" && 
							this.PeekToken().Text == "else")
						{
							this.ReadToken("KEYWORD");
							falseBlock = this.ReadBody();
						}

						return new RawIfInstructionNode(
							condition,
							trueBlock,
							falseBlock);
					}
					case "while":
					{
						this.ReadToken("KEYWORD");
						this.ReadToken("O_BRACKET");
						var condition = ConvertToExpression(
							this.ReadTokensUntil("C_BRACKET"));
						var block = this.ReadBody();
						return new RawWhileInstructionNode(
							condition,
							block);
					}
					default: throw new ParserException(tok);
				}
			}
			else if (tok.Type.Name == "DELIMITER")
			{
				this.ReadToken("DELIMITER");
				return new RawNopInstructionNode();
			}
			else
			{
				var items = ReadTokensUntil("DELIMITER");
				return new RawExpressionInstructionNode(
					this.ConvertToExpression(items));
			}
		}

		private Token[] ReadTokensUntil(string delimiter) =>
			ReadTokensUntil(t => t.Type.Name == delimiter);

		private Token[] ReadTokensUntil(Predicate<Token> predicate)
		{
			Token tok;
			var items = new List<Token>();
			while (!predicate(tok = this.PeekToken()))
			{
				items.Add(this.ReadToken());
			}
			tok = this.ReadToken();
			if (predicate(tok) == false)
				throw new ParserException(tok);
			return items.ToArray();
		}

		private RawExpressionNode ConvertToExpression(Token[] tokens)
		{
			if (tokens.Length == 0)
				throw new InvalidOperationException("Something went terribly wrong. Expression parsing tried to parse empty expression.");
			if (tokens.Length == 1)
			{ // trivial case: variable or literal
				var tok = tokens[0];
				if (tok.Type.Name == "IDENTIFIER")
					return new RawVariableExpressionNode(tok.Text);
				else if (tok.Type.Name == "STRING")
					return new RawLiteralExpressionNode(tok.Text);
				else if (tok.Type.Name == "NUMBER")
					return new RawLiteralExpressionNode(tok.Text);
				else
					throw new ParserException(tok, "Expected string, variable, constant or number.");
			}

			if (tokens[0].Type.Name == "UNARY_OPERATOR")
			{
				return new RawUnaryOperatorExpressionNode(
					tokens[0].Text,
					ConvertToExpression(tokens.Skip(1).ToArray()));
			}

			var operators = new[]
			{
				"=", "+", "-", "*", "/", "%"
			};
			foreach (var op in operators)
			{
				for (int i = 0; i < tokens.Length; i++)
				{
					if (SkipOverBracket(tokens, ref i))
						break;

					var t = tokens[i];
					if ((t.Type.Name != "BINARY_OPERATOR" && t.Type.Name != "ASSIGNMENT") || t.Text != op)
						continue;

					var prefix = tokens.Take(i).ToArray();
					var postfix = tokens.Skip(i + 1).ToArray();

					var lhs = ConvertToExpression(prefix);
					var rhs = ConvertToExpression(postfix);

					return new RawBinaryOperatorExpressionNode(
						op,
						lhs,
						rhs);
				}
			}

			if (tokens[0].Type.Name == "O_BRACKET")
			{
				if (tokens[tokens.Length - 1].Type.Name != "C_BRACKET")
					throw new ParserException(tokens[tokens.Length - 1], "closing bracket excepted.");
				return ConvertToExpression(tokens.Skip(1).Take(tokens.Length - 2).ToArray());
			}

			throw new ParserException(tokens[0]);
		}

		/// <summary>
		/// Skips all bracket enclosed tokens.
		/// </summary>
		/// <param name="tokens"></param>
		/// <param name="i"></param>
		/// <returns>True if the function got to the end of tokens.</returns>
		private bool SkipOverBracket(Token[] tokens, ref int i)
		{
			int initial = i;
			if (tokens[initial].Type.Name != "O_BRACKET")
				return false;
			int stack = 1;
			for (i = i + 1; i < tokens.Length; i++)
			{
				if (tokens[i].Type.Name == "O_BRACKET")
					stack++;
				else if (tokens[i].Type.Name == "C_BRACKET")
				{
					if (--stack == 0)
					{
						i += 1;
						if (i >= tokens.Length)
							return true;
						return false;
					}
				}
			}
			throw new ParserException(tokens[initial], "Closing bracket missing!");
		}
	}
}
