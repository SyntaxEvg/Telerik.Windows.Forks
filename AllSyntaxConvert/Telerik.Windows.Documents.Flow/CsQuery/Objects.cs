using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Text;
using CsQuery.ExtensionMethods.Internal;
using CsQuery.Implementation;
using CsQuery.Utility;

namespace CsQuery
{
	static class Objects
	{
		static Objects()
		{
			MemberInfo[] members = typeof(object).GetMembers();
			foreach (MemberInfo memberInfo in members)
			{
				Objects.IgnorePropertyNames.Add(memberInfo.Name);
			}
		}

		public static bool IsNullableType(Type type)
		{
			return type == typeof(string) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
		}

		public static bool IsJson(object obj)
		{
			string text = obj as string;
			return text != null && text.StartsWith("{") && !text.StartsWith("{{");
		}

		public static bool IsImmutable(object obj)
		{
			return obj == null || obj == DBNull.Value || obj is string || (obj is ValueType && !Objects.IsKeyValuePair(obj));
		}

		public static bool IsExtendableType(object obj)
		{
			return Objects.IsExpando(obj) || (!Objects.IsImmutable(obj) && !(obj is IEnumerable));
		}

		public static bool IsTruthy(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is string)
			{
				return !string.IsNullOrEmpty((string)obj);
			}
			if (obj is bool)
			{
				return (bool)obj;
			}
			return !Objects.IsNumericType(obj.GetType()) || System.Convert.ToDouble(obj) != 0.0;
		}

		public static bool IsNumericType(Type type)
		{
			Type underlyingType = Objects.GetUnderlyingType(type);
			return underlyingType.IsPrimitive && (!(underlyingType == typeof(string)) && !(underlyingType == typeof(char))) && !(underlyingType == typeof(bool));
		}

		public static bool IsNativeType(Type type)
		{
			Type underlyingType = Objects.GetUnderlyingType(type);
			return underlyingType.IsEnum || underlyingType.IsValueType || underlyingType.IsPrimitive || underlyingType == typeof(string);
		}

		public static string Join(Array array)
		{
			return Objects.Join(Objects.toStringList(array), ",");
		}

		public static string Join(IEnumerable list)
		{
			return Objects.Join(Objects.toStringList(list), ",");
		}

		public static bool IsExpando(object obj)
		{
			return obj is IDictionary<string, object>;
		}

		public static bool IsEmptyExpando(object obj)
		{
			return Objects.IsExpando(obj) && ((IDictionary<string, object>)obj).Count == 0;
		}

		public static bool IsKeyValuePair(object obj)
		{
			Type type = obj.GetType();
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(KeyValuePair<, >))
				{
					return true;
				}
			}
			return false;
		}

		public static IConvertible Coerce(object value)
		{
			if (value == null)
			{
				return null;
			}
			Type underlyingType = Objects.GetUnderlyingType(value.GetType());
			if (underlyingType == typeof(bool) || underlyingType == typeof(DateTime) || underlyingType == typeof(double))
			{
				return (IConvertible)value;
			}
			if (Objects.IsNumericType(value.GetType()))
			{
				return Support.NumberToDoubleOrInt((IConvertible)value);
			}
			string text = value.ToString();
			if (text == "false")
			{
				return false;
			}
			if (text == "true")
			{
				return true;
			}
			if (text == "undefined" || text == "null")
			{
				return null;
			}
			double num;
			if (double.TryParse(text, out num))
			{
				return Support.NumberToDoubleOrInt(num);
			}
			DateTime dateTime;
			if (DateTime.TryParse(text, out dateTime))
			{
				return dateTime;
			}
			return text;
		}

		public static T Convert<T>(object value)
		{
			T result;
			if (!Objects.TryConvert<T>(value, out result))
			{
				throw new InvalidCastException("Unable to convert to type " + typeof(T).ToString());
			}
			return result;
		}

		public static object Convert(object value, Type type)
		{
			object result;
			if (!Objects.TryConvert(value, out result, type, Objects.DefaultValue(type)))
			{
				throw new InvalidCastException("Unable to convert to type " + type.ToString());
			}
			return result;
		}

		public static T Convert<T>(object value, T defaultValue)
		{
			T result;
			if (!Objects.TryConvert<T>(value, out result))
			{
				result = defaultValue;
			}
			return result;
		}

		public static bool TryConvert<T>(object value, out T typedValue)
		{
			object obj;
			if (Objects.TryConvert(value, out obj, typeof(T), null))
			{
				typedValue = (T)((object)obj);
				return true;
			}
			typedValue = (T)((object)Objects.DefaultValue(typeof(T)));
			return false;
		}

		public static bool TryConvert(object value, out object typedValue, Type type, object defaultValue = null)
		{
			typedValue = null;
			object obj = defaultValue;
			bool flag = false;
			string text = ((value == null) ? string.Empty : value.ToString().ToLower().Trim());
			if (type == typeof(string))
			{
				typedValue = ((value == null) ? null : value.ToString());
				return true;
			}
			Type type2;
			if (Objects.IsNullableType(type))
			{
				if (text == string.Empty)
				{
					typedValue = null;
					return true;
				}
				type2 = Objects.GetUnderlyingType(type);
			}
			else
			{
				if (text == string.Empty)
				{
					typedValue = Objects.DefaultValue(type);
					return false;
				}
				type2 = type;
			}
			if (type2 == value.GetType())
			{
				obj = value;
				flag = true;
			}
			else if (type2 == typeof(bool))
			{
				bool flag2;
				flag = Objects.TryStringToBool(text, out flag2);
				if (flag)
				{
					obj = flag2;
				}
			}
			else if (type2.IsEnum)
			{
				obj = Enum.Parse(type2, text);
				flag = true;
			}
			else if (type2 == typeof(int) || type2 == typeof(long) || type2 == typeof(float) || type2 == typeof(double) || type2 == typeof(decimal))
			{
				object obj2;
				if (Objects.TryParseNumber(text, out obj2, type2))
				{
					obj = obj2;
					flag = true;
				}
			}
			else if (type2 == typeof(DateTime))
			{
				DateTime dateTime;
				if (DateTime.TryParse(text, out dateTime))
				{
					obj = dateTime;
					flag = true;
				}
			}
			else
			{
				obj = value;
			}
			if (obj != null && obj.GetType() != type2)
			{
				if (type2 is IConvertible)
				{
					try
					{
						typedValue = System.Convert.ChangeType(obj, type2);
						flag = true;
					}
					catch
					{
						typedValue = obj ?? Objects.DefaultValue(type2);
					}
				}
				if (!flag)
				{
					typedValue = obj ?? Objects.DefaultValue(type2);
				}
			}
			else
			{
				typedValue = obj ?? Objects.DefaultValue(type2);
			}
			return flag;
		}

		public static object ChangeType(object value, Type conversionType)
		{
			if (conversionType == null)
			{
				throw new ArgumentNullException("conversionType");
			}
			if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				if (value == null)
				{
					return null;
				}
				NullableConverter nullableConverter = new NullableConverter(conversionType);
				conversionType = nullableConverter.UnderlyingType;
			}
			return System.Convert.ChangeType(value, conversionType);
		}

		public static bool TryParseNumber(string value, out object number, Type T)
		{
			number = 0;
			double num;
			if (double.TryParse(value, out num))
			{
				if (T == typeof(int))
				{
					number = System.Convert.ToInt32(Math.Round(num));
				}
				else if (T == typeof(long))
				{
					number = System.Convert.ToInt64(Math.Round(num));
				}
				else if (T == typeof(double))
				{
					number = System.Convert.ToDouble(num);
				}
				else if (T == typeof(decimal))
				{
					number = System.Convert.ToDecimal(num);
				}
				else
				{
					if (!(T == typeof(float)))
					{
						throw new InvalidCastException("Unhandled type for TryParseNumber: " + T.GetType().ToString());
					}
					number = System.Convert.ToSingle(num);
				}
				return true;
			}
			return false;
		}

		public static IEnumerable<T> EnumerateProperties<T>(object obj)
		{
			return Objects.EnumerateProperties<T>(obj, new Type[0]);
		}

		public static IEnumerable<T> EnumerateProperties<T>(object obj, IEnumerable<Type> ignoreAttributes)
		{
			HashSet<Type> IgnoreList = new HashSet<Type>();
			if (ignoreAttributes != null)
			{
				IgnoreList.AddRange(ignoreAttributes);
			}
			IDictionary<string, object> source;
			if (obj is IDictionary<string, object>)
			{
				source = (IDictionary<string, object>)obj;
			}
			else
			{
				source = Objects.ToExpando<JsObject>(obj, false, ignoreAttributes);
			}
			foreach (KeyValuePair<string, object> kvp in source)
			{
				if (typeof(T) == typeof(KeyValuePair<string, object>))
				{
					KeyValuePair<string, object> keyValuePair = kvp;
					string key = keyValuePair.Key;
					KeyValuePair<string, object> keyValuePair2 = kvp;
					object value;
					if (!(keyValuePair2.Value is IDictionary<string, object>))
					{
						KeyValuePair<string, object> keyValuePair3 = kvp;
						value = keyValuePair3.Value;
					}
					else
					{
						KeyValuePair<string, object> keyValuePair4 = kvp;
						value = Objects.ToExpando((IDictionary<string, object>)keyValuePair4.Value);
					}
					yield return (T)((object)new KeyValuePair<string, object>(key, value));
				}
				else
				{
					KeyValuePair<string, object> keyValuePair5 = kvp;
					yield return Objects.Convert<T>(keyValuePair5.Value);
				}
			}
			yield break;
		}

		public static object DefaultValue(Type type)
		{
			if (!type.IsValueType)
			{
				return null;
			}
			return Objects.CreateInstance(type);
		}

		public static object CreateInstance(Type type)
		{
			return FastActivator.CreateInstance(type);
		}

		public static T CreateInstance<T>() where T : class
		{
			return FastActivator.CreateInstance<T>();
		}

		public static IEnumerable<T> Enumerate<T>(T obj)
		{
			if (obj != null)
			{
				yield return obj;
			}
			yield break;
		}

		public static IEnumerable<T> Enumerate<T>(params T[] obj)
		{
			return obj;
		}

		public static IEnumerable Enumerate(params object[] obj)
		{
			return obj;
		}

		public static IEnumerable<T> EmptyEnumerable<T>()
		{
			yield break;
		}

		public static T Dict2Dynamic<T>(IDictionary<string, object> obj) where T : IDynamicMetaObjectProvider, new()
		{
			return Objects.Dict2Dynamic<T>(obj, false);
		}

		public static string Join(IEnumerable<string> list, string separator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in list)
			{
				stringBuilder.Append((stringBuilder.Length == 0) ? text : (separator + text));
			}
			return stringBuilder.ToString();
		}

		static IEnumerable<string> toStringList(IEnumerable source)
		{
			foreach (object item in source)
			{
				yield return item.ToString();
			}
			yield break;
		}

		static object ParseValue(object value)
		{
			object result;
			if (value != null && value.GetType().IsAssignableFrom(typeof(DateTime)))
			{
				result = DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc).ToLocalTime();
			}
			else
			{
				result = value;
			}
			return result;
		}

		static object ConvertDeserializedValue<T>(object value, bool convertDates) where T : IDynamicMetaObjectProvider, new()
		{
			if (value is IDictionary<string, object>)
			{
				return Objects.Dict2Dynamic<T>((IDictionary<string, object>)value);
			}
			if (value is IEnumerable && !(value is string))
			{
				IList<object> list = new List<object>();
				Type type = null;
				bool flag = true;
				foreach (object obj in ((IEnumerable)value))
				{
					if (flag)
					{
						if (type == null)
						{
							type = obj.GetType();
						}
						else
						{
							flag = type == obj.GetType();
						}
					}
					list.Add(obj);
				}
				Array array;
				if (type != null)
				{
					if (typeof(IDictionary<string, object>).IsAssignableFrom(type))
					{
						array = Array.CreateInstance(Config.DynamicObjectType, list.Count);
					}
					else
					{
						array = Array.CreateInstance(type, list.Count);
					}
				}
				else
				{
					array = Array.CreateInstance(Config.DynamicObjectType, list.Count);
				}
				for (int i = 0; i < list.Count; i++)
				{
					array.SetValue(Objects.ConvertDeserializedValue<T>(list[i], true), i);
				}
				return array;
			}
			if (convertDates)
			{
				return Objects.ParseValue(value);
			}
			return value;
		}

		public static Type GetUnderlyingType(Type type)
		{
			if (type != typeof(string) && Objects.IsNullableType(type))
			{
				return Nullable.GetUnderlyingType(type);
			}
			return type;
		}

		public static T Dict2Dynamic<T>(IDictionary<string, object> obj, bool convertDates) where T : IDynamicMetaObjectProvider, new()
		{
			T t = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			if (obj != null)
			{
				IDictionary<string, object> dictionary = (IDictionary<string, object>)((object)t);
				foreach (KeyValuePair<string, object> keyValuePair in obj)
				{
					dictionary[keyValuePair.Key] = Objects.ConvertDeserializedValue<T>(keyValuePair.Value, convertDates);
				}
			}
			return t;
		}

		public static object Extend(bool deep, object target, params object[] inputObjects)
		{
			return Objects.ExtendImpl(null, deep, target, inputObjects);
		}

		static object ExtendImpl(HashSet<object> parents, bool deep, object target, params object[] inputObjects)
		{
			if (deep && parents == null)
			{
				parents = new HashSet<object>();
				parents.Add(target);
			}
			Queue<object> queue = new Queue<object>(inputObjects);
			Queue<object> queue2 = new Queue<object>();
			HashSet<object> hashSet = new HashSet<object>();
			while (queue.Count > 0)
			{
				object obj = queue.Dequeue();
				if (!Objects.IsExpando(obj) && Objects.IsExtendableType(obj) && obj is IEnumerable)
				{
					foreach (object item in ((IEnumerable)obj))
					{
						queue.Enqueue(item);
					}
				}
				if (!Objects.IsImmutable(obj) && hashSet.Add(obj))
				{
					queue2.Enqueue(obj);
				}
			}
			if (target == null)
			{
				target = FastActivator.CreateInstance(Config.DynamicObjectType);
			}
			else if (!Objects.IsExtendableType(target))
			{
				throw new InvalidCastException("Target type '" + target.GetType().ToString() + "' is not valid for CsQuery.Extend.");
			}
			while (queue2.Count > 0)
			{
				object obj2 = queue2.Dequeue();
				if (Objects.IsExpando(obj2))
				{
					using (IEnumerator<KeyValuePair<string, object>> enumerator2 = ((IDictionary<string, object>)obj2).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<string, object> keyValuePair = enumerator2.Current;
							Objects.AddExtendKVP(deep, parents, target, keyValuePair.Key, keyValuePair.Value);
						}
						continue;
					}
				}
				if (!Objects.IsExtendableType(obj2) && obj2 is IEnumerable)
				{
					var obj3 =obj2 as IEnumerable;

					foreach (var item in obj3)
                    {
						queue2.Enqueue(item);
					}
				}
				IEnumerable<MemberInfo> members = obj2.GetType().GetMembers();
				foreach (MemberInfo memberInfo in members)
				{
					if (!Objects.IgnorePropertyNames.Contains(memberInfo.Name))
					{
						object value;
						if (memberInfo is PropertyInfo)
						{
							PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
							if (!propertyInfo.CanRead || propertyInfo.GetIndexParameters().Length > 0)
							{
								continue;
							}
							value = ((PropertyInfo)memberInfo).GetGetMethod().Invoke(obj2, null);
						}
						else
						{
							if (!(memberInfo is FieldInfo))
							{
								continue;
							}
							FieldInfo fieldInfo = (FieldInfo)memberInfo;
							if (!fieldInfo.IsPublic || fieldInfo.IsStatic)
							{
								continue;
							}
							value = fieldInfo.GetValue(obj2);
						}
						Objects.AddExtendKVP(deep, parents, target, memberInfo.Name, value);
					}
				}
			}
			return target;
		}

		public static JsObject ToExpando(object source)
		{
			return Objects.ToExpando(source, false);
		}

		public static T ToExpando<T>(object source) where T : IDynamicMetaObjectProvider, IDictionary<string, object>, new()
		{
			return Objects.ToExpando<T>(source, false);
		}

		public static JsObject ToExpando(object source, bool deep)
		{
			return Objects.ToExpando<JsObject>(source, deep);
		}

		public static T ToExpando<T>(object source, bool deep) where T : IDictionary<string, object>, IDynamicMetaObjectProvider, new()
		{
			return Objects.ToExpando<T>(source, deep, new Type[0]);
		}

		public static T ToExpando<T>(object source, bool deep, IEnumerable<Type> ignoreAttributes) where T : IDictionary<string, object>, IDynamicMetaObjectProvider, new()
		{
			if (Objects.IsExpando(source) && !deep)
			{
				return Objects.Dict2Dynamic<T>((IDictionary<string, object>)source);
			}
			return Objects.ToNewExpando<T>(source, deep, ignoreAttributes);
		}

		public static object CloneObject(object obj)
		{
			return Objects.CloneObject(obj, false);
		}

		public static object CloneObject(object obj, bool deep)
		{
			if (Objects.IsImmutable(obj))
			{
				return obj;
			}
			if (obj.GetType().IsArray || Objects.IsExpando(obj))
			{
				return ((IEnumerable)obj).CloneList(deep);
			}
			return Objects.ToExpando(obj, true);
		}

		public static object DeleteProperty(object obj, string property)
		{
			if (Objects.IsImmutable(obj))
			{
				throw new ArgumentException("The object is a value type, it has no deletable properties.");
			}
			if (Objects.IsExpando(obj))
			{
				IDictionary<string, object> dictionary = (IDictionary<string, object>)obj;
				dictionary.Remove(property);
				return dictionary;
			}
			object obj2 = Objects.CloneObject(obj);
			return Objects.DeleteProperty(obj2, property);
		}

		static void AddExtendKVP(bool deep, HashSet<object> parents, object target, string name, object value)
		{
			IDictionary<string, object> dictionary = null;
			if (Objects.IsExpando(target))
			{
				dictionary = (IDictionary<string, object>)target;
			}
			if (deep)
			{
				if (Objects.IsExtendableType(value) && !parents.Add(value))
				{
					if (dictionary != null)
					{
						dictionary.Remove(name);
					}
					return;
				}
				object obj;
				if (Objects.IsExtendableType(value) && dictionary != null && dictionary.TryGetValue(name, out obj))
				{
					value = Objects.ExtendImpl(parents, true, null, new object[]
					{
						Objects.IsExtendableType(obj) ? obj : null,
						value
					});
				}
				else
				{
					value = Objects.CloneObject(value, true);
				}
			}
			if (dictionary != null)
			{
				dictionary[name] = value;
				return;
			}
			IEnumerable<MemberInfo> members = target.GetType().GetMembers();
			foreach (MemberInfo memberInfo in members)
			{
				if (memberInfo.Name.Equals(name, StringComparison.CurrentCulture))
				{
					if (memberInfo is PropertyInfo)
					{
						PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
						if (propertyInfo.CanWrite)
						{
							propertyInfo.GetSetMethod().Invoke(target, new object[] { value });
						}
					}
					else if (memberInfo is FieldInfo)
					{
						FieldInfo fieldInfo = (FieldInfo)memberInfo;
						if (!fieldInfo.IsStatic && fieldInfo.IsPublic && !fieldInfo.IsLiteral && !fieldInfo.IsInitOnly)
						{
							fieldInfo.SetValue(target, value);
						}
					}
				}
			}
		}

		static T ToNewExpando<T>(object source, bool deep, IEnumerable<Type> ignoreAttributes) where T : IDynamicMetaObjectProvider, IDictionary<string, object>, new()
		{
			if (source == null)
			{
				return default(T);
			}
			HashSet<Type> hashSet = new HashSet<Type>(ignoreAttributes);
			if (Objects.IsExpando(source))
			{
				return (T)((object)Objects.CloneObject(source, deep));
			}
			if (source is IDictionary)
			{
				T result = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
				IDictionary dictionary = (IDictionary)source;
				foreach (object obj in dictionary.Keys)
				{
					string key = obj.ToString();
					if (result.ContainsKey(key))
					{
						throw new InvalidCastException("The key '" + obj + "' could not be added because the same key already exists. Conversion of the source object's keys to strings did not result in unique keys.");
					}
					result.Add(key, dictionary[obj]);
				}
				return result;
			}
			if (!Objects.IsExtendableType(source))
			{
				throw new InvalidCastException("Conversion to ExpandObject must be from a JSON string, an object, or an ExpandoObject");
			}
			T t = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			IDictionary<string, object> dictionary2 = t;
			IEnumerable<MemberInfo> members = source.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
			using (IEnumerator<MemberInfo> enumerator2 = members.GetEnumerator())
			{
				IL_220:
				while (enumerator2.MoveNext())
				{
					MemberInfo memberInfo = enumerator2.Current;
					if (!Objects.IgnorePropertyNames.Contains(memberInfo.Name))
					{
						foreach (object obj2 in memberInfo.GetCustomAttributes(false))
						{
							Attribute attribute = (Attribute)obj2;
							if (hashSet.Contains(attribute.GetType()))
							{
								goto IL_220;
							}
						}
						string name = memberInfo.Name;
						object obj3 = null;
						bool flag = false;
						if (memberInfo is PropertyInfo)
						{
							PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
							if (propertyInfo.GetIndexParameters().Length != 0 || !propertyInfo.CanRead)
							{
								goto IL_204;
							}
							try
							{
								obj3 = ((PropertyInfo)memberInfo).GetGetMethod().Invoke(source, null);
								goto IL_204;
							}
							catch
							{
								flag = true;
								goto IL_204;
							}
						}
						if (!(memberInfo is FieldInfo))
						{
							continue;
						}
						obj3 = ((FieldInfo)memberInfo).GetValue(source);
						IL_204:
						if (!flag)
						{
							dictionary2[name] = (deep ? Objects.CloneObject(obj3, true) : obj3);
						}
					}
				}
			}
			return t;
		}

		static bool TryStringToBool(string value, out bool result)
		{
			switch (value)
			{
			case "on":
			case "yes":
			case "true":
			case "enabled":
			case "active":
			case "1":
				result = true;
				return true;
			case "off":
			case "no":
			case "false":
			case "disabled":
			case "0":
				result = false;
				return true;
			}
			result = false;
			return false;
		}

		public static IDomText CreateTextNode(string text)
		{
			return new DomText(text);
		}

		public static IDomComment CreateComment(string comment)
		{
			return new DomComment(comment);
		}

		public static IDomDocument CreateDocument()
		{
			return new DomDocument();
		}

		public static IDomCData CreateCData(string data)
		{
			return new DomCData();
		}

		public static IDomFragment CreateFragment()
		{
			return new DomFragment();
		}

		static HashSet<string> IgnorePropertyNames = new HashSet<string>();
	}
}
