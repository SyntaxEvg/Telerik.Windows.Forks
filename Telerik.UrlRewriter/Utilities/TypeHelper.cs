using System;
using System.Reflection;

namespace Telerik.UrlRewriter.Utilities
{
	sealed class TypeHelper
	{
		TypeHelper()
		{
		}

		public static object Activate(string fullTypeName, object[] args)
		{
			string[] array = fullTypeName.Split(",".ToCharArray(), 2);
			if (array.Length != 2)
			{
				throw new ArgumentOutOfRangeException("fullTypeName", fullTypeName, MessageProvider.FormatString(Message.FullTypeNameRequiresAssemblyName, new object[0]));
			}
			return TypeHelper.Activate(array[1].Trim(), array[0].Trim(), args);
		}

		public static object Activate(string assemblyName, string typeName, object[] args)
		{
			if (assemblyName.Length == 0)
			{
				throw new ArgumentOutOfRangeException("assembly", assemblyName, MessageProvider.FormatString(Message.AssemblyNameRequired, new object[0]));
			}
			if (typeName.Length == 0)
			{
				throw new ArgumentOutOfRangeException("typeName", typeName, MessageProvider.FormatString(Message.TypeNameRequired, new object[0]));
			}
			return AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assemblyName, typeName, false, BindingFlags.Default, null, args, null, null, null);
		}
	}
}
