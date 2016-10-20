using EinCompiler.BuiltInTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EinCompiler
{
	public sealed class ModuleDescription
	{
		public TypeContainer Types { get; set; } = new TypeContainer();

		public VariableContainer Variables { get; } = new VariableContainer();

		public ConstantContainer Constants { get; } = new ConstantContainer();

		public FunctionContainer Functions { get; } = new FunctionContainer();

		public void Merge(ModuleDescription module)
		{
			if (module == null) throw new ArgumentNullException(nameof(module));

			foreach (var c in module.Constants)
				this.Constants.Add(c);

			foreach (var var in module.Variables)
				this.Variables.Add(var);

			foreach (var fn in module.Functions)
				this.Functions.Add(fn);
		}
	}

	public class TypeContainer : IReadOnlyDictionary<string, TypeDescription>
	{
		private readonly Dictionary<string, TypeDescription> contents = new Dictionary<string, TypeDescription>();

		public TypeContainer()
		{

		}

		public void Add(TypeDescription type) => Add(type.Name, type);

		public void Add(string alias, TypeDescription type) => contents.Add(alias, type);

		public TypeDescription this[string key] => ContainsKey(key) ? contents[key] : null;

		public TypeDescription Boolean { get; set; }

		public int Count => contents.Count;

		public IEnumerable<string> Keys => contents.Keys;

		public IEnumerable<TypeDescription> Values => contents.Values;

		public bool ContainsKey(string key) => contents.ContainsKey(key);

		public TypeDescription GetArrayType(TypeDescription elementType, int length)
		{
			var name = elementType.Name + "[" + length + "]";
			var type = this[name];
			if (type == null)
			{
				type = new ArrayType(elementType, length);
				this.Add(type);
			}
			return type;
		}

		public IEnumerator<KeyValuePair<string, TypeDescription>> GetEnumerator() => contents.GetEnumerator();

		public bool TryGetValue(string key, out TypeDescription value)
		{
			return contents.TryGetValue(key, out value);
		}

		IEnumerator IEnumerable.GetEnumerator() => contents.GetEnumerator();
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