using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EinCompiler
{
	public sealed class ModuleDescription
	{
		public VariableContainer Variables { get; } = new VariableContainer();

		public ConstantContainer Constants { get; } = new ConstantContainer();

		public FunctionContainer Functions { get; } = new FunctionContainer();
	}

	public class TypeContainer : DescriptionContainer<TypeDescription>
	{

	}

	public class FunctionContainer : DescriptionContainer<FunctionDescription>
	{

	}

	public class VariableContainer : DescriptionContainer<VariableDescription>
	{
		public VariableContainer(VariableContainer shadow = null) :
			base(shadow)
		{

		}
	}

	public class ConstantContainer : DescriptionContainer<ConstantDescription>
	{

	}

	public class DescriptionContainer<T> : ICollection<T>
		where T : IDescription
	{
		private readonly List<T> items = new List<T>();

		private readonly DescriptionContainer<T> shadow = null;

		public DescriptionContainer(DescriptionContainer<T> shadow = null)
		{
			this.shadow = shadow;
		}

		public T this[string name]
		{
			get
			{
				var value = this.items.FirstOrDefault(i => i.Name == name);
				if (value == null && this.shadow != null)
					value = this.shadow[name];
				if(value == null)
					throw new IndexOutOfRangeException();
				return value;
			}
		}

		public int Count
		{
			get
			{
				return items.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((ICollection<T>)items).IsReadOnly;
			}
		}

		public void Add(T item)
		{
			if (this.items.Any(i => i.Name == item.Name))
				throw new InvalidOperationException("An item with this name already exists.");
			items.Add(item);
		}

		public void Clear()
		{
			items.Clear();
		}

		public bool Contains(T item)
		{
			return items.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((ICollection<T>)items).GetEnumerator();
		}

		public bool Remove(T item)
		{
			return items.Remove(item);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((ICollection<T>)items).GetEnumerator();
		}
	}
}