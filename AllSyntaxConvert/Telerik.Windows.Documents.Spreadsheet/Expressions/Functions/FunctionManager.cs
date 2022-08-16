using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public static class FunctionManager
	{
		static FunctionManager()
		{
			foreach (Type type2 in from type in Assembly.GetExecutingAssembly().GetTypes()
				where !type.IsAbstract && type.IsSubclassOf(typeof(FunctionBase))
				select type)
			{
				FunctionManager.RegisterFunction((FunctionBase)Activator.CreateInstance(type2));
			}
		}

		public static void UnregisterFunction(FunctionBase function)
		{
			FunctionManager.UnregisterFunction(function.Name);
		}

		public static void UnregisterFunction(string functionName)
		{
			string key = functionName.ToUpper();
			if (FunctionManager.functionNameToFunction.ContainsKey(key))
			{
				FunctionManager.functionNameToFunction.Remove(key);
			}
		}

		public static void RegisterFunction(FunctionBase function)
		{
			FunctionManager.functionNameToFunction[function.Name.ToUpper()] = function;
		}

		public static FunctionBase GetFunctionByName(string functionName)
		{
			FunctionBase result;
			if (FunctionManager.functionNameToFunction.TryGetValue(functionName.ToUpper(), out result))
			{
				return result;
			}
			return null;
		}

		public static IEnumerable<FunctionBase> GetAllFunctions()
		{
			return from p in FunctionManager.functionNameToFunction.Values
				orderby p.Name
				select p;
		}

		static readonly Dictionary<string, FunctionBase> functionNameToFunction = new Dictionary<string, FunctionBase>();
	}
}
