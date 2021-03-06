﻿using System;
using System.Linq;

namespace EinCompiler
{
	public sealed class FunctionCallExpression : Expression
	{
		public FunctionCallExpression(
			FunctionDescription function,
			Expression[] arguments)
		{
			if (function == null) throw new ArgumentNullException(nameof(function));
			if (arguments == null) throw new ArgumentNullException(nameof(arguments));
			if (arguments.Any(a => a == null))
				throw new ArgumentOutOfRangeException(nameof(arguments), "Null parameter is not allowed.");

			this.Function = function;
			this.Arguments = arguments;

			if (this.Arguments.Length != this.Function.Parameters.Count)
				throw new InvalidOperationException();
		}

		public override bool IsTopLevelPossible => true;

		public FunctionDescription Function { get; private set; }

		public Expression[] Arguments { get; private set; }

		public override void DeduceAndCheckType(TypeDescription typeHint)
		{
			for(int i = 0; i < this.Arguments.Length; i++)
			{
				// Deduce each argument type.
				this.Arguments[i].DeduceAndCheckType(this.Function.Parameters[i].Type);
				if (this.Arguments[i].Type != this.Function.Parameters[i].Type)
				{
					throw new InvalidOperationException($"Parameter type mismatch: The parameter {this.Function.Parameters[i].Name} received a {this.Arguments[i].Type.Name}, but expected a {this.Function.Parameters[i].Type.Name}");
				}
			}
		}

		public override TypeDescription Type => this.Function.ReturnType;

		public override string ToString()
		{
			return this.Function.Name + "(" +
				string.Join<Expression>(", ", this.Arguments) +
				")";
		}
	}
}