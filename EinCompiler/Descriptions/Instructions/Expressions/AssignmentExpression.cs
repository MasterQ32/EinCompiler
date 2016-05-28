using System;

namespace EinCompiler
{
	public sealed class AssignmentExpression : Expression
	{
		public AssignmentExpression(Expression target, Expression source)
		{
			if (target.IsAssignable == false)
				throw new ArgumentException("Target must be an assignable expression.", nameof(target));

			this.Target = target;
			this.Source = source;
		}

		public override void DeduceAndCheckType(TypeDescription typeHint)
		{
			// First, pass the type hint to the assignment target.
			this.Target.DeduceAndCheckType(typeHint);

			// Then deduce the type by the passing the targets type to the source.
			this.Source.DeduceAndCheckType(this.Target.Type);

			if (this.Source.Type != this.Type)
				throw new InvalidOperationException();
		}

		public override TypeDescription Type => this.Target.Type;

		public override bool IsTopLevelPossible => true;

		public Expression Source { get; private set; }

		public Expression Target { get; private set; }

		public override string ToString()
		{
			return this.Target + " := " + this.Source;
		}
	}
}