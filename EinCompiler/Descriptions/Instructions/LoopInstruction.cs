using System;

namespace EinCompiler
{
	public sealed class LoopInstruction : InstructionDescription
	{
		public LoopInstruction(Expression condition, BodyDescription body)
		{
			if (condition == null) throw new ArgumentNullException(nameof(condition));
			if (body == null) throw new ArgumentNullException(nameof(body));
			this.Condition = condition;
			this.Body  = body;
		}

		public BodyDescription Body { get; private set; }

		public Expression Condition { get; private set; }
		
		public override string ToString() => "while(" + Condition + ") { ... }";
	}
}