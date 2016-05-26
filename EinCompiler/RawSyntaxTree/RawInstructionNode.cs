using System;

namespace EinCompiler.RawSyntaxTree
{
	public abstract class RawInstructionNode : RawSyntaxNode
	{
		public abstract InstructionDescription Translate(TypeContainer types);
	}

	public sealed class RawExpressionInstructionNode : RawInstructionNode
	{
		public RawExpressionInstructionNode(RawExpressionNode expression)
		{
			this.Expression = expression;
		}

		public RawExpressionNode Expression { get; private set; }

		public override InstructionDescription Translate(TypeContainer types)
		{
			throw new NotImplementedException();
		}

		public override string ToString() => $"{Expression}";
	}

	public sealed class RawReturnInstructionNode : RawInstructionNode
	{
		public RawReturnInstructionNode(RawExpressionNode expression)
		{
			this.Expression = expression;
		}

		public override InstructionDescription Translate(TypeContainer types)
		{
			throw new NotImplementedException();
		}

		public RawExpressionNode Expression { get; private set; }

		public override string ToString() => $"return {Expression}";
	}

	public sealed class RawIfInstructionNode : RawInstructionNode
	{
		public RawIfInstructionNode(
			RawExpressionNode condition,
			RawBodyNode trueBody,
			RawBodyNode falseBody)
		{
			this.TrueBody = trueBody;
			this.FalseBody = falseBody;
			this.Condition = condition;
		}

		public override InstructionDescription Translate(TypeContainer types)
		{
			throw new NotImplementedException();
		}

		public RawBodyNode TrueBody { get; private set; }

		public RawExpressionNode Condition { get; private set; }
		public RawBodyNode FalseBody { get; private set; }

		public override string ToString() => $"if ( {Condition} ) {{ {TrueBody} }} else {{ {FalseBody} }}";
	}

	public sealed class RawWhileInstructionNode : RawInstructionNode
	{
		public RawWhileInstructionNode(
			RawExpressionNode condition,
			RawBodyNode body)
		{
			this.Body = body;
			this.Condition = condition;
		}

		public override InstructionDescription Translate(TypeContainer types)
		{
			throw new NotImplementedException();
		}

		public RawBodyNode Body { get; private set; }

		public RawExpressionNode Condition { get; private set; }

		public override string ToString() => $"while ( {Condition} ) {{ {Body} }} ";
	}

	public sealed class RawBreakInstructionNode : RawInstructionNode
	{
		public RawBreakInstructionNode()
		{

		}

		public override InstructionDescription Translate(TypeContainer types)
		{
			throw new NotImplementedException();
		}

		public override string ToString() => "break";
	}
}