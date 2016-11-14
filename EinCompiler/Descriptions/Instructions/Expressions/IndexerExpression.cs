using System;

namespace EinCompiler
{
	public sealed class IndexerExpression : Expression
	{
		private readonly ArrayType type;

		public IndexerExpression(VariableDescription var, Expression expression)
		{
			if (var == null) throw new ArgumentNullException(nameof(var));
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			this.type = var.Type as ArrayType;
			if (this.type == null)
				throw new ArgumentException("The variable must be of an array type.", nameof(var));

			this.Array = var;
			this.Index = expression;
		}

		public override void DeduceAndCheckType(TypeDescription typeHint)
		{
			this.Index.DeduceAndCheckType(Types.Int32);

			if (!(this.Index.Type is IntegerType))
				throw new SemanticException(null, "Array indexing requires an integer type.");
		}

		public override TypeDescription Type => this.type.ElementType;

		public override bool IsAssignable => true;

		public VariableDescription Array { get; private set; }

		public Expression Index { get; private set; }
	}
}