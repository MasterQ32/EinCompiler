using System;

namespace EinCompiler
{
	public sealed class ConditionalInstruction : InstructionDescription
	{
		public ConditionalInstruction(Expression condition, BodyDescription trueBody, BodyDescription falseBody)
		{
			if (condition == null) throw new ArgumentNullException(nameof(condition));
			if (trueBody == null) throw new ArgumentNullException(nameof(trueBody));
			this.Condition = condition;
			this.TrueBody = trueBody;
			this.FalseBody = falseBody;
		}

		public Expression Condition { get; private set; }

		public BodyDescription FalseBody { get; private set; }

		public BodyDescription TrueBody { get; private set; }

		public override string ToString()
		{
			if(FalseBody != null)
				return "if(" + this.Condition + ") { ... } else { ... }";
			else
			return "if(" + this.Condition + ") { ... }";
		}
	}
}