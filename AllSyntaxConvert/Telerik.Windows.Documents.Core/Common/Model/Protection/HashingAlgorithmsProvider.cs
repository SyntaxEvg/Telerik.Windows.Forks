using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	public static class HashingAlgorithmsProvider
	{
		public static void EnforceFips1402()
		{
			HashingAlgorithmsProvider.isFipsEnforced = true;
		}

		internal static IProtectionAlgorithm GetProtectionAlgorithm(string algorithmName)
		{
			HashingAlgorithmsProvider.Initialize();
			IProtectionAlgorithm result;
			if (!HashingAlgorithmsProvider.algorithms.TryGetValue(algorithmName, out result))
			{
				throw new InvalidOperationException("Hashing algorithm not supported");
			}
			return result;
		}

		static void Initialize()
		{
			if (HashingAlgorithmsProvider.algorithms.Count == 0)
			{
				if (!HashingAlgorithmsProvider.isFipsEnforced)
				{
					HashingAlgorithmsProvider.algorithms.Add("RIPEMD-160", new RIPEMD160());
					HashingAlgorithmsProvider.algorithms.Add("WHIRLPOOL", new Whirlpool());
				}
				HashingAlgorithmsProvider.algorithms.Add("SHA-1", new SHA1());
				HashingAlgorithmsProvider.algorithms.Add("SHA-256", new SHA256());
				HashingAlgorithmsProvider.algorithms.Add("SHA-384", new SHA384());
				HashingAlgorithmsProvider.algorithms.Add("SHA-512", new SHA512());
			}
		}

		static readonly Dictionary<string, IProtectionAlgorithm> algorithms = new Dictionary<string, IProtectionAlgorithm>();

		static bool isFipsEnforced;
	}
}
