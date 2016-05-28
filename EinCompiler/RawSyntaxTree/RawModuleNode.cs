using System;
using System.Collections.Generic;
using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawModuleNode : RawSyntaxNode
	{
		private List<RawFunctionNode> functions = new List<RawFunctionNode>();
		private List<RawNakedFunctionNode> nakedFunctions = new List<RawNakedFunctionNode>();
		private List<RawVariableNode> variables = new List<RawVariableNode>();
		private List<RawConstantNode> constants = new List<RawConstantNode>();
		private List<string> includes = new List<string>();

		public RawModuleNode()
		{

		}

		public void Add(RawSyntaxNode node)
		{
			if (node is RawFunctionNode)
			{
				functions.Add((RawFunctionNode)node);
			}
			else if (node is RawNakedFunctionNode)
			{
				nakedFunctions.Add((RawNakedFunctionNode)node);
			}
			else if (node is RawVariableNode)
			{
				variables.Add((RawVariableNode)node);
			}
			else if (node is RawConstantNode)
			{
				constants.Add((RawConstantNode)node);
			}
			else if(node is RawIncludeNode)
			{
				includes.Add(((RawIncludeNode)node).FileName);
			}
			else
			{
				throw new NotSupportedException($"{node} is not a supported node type.");
			}
		}

		public ModuleDescription Translate(TypeContainer types, Func<string, ModuleDescription> fileLoader )
		{
			var description = new ModuleDescription();

			foreach(var include in this.includes)
			{
				var module = fileLoader(include);

				description.Merge(module);
			}

			foreach (var con in this.constants)
			{
				description.Constants.Add(new ConstantDescription(
					types[con.Type],
					con.Name,
					types[con.Type].CreateValueFromString(con.Value)));
			}

			foreach (var var in this.variables)
			{
				VariableDescription vardesc;
				if (var.Value != null)
				{
					vardesc = new VariableDescription(
						types[var.Type],
						var.Name,
						types[var.Type].CreateValueFromString(var.Value));
				}
				else
				{
					vardesc = new VariableDescription(
						types[var.Type],
						var.Name);
				}
				
				foreach(var mod in var.Modifiers)
				{
					var storage = (StorageModifier)Enum.Parse(typeof(StorageModifier), mod, true);
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

				description.Variables.Add(vardesc);
			}

			// First, create all function prototypes to make
			// them available at all code.
			foreach(var func in this.functions)
			{
				description.Functions.Add(new FunctionDescription(
					func.Name,
					func.ReturnType != null ? types[func.ReturnType] : TypeDescription.Void,
					func.Parameters.Select(p => new ParameterDescription(types[p.Type], p.Name)).ToArray(),
					func.Locals.Select((l, i) => new LocalDecription(types[l.Type], l.Name, i)).ToArray()));
			}

			// Second, create all naked functions with their
			// bodies already assigned.
			foreach(var naked in this.nakedFunctions)
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
			foreach (var func in this.functions)
			{
				var localVariables = new VariableContainer(description.Variables);

				for(int i = 0; i < func.Parameters.Count; i++)
				{
					var var = new ParameterVariableDescription(
						types[func.Parameters[i].Type],
						func.Parameters[i].Name,
						i);
					localVariables.Add(var);
				}
				foreach(var local in description.Functions[func.Name].Locals)
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

		public ICollection<RawFunctionNode> Functions => this.functions;

		public ICollection<RawNakedFunctionNode> NakedFunctions => this.nakedFunctions;

		public ICollection<RawVariableNode> Variables => this.variables;

		public ICollection<RawConstantNode> Constants => this.constants;

		public ICollection<string> Includes => this.includes;
	}
}