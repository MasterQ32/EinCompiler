using System;

namespace EinCompiler
{
	public abstract class Expression
	{
		public virtual bool IsAssignable => false;

		public virtual bool IsTopLevelPossible => false;

		public virtual TypeDescription Type => Types.Invalid;

		public abstract void DeduceAndCheckType(TypeDescription typeHint);

		public override string ToString() => $"{Type}";
	}
}