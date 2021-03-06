﻿using System;

namespace EinCompiler
{
	public sealed class VariableExpression : Expression
	{
		public VariableExpression(VariableDescription variableDescription)
		{
			if (variableDescription == null) throw new ArgumentNullException(nameof(variableDescription));
			this.Variable = variableDescription;
		}
		
		public override bool IsAssignable => true;

		public override TypeDescription Type => this.Variable.Type;

		public VariableDescription Variable { get; private set; }

		public override void DeduceAndCheckType(TypeDescription typeHint)
		{
			// Variables are correctly deduced by the variable itself.
		}

		public override string ToString()
		{
			return this.Variable.Name;
		}
	}
}