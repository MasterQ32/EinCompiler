using EinCompiler.RawSyntaxTree;
using System;
using System.Linq;

namespace EinCompiler
{
	public sealed class ModuleInstancer
	{
		private readonly Func<string, ModuleDescription> fileLoader;
		private readonly TypeContainer types;

		public ModuleInstancer(TypeContainer types, Func<string, ModuleDescription> fileLoader)
		{
			this.types = types;
			this.fileLoader = fileLoader;
		}
		
		public ModuleDescription CreateInstance(RawModuleNode node)
		{
			var description = new ModuleDescription();

			foreach (var include in node.Includes)
			{
				var module = LoadSubModule(include);

				description.Merge(module);
			}

			foreach (var con in node.Constants)
			{
				description.Constants.Add(CreateConstant(con));
			}

			foreach (var var in node.Variables)
			{
				VariableDescription vardesc = CreateVariable(var);

				description.Variables.Add(vardesc);
			}

			// First, create all function prototypes to make
			// them available at all code.
			foreach (var func in node.Functions)
			{
				description.Functions.Add(new FunctionDescription(
					func.Name,
					func.ReturnType != null ? types[func.ReturnType] : TypeDescription.Void,
					func.Parameters.Select(p => new ParameterDescription(types[p.Type], p.Name)).ToArray(),
					func.Locals.Select((l, i) => new LocalDecription(types[l.Type], l.Name, i)).ToArray()));
			}

			// Second, create all naked functions with their
			// bodies already assigned.
			foreach (var naked in node.NakedFunctions)
			{
				description.Functions.Add(new FunctionDescription(
					naked.Name,
					naked.ReturnType != null ? types[naked.ReturnType] : TypeDescription.Void,
					naked.Parameters.Select(p => new ParameterDescription(types[p.Type], p.Name)).ToArray(),
					naked.Body.Substring(2, naked.Body.Length - 4))
				{
					IsInline = naked.IsInline
				});
			}

			// Then, translate all function bodies to
			// have them without the need of forward
			// declaration
			foreach (var func in node.Functions)
			{
				var localVariables = new VariableContainer(description.Variables);

				for (int i = 0; i < func.Parameters.Count; i++)
				{
					var var = new ParameterVariableDescription(
						types[func.Parameters[i].Type],
						func.Parameters[i].Name,
						i);
					localVariables.Add(var);
				}
				foreach (var local in description.Functions[func.Name].Locals)
				{
					localVariables.Add(local);
				}


				description.Functions[func.Name].Body =
					func.Body.Translate(
						types,
						localVariables,
						description.Functions);
			}

			return description;
		}

		private ModuleDescription LoadSubModule(string include) => fileLoader(include);

		private TypeDescription CreateType(RawTypeNode node)
		{
			if (node.ArraySize != null)
				throw new InvalidOperationException("Arrays not supported.");
			return types[node.Name.Text];
		}

		private ConstantDescription CreateConstant(RawConstantNode con)
		{
			var type = CreateType(con.Type);
			return new ConstantDescription(
				type,
				con.Name.Text,
				type.CreateValueFromString(con.Value.Text));
		}

		private VariableDescription CreateVariable(RawVariableNode var)
		{
			VariableDescription vardesc;

			var type = CreateType(var.Type);

			if (var.Value != null)
			{
				vardesc = new VariableDescription(
					type,
					var.Name.Text,
					type.CreateValueFromString(var.Value.Text));
			}
			else
			{
				vardesc = new VariableDescription(
					type,
					var.Name.Text);
			}

			foreach (var mod in var.Modifiers)
			{
				var storage = (StorageModifier)Enum.Parse(typeof(StorageModifier), mod.Text, true);
				vardesc.Storage |= storage;
			}

			var validMods = new[]
			{
					StorageModifier.Global,
					StorageModifier.Static,
					StorageModifier.Private,
					StorageModifier.Shared,
					StorageModifier.Private | StorageModifier.Static,
				};
			if (validMods.Contains(vardesc.Storage) == false)
				throw new InvalidOperationException("Invalid variable storage.");
			return vardesc;
		}
	}
}