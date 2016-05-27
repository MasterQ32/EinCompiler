using System;

namespace EinCompiler.RawSyntaxTree
{
	public abstract class RawInstructionNode : RawSyntaxNode
	{
		public abstract InstructionDescription Translate(
			TypeContainer types, 
			VariableContainer vars, 
			FunctionContainer funcs);
	}

	public sealed class RawExpressionInstructionNode : RawInstructionNode
	{
		public RawExpressionInstructionNode(RawExpressionNode expression)
		{
			this.Expression = expression;
		}

		public RawExpressionNode Expression { get; private set; }

		public override InstructionDescription Translate(
			TypeContainer types, 
			VariableContainer vars, 
			FunctionContainer funcs)
		{
			var expression = this.Expression.Translate(types, vars, funcs);
			if (expression.IsTopLevelPossible == false)
				throw new InvalidOperationException();
			// Deduce and check all invalid types.
			expression.DeduceAndCheckType(null);
			return new ExpressionInstruction(expression);
		}

		public override string ToString() => $"{Expression}";
	}

	public sealed class RawReturnInstructionNode : RawInstructionNode
	{
		public RawReturnInstructionNode(RawExpressionNode expression)
		{
			this.Expression = expression;
		}

		public override InstructionDescription Translate(
			TypeContainer types, 
			VariableContainer vars, 
			FunctionContainer funcs)
		{
			return new ReturnInstruction(
				this.Expression.Translate(types, vars, funcs));
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

		public override InstructionDescription Translate(
			TypeContainer types, 
			VariableContainer vars,
			FunctionContainer funcs)
		{
			return new ConditionalInstruction(
				this.Condition.Translate(types, vars, funcs),
				this.TrueBody.Translate(types, vars, funcs),
				this.FalseBody?.Translate(types, vars, funcs));
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

		public override InstructionDescription Translate(
			TypeContainer types, 
			VariableContainer vars,
			FunctionContainer funcs)
		{
			return new LoopInstruction(
				this.Condition.Translate(types, vars, funcs),
				this.Body.Translate(types, vars, funcs));
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

		public override InstructionDescription Translate(
			TypeContainer types, 
			VariableContainer vars,
			FunctionContainer funcs)
		{
			return new BreakLoopInstruction();
		}

		public override string ToString() => "break";
	}
}