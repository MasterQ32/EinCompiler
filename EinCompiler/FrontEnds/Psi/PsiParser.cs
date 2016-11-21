using EinCompiler.RawSyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EinCompiler.FrontEnds
{
	using static PsiTokens;

	public sealed class PsiParser : Parser
	{
		static Regex unescaper = new Regex(@"\\(.)", RegexOptions.Compiled);

		Token ReadValue() => this.ReadToken(NUMBER, STRING);

		protected override RawSyntaxNode ReadNext()
		{
			var attribs = ReadAttributes();

			var @class = this.ReadToken(KEYWORD);
			switch (@class.Text)
			{
				case "include":
					{
						this.ReadToken(O_BRACKET);
						var text = this.ReadToken(STRING);
						this.ReadToken(C_BRACKET);
						this.ReadDelimiter();

						var fileName = Unescape(text.Text);
						fileName = fileName.Substring(1, fileName.Length - 2);
						return new RawIncludeNode(fileName);
					}
				case "const":
					{
						var name = this.ReadToken(IDENTIFIER);
						this.ReadToken(COLON);
						var type = this.ReadType();
						this.ReadToken(ASSIGNMENT);
						var value = this.ReadValue();
						this.ReadDelimiter();
						return new RawConstantNode(name, type, value) { Attributes = attribs };
					}
				case "private":
				case "static":
				case "shared":
				case "global":
				case "var":
					{
						var classes = new List<Token>();
						while (@class.Text != "var")
						{
							classes.Add(@class);
							@class = this.ReadToken(KEYWORD);
						}
						var name = this.ReadToken(IDENTIFIER);
						this.ReadToken(COLON);
						var type = this.ReadType();
					
						Token value = null;

						var option = this.PeekToken(DELIMITER, ASSIGNMENT);
						if (option.Type == ASSIGNMENT)
						{
							this.ReadToken(ASSIGNMENT);
							value = this.ReadValue();
						}
						this.ReadDelimiter();

						return new RawVariableNode(
							name,
							type,
							value,
							classes.ToArray())
						{
							Attributes = attribs 
						};
					}
				case "export":
				case "naked":
				case "inline":
				case "extern":
				case "fn":
					{
						var modifiers = new List<string>();
						while (@class.Text != "fn")
						{
							modifiers.Add(@class.Text);
							@class = this.ReadToken(KEYWORD);
						}

						var parameters = new List<RawParameterNode>();
						var name = this.ReadToken(IDENTIFIER);
						this.ReadToken(O_BRACKET);
						while (this.PeekToken().Type != C_BRACKET)
						{
							var pname = this.ReadToken(IDENTIFIER);
							this.ReadToken(COLON);
							var ptype = this.ReadType();
							parameters.Add(new RawParameterNode(pname, ptype));
							if (this.PeekToken().Type != C_BRACKET)
								this.ReadToken(SEPARATOR);
						}
						this.ReadToken(C_BRACKET);
						RawTypeNode returnType = null;
						if (this.PeekToken().Type == ARROW)
						{
							this.ReadToken(ARROW);
							returnType = this.ReadType();
						}

						if (modifiers.Contains("extern"))
						{
							this.ReadToken(DELIMITER);
							return new RawExternFunctionNode(
								name,
								returnType,
								parameters)
							{ 
								Attributes = attribs 
							};
						}
						else
						{
							if (modifiers.Contains("naked") || modifiers.Contains("inline"))
							{
								var body = this.ReadToken(RAW_BLOCK);
								return new RawNakedFunctionNode(
									name,
									returnType,
									parameters,
									body.Text)
								{
									IsInline = modifiers.Contains("inline"),
									Attributes = attribs,
								};
							}
							else
							{
								var locals = new List<RawLocal>();

								/*
							while (this.PeekToken ().Type != O_CBRACKET) {
								var tok = this.ReadToken (KEYWORD);

								switch (tok.Text) {
								case "var": // local variable in this case
									{
										var locname = this.ReadToken (IDENTIFIER);
										this.ReadToken (COLON);
										var loctype = this.ReadType ();
										this.ReadDelimiter ();
										locals.Add (new RawLocal (locname, loctype));
										break;
									}
								default:
									throw new ParserException (tok);
								}
							}
							*/

								var body = this.ReadBody(locals);

								return new RawFunctionNode(
									name,
									returnType,
									parameters,
									locals,
									body)
								{
									IsExported = modifiers.Contains("export"),
									Attributes = attribs,
								};
							}
						}
					}
				default:
					{
						throw new ParserException(@class);
					}
			}
		}

		private List<AttributeDeclaration> ReadAttributes()
		{
			var attributes = new List<AttributeDeclaration>();
			while(PeekToken().Type == COLON)
			{
				ReadToken(COLON);

				var args = new List<string>();
				var name = ReadToken(IDENTIFIER);
				if(PeekToken().Type == O_BRACKET)
				{
					ReadToken(O_BRACKET);

					for(int i = 0; PeekToken().Type != C_BRACKET ; i++)
					{
						if(i > 0)
							ReadToken(SEPARATOR);
						var token = ReadToken(STRING, NUMBER, IDENTIFIER);
						args.Add(token.Text);
					}

					ReadToken(C_BRACKET);
				}

				attributes.Add(new AttributeDeclaration(name.Text, args));
			}
			return attributes;
		}

		private RawTypeNode ReadType()
		{
			var name = this.ReadToken(IDENTIFIER);
			var option = this.PeekToken();

			Token arraySize = null;
			if (option.Type == O_SBRACKET)
			{
				this.ReadToken(O_SBRACKET);
				arraySize = this.ReadToken(NUMBER, C_SBRACKET);
				if(arraySize.Type == NUMBER)
					this.ReadToken(C_SBRACKET);
			}

			return new RawTypeNode(name, arraySize);
		}

		private void ReadDelimiter() => this.ReadToken(DELIMITER);

		private string Unescape(string text) =>
			unescaper.Replace(text, (m) =>
			{
				switch(m.Groups[1].Value)
				{
					case "n": return "\n";
					case "r": return "\r";
					case "t": return "\t";
					case "b": return "\b";
					case "\\": return "\\";
					case "\"": return "\"";
					case "\'": return "\'";
					default: return m.Value;
				}
			});

		private RawBodyNode ReadBody(IList<RawLocal> locals)
		{
			var body = new List<RawInstructionNode>();

			this.ReadToken(O_CBRACKET);
			while (this.PeekToken().Type != C_CBRACKET)
			{
				var instruction = this.ReadInstruction(locals);
				if (instruction != null)
				{
					body.Add(instruction);
				}
			}
			this.ReadToken(C_CBRACKET);

			return new RawBodyNode(body);
		}

		private RawInstructionNode ReadInstruction(IList<RawLocal> locals)
		{
			var tok = this.PeekToken();

			if (tok.Type == KEYWORD)
			{
				switch (tok.Text)
				{
					case "var":
						{
							this.ReadToken(KEYWORD);

							var locname = this.ReadToken(IDENTIFIER);
							this.ReadToken(COLON);
							var loctype = this.ReadType();

							locals.Add(new RawLocal(locname, loctype));

							if (PeekToken().Type == ASSIGNMENT)
							{

								var ass = this.ReadToken(ASSIGNMENT);

								var expr = this.ReadTokensUntil(DELIMITER);

								return new RawExpressionInstructionNode(new RawBinaryOperatorExpressionNode(
										ass,
										new RawVariableExpressionNode(locname),
										ConvertToExpression(expr)));
							}
							else
							{
								this.ReadDelimiter();
								return null;
							}
						}
					case "break":
						{
							this.ReadToken(KEYWORD);
							this.ReadDelimiter();
							return new RawBreakInstructionNode();
						}
					case "return":
						{
							this.ReadToken(KEYWORD);
							var items = ReadTokensUntil(DELIMITER);
							return new RawReturnInstructionNode(
								ConvertToExpression(items));
						}
					case "if":
						{
							this.ReadToken(KEYWORD);
							this.ReadToken(O_BRACKET);
							var condition = ConvertToExpression(
								                this.ReadTokensUntil(O_BRACKET, C_BRACKET));
							var trueBlock = this.ReadBody(locals);
							RawBodyNode falseBlock = null;
							if (
								this.PeekToken().Type == KEYWORD &&
								this.PeekToken().Text == "else")
							{
								this.ReadToken(KEYWORD);
								falseBlock = this.ReadBody(locals);
							}

							return new RawIfInstructionNode(
								condition,
								trueBlock,
								falseBlock);
						}
					case "while":
						{
							this.ReadToken(KEYWORD);
							this.ReadToken(O_BRACKET);
							var condition = ConvertToExpression(
								                this.ReadTokensUntil(O_BRACKET, C_BRACKET));
							var block = this.ReadBody(locals);
							return new RawWhileInstructionNode(
								condition,
								block);
						}
					default:
						throw new ParserException(tok);
				}
			}
			else if (tok.Type == DELIMITER)
			{
				this.ReadDelimiter();
				return new RawNopInstructionNode();
			}
			else
			{
				var items = ReadTokensUntil(DELIMITER);
				return new RawExpressionInstructionNode(
					this.ConvertToExpression(items));
			}
		}

		private Token[] ReadTokensUntil(TokenType delimiter) =>
			ReadTokensUntil(t => t.Type == delimiter);

		private Token[] ReadTokensUntil(TokenType begin, TokenType end)
		{
			int depth = 0;

			return ReadTokensUntil(t =>
				{
					if (t.Type == begin)
					{
						depth++;
					}
					else if (t.Type == end)
					{
						if (depth == 0)
						{
							return true;
						}
						depth -= 1;
					}
					return false;
				});
		}

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
				if (tok.Type == IDENTIFIER)
					return new RawVariableExpressionNode(tok);
				else if (tok.Type == STRING)
					return new RawLiteralExpressionNode(tok);
				else if (tok.Type == NUMBER)
					return new RawLiteralExpressionNode(tok);
				else if (tok.Type == CHARACTER)
					return new RawLiteralExpressionNode(tok);
				else
					throw new ParserException(tok, "Expected string, variable, constant or number.");
			}

			if (tokens[0].Type == OPERATOR)
			{
				return new RawUnaryOperatorExpressionNode(
					tokens[0],
					ConvertToExpression(tokens.Skip(1).ToArray()));
			}

			var operators = new[]
			{
				":=",
				">", "<", ">=", "<=", "=", "!=",
				"+", "-", "*", "/", "%"
			};
			foreach (var op in operators)
			{
				for (int i = 0; i < tokens.Length; i++)
				{
					if (SkipOverBracket(tokens, ref i))
						break;

					var t = tokens[i];
					if ((t.Type != OPERATOR && t.Type != ASSIGNMENT) || t.Text != op)
						continue;

					var prefix = tokens.Take(i).ToArray();
					var postfix = tokens.Skip(i + 1).ToArray();

					var lhs = ConvertToExpression(prefix);
					var rhs = ConvertToExpression(postfix);

					return new RawBinaryOperatorExpressionNode(
						tokens[i],
						lhs,
						rhs);
				}
			}

			if (tokens[0].Type == O_BRACKET)
			{
				if (tokens[tokens.Length - 1].Type != C_BRACKET)
					throw new ParserException(tokens[tokens.Length - 1], "closing bracket excepted.");
				return ConvertToExpression(tokens.Skip(1).Take(tokens.Length - 2).ToArray());
			}

			if (tokens.Length >= 3)
			{
				if (tokens[0].Type == IDENTIFIER && tokens[1].Type == O_BRACKET && tokens[tokens.Length - 1].Type == C_BRACKET)
				{
					// This is a function call
					var args = SplitTokens(
						           tokens.Skip(2).Take(tokens.Length - 3).ToArray(),
						           t => t.Type == SEPARATOR);


					return new RawFunctionCallExpression(
						tokens[0],
						args.Select(p => ConvertToExpression(p)).ToArray());
				}
				if (tokens[0].Type == IDENTIFIER && tokens[1].Type == O_SBRACKET && tokens[tokens.Length - 1].Type == C_SBRACKET)
				{
					return new RawIndexerExpression(
						tokens[0],
						ConvertToExpression(tokens.Skip(2).Take(tokens.Length - 3).ToArray()));

				}
			}

			throw new ParserException(tokens[0]);
		}

		List<Token[]> SplitTokens(Token[] tokens, Predicate<Token> isSeparator)
		{
			var list = new List<Token[]>();
			int start = 0;
			for (int i = 0; i < tokens.Length; i++)
			{
				if (SkipOverBracket(tokens, ref i))
					break;

				var t = tokens[i];
				if (isSeparator(t) == false)
					continue;

				var portion = tokens.Skip(start).Take(i - start).ToArray();
				list.Add(portion);

				start = i + 1;
			}
			if (start < tokens.Length)
			{
				var portion = tokens.Skip(start).ToArray();
				list.Add(portion);
			}
			return list;
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
			if (tokens[initial].Type != O_BRACKET)
				return false;
			int stack = 1;
			for (i = i + 1; i < tokens.Length; i++)
			{
				if (tokens[i].Type == O_BRACKET)
					stack++;
				else if (tokens[i].Type == C_BRACKET)
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
