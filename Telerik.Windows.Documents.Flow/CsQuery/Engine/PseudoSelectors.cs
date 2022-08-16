using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsQuery.Utility;

namespace CsQuery.Engine
{
	class PseudoSelectors
	{
		public PseudoSelectors()
		{
			if (PseudoSelectors.Items != null)
			{
				throw new Exception("You can only create one instance of the PseudoSelectors class.");
			}
			this.InnerSelectors = new ConcurrentDictionary<string, Type>();
			this.PopulateInnerSelectors();
		}

		public static PseudoSelectors Items { get; protected set; } = new PseudoSelectors();

		public IPseudoSelector GetInstance(string name)
		{
			IPseudoSelector result;
			if (this.TryGetInstance(name, out result))
			{
				return result;
			}
			throw new ArgumentException(string.Format("Attempt to use nonexistent pseudoselector :{0}", name));
		}

		public Type GetRegisteredType(string name)
		{
			Type result;
			if (this.TryGetRegisteredType(name, out result))
			{
				return result;
			}
			throw new KeyNotFoundException("The named pseudoclass filter is not registered.");
		}

		public bool TryGetRegisteredType(string name, out Type type)
		{
			return this.InnerSelectors.TryGetValue(name, out type);
		}

		public bool TryGetInstance(string name, out IPseudoSelector instance)
		{
			Type type;
			if (this.InnerSelectors.TryGetValue(name, out type))
			{
				instance = (IPseudoSelector)FastActivator.CreateInstance(type);
				return true;
			}
			instance = null;
			return false;
		}

		public void Register(string name, Type type)
		{
			this.ValidateType(type);
			this.InnerSelectors[name] = type;
		}

		public int Register(Assembly assembly = null)
		{
			Assembly firstExternalAssembly = Support.GetFirstExternalAssembly();
			return this.PopulateFromAssembly(firstExternalAssembly, new string[] { "CsQuery.Engine.PseudoClassSelectors", "CsQuery.Extensions" });
		}

		public bool Unregister(string name)
		{
			Type type;
			return this.InnerSelectors.TryRemove(name, out type);
		}

		void ValidateType(Type value)
		{
			if (value.GetInterface("IPseudoSelector") == null)
			{
				throw new ArgumentException("The type must implement IPseudoSelector.");
			}
		}

		void PopulateInnerSelectors()
		{
			string text = "CsQuery.Engine.PseudoClassSelectors";
			this.PopulateFromAssembly(Assembly.GetExecutingAssembly(), new string[] { text });
			if (this.InnerSelectors.Count == 0)
			{
				throw new InvalidOperationException(string.Format("I didn't find the native PseudoClassSelectors in the namespace {0}.", text));
			}
			if (Config.StartupOptions.HasFlag(StartupOptions.LookForExtensions))
			{
				this.Register(null);
			}
		}

		int PopulateFromAssembly(Assembly assy, params string[] nameSpaces)
		{
			int num = 0;
			foreach (Type type in assy.GetTypes())
			{
				if (type.IsClass && type.Namespace != null && !type.IsAbstract && nameSpaces.Contains(type.Namespace) && type.GetInterface("IPseudoSelector") != null)
				{
					IPseudoSelector pseudoSelector = (IPseudoSelector)FastActivator.CreateInstance(type);
					this.InnerSelectors[pseudoSelector.Name] = type;
					num++;
				}
			}
			return num;
		}

		ConcurrentDictionary<string, Type> InnerSelectors;
	}
}
