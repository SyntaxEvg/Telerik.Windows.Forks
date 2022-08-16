using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CsQuery.Utility
{
	static class Types
	{
		public static bool IsAnonymousType(Type type)
		{
			if (Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) && type.IsGenericType && type.Name.Contains("AnonymousType") && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")))
			{
				TypeAttributes attributes = type.Attributes;
				return 0 == 0;
			}
			return false;
		}
	}
}
