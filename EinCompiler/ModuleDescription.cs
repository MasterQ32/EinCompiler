using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EinCompiler
{
	public sealed class ModuleDescription
	{
		private readonly DescriptionContainer<VariableDescription> variables = new DescriptionContainer<VariableDescription>();
		private readonly DescriptionContainer<ConstantDescription> constants = new DescriptionContainer<ConstantDescription>();


		public DescriptionContainer<VariableDescription> Variables => this.variables;

		public DescriptionContainer<ConstantDescription> Constants => this.constants;
	}

	public class TypeContainer : DescriptionContainer<TypeDescription>
	{

	}

	public class DescriptionContainer<T> : ICollection<T>
		where T : IDescription
	{
		private readonly List<T> items = new List<T>();

		public T this[string name]
		{
			get { return this.items.First(i => i.Name == name); }
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