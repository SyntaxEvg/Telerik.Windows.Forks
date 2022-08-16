using System;
using System.Collections.Concurrent;
using System.Reflection.Emit;

namespace CsQuery.Utility
{
	static class FastActivator
	{
		public static T CreateInstance<T>() where T : class
		{
			return (T)((object)FastActivator.CreateInstance(typeof(T)));
		}

		public static object CreateInstance(Type type)
		{
			if (!type.IsClass)
			{
				return Activator.CreateInstance(type);
			}
			FastActivator.CreateObject createObject;
			if (!FastActivator.creatorCache.TryGetValue(type, out createObject))
			{
				DynamicMethod dynamicMethod = new DynamicMethod("DM$OBJ_FACTORY_" + type.Name, type, null, type);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
				ilgenerator.Emit(OpCodes.Ret);
				createObject = (FastActivator.CreateObject)dynamicMethod.CreateDelegate(FastActivator.coType);
				FastActivator.creatorCache[type] = createObject;
			}
			return createObject();
		}

		static ConcurrentDictionary<Type, FastActivator.CreateObject> creatorCache = new ConcurrentDictionary<Type, FastActivator.CreateObject>();

		static readonly Type coType = typeof(FastActivator.CreateObject);

		delegate object CreateObject();
	}
}
