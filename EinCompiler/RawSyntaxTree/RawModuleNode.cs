using System;
using System.Collections.Generic;
using System.Linq;

namespace EinCompiler.RawSyntaxTree
{
	public sealed class RawModuleNode : RawSyntaxNode
	{
		private List<RawFunctionNode> functions = new List<RawFunctionNode>();
		private List<RawVariableNode> variables = new List<RawVariableNode>();
		private List<RawConstantNode> constants = new List<RawConstantNode>();

		public RawModuleNode()
		{

		}

		public void Add(RawSyntaxNode node)
		{
			if (node is RawFunctionNode)
			{
				functions.Add((RawFunctionNode)node);
			}
			else if (node is RawVariableNode)
			{
				variables.Add((RawVariableNode)node);
			}
			else if (node is RawConstantNode)
			{
				constants.Add((RawConstantNode)node);
			}
			else
			{
				throw new NotSupportedException($"{node} is not a supported node type.");
			}
		}

		public ModuleDescription Translate(TypeContainer types)
		{
			var description = new ModuleDescription();

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

			

			return description;
		}

		public ICollection<RawFunctionNode> Functions => this.functions;

		public ICollection<RawVariableNode> Variables => this.variables;

		public ICollection<RawConstantNode> Constants => this.constants;
	}
}