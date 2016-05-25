using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EinCompiler
{
	public abstract class RawSyntaxNode : IList<RawSyntaxNode>
	{
		private readonly List<RawSyntaxNode> children = new List<RawSyntaxNode>();
		
		public RawSyntaxNode this[int index]
		{
			get
			{
				return children[index];
			}

			set
			{
				children[index] = value;
			}
		}

		public int Count
		{
			get
			{
				return children.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IList<RawSyntaxNode>)children).IsReadOnly;
			}
		}
		
		public void Add(RawSyntaxNode item)
		{
			children.Add(item);
		}

		public void Clear()
		{
			children.Clear();
		}

		public bool Contains(RawSyntaxNode item)
		{
			return children.Contains(item);
		}

		public void CopyTo(RawSyntaxNode[] array, int arrayIndex)
		{
			children.CopyTo(array, arrayIndex);
		}

		public IEnumerator<RawSyntaxNode> GetEnumerator()
		{
			return ((IList<RawSyntaxNode>)children).GetEnumerator();
		}

		public int IndexOf(RawSyntaxNode item)
		{
			return children.IndexOf(item);
		}

		public void Insert(int index, RawSyntaxNode item)
		{
			children.Insert(index, item);
		}

		public bool Remove(RawSyntaxNode item)
		{
			return children.Remove(item);
		}

		public void RemoveAt(int index)
		{
			children.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<RawSyntaxNode>)children).GetEnumerator();
		}
	}
}
