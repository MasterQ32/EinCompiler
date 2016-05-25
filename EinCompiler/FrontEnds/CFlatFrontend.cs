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
					while(this.PeekToken().Type.Name != "C_BRACKET")
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
					if(this.PeekToken().Type.Name == "ARROW")
					{
						this.ReadToken("ARROW");
						returnType = this.ReadToken("IDENTIFIER");
					}

					this.ReadToken("O_CBRACKET");

					while (this.ReadToken().Type.Name != "C_CBRACKET") ;

					return new RawFunctionNode(
						name.Text,
						returnType?.Text,
						parameters)
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
	}
}
