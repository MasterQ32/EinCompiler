using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EinCompiler.BackEnds
{
	public sealed class CCodeBackEnd : BackEnd
	{
		protected override void WriteLineComment(string text)
		{
			WriteLine("// {0}", text);
		}

		protected override void Generate(ModuleDescription module)
		{
			WriteLineComment("Constants:");
			foreach (var @const in module.Constants)
			{
				WriteConstant(@const);
			}

			WriteLine();

			WriteLineComment("Variables:");
			foreach (var var in module.Variables)
			{
				WriteVariable(var);
			}

			WriteLine();

			WriteLineComment("Variables:");
			foreach (var var in module.Variables)
			{
				WriteVariable(var);
			}
		}

		private void WriteConstant(ConstantDescription @const)
		{
			WriteLine("const {0} {1} = {2};", @const.Type.Name, @const.Name, @const.InitialValue.GetString());
		}

		private void WriteVariable(VariableDescription var)
		{
			if(var.InitialValue != null)
				WriteLine("{0} {1} = {2};", var.Type.Name, var.Name, var.InitialValue.GetString());
			else
				WriteLine("{0} {1};", var.Type.Name, var.Name);
		}
	}
}
