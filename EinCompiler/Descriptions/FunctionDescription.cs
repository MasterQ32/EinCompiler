using System;
using System.Collections.Generic;

namespace EinCompiler
{
	public sealed class FunctionDescription : IDescription
	{
		public FunctionDescription(
			string name,
			TypeDescription returnType,
			ParameterDescription[] parameters,
			LocalDecription[] locals)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));
			if (returnType == null) throw new ArgumentNullException(nameof(returnType));
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));
			if (locals == null) throw new ArgumentNullException(nameof(locals));
			this.Name = name;
			this.ReturnType = returnType;
			this.Parameters = parameters;
			this.Locals = locals;
		}

		public FunctionDescription(
			string name,
			TypeDescription returnType,
			ParameterDescription[] parameters,
			LocalDecription[] locals,
			BodyDescription body) :
			this(name, returnType, parameters, locals)
		{
			if (body == null) throw new ArgumentNullException(nameof(body));
			this.Body = body;
		}

		public FunctionDescription(
			string name,
			TypeDescription returnType,
			ParameterDescription[] parameters,
			string body) :
			this(name, returnType, parameters, new LocalDecription[0])
		{
			if (body == null) throw new ArgumentNullException(nameof(body));
			this.NakedBody = body;
		}

		public BodyDescription Body { get; set; }

		public string Name { get; private set; }

		public bool IsNaked => (this.NakedBody != null);

		public bool IsInline { get; set; }

		public IReadOnlyList<ParameterDescription> Parameters { get; private set; }

		public TypeDescription ReturnType { get; private set; }

		public string NakedBody { get; private set; }

		public IReadOnlyList<LocalDecription> Locals { get; private set; }

		public override string ToString() =>
			$"{ReturnType} {Name}({string.Join(", ", Parameters)})";
	}
}