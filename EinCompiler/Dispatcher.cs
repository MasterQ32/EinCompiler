using System;
using System.Collections.Generic;

namespace EinCompiler
{
	public sealed class Dispatcher<TObject, TArgs>
	{
		public delegate void Function(TObject obj, TArgs args);

		public delegate void DispatchFunction<T>(T obj, TArgs args)
			where T : TObject;

		private readonly Dictionary<Type, Function> handlers = new Dictionary<Type, Function>();

		public Dispatcher()
		{

		}

		public void Register<T>(DispatchFunction<T> func)
			where T : TObject
		{
			handlers [typeof(T)] = (o,a) => func((T)o, a);
		}

		public void Invoke<T>(T obj, TArgs args) 
			where T : TObject
		{
			var type = obj.GetType ();
			while(type != null && type != typeof(TObject))
			{
				Function func;
				if (handlers.TryGetValue (type, out func)) {
					func.Invoke (obj, args);
					return;
				}
				type = type.BaseType;
			}
			throw new InvalidOperationException($"No handler registered for {obj.GetType().Name}.");
		}
	}
}

