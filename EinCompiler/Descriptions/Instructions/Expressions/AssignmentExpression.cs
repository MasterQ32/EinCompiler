using System;

namespace EinCompiler.RawSyntaxTree
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

		public override bool IsTopLevelPossible => true;

		public Expression Source { get; private set; }

		public Expression Target { get; private set; }
	}
}