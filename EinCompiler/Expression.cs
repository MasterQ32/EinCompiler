namespace EinCompiler
{
	public abstract class Expression
	{
		public virtual bool IsAssignable => false;

		public virtual bool IsTopLevelPossible => false;
	}
}