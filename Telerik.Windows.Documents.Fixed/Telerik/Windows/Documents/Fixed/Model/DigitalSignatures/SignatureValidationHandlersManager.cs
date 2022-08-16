using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public static class SignatureValidationHandlersManager
	{
		static SignatureValidationHandlersManager()
		{
			SignatureValidationHandlersManager.RegisterHandler("adbe.x509.rsa_sha1", new Pkcs1());
			SignatureValidationHandlersManager.RegisterHandler("adbe.pkcs7.sha1", new Pkcs7());
			SignatureValidationHandlersManager.RegisterHandler("adbe.pkcs7.detached", new Pkcs7Detached());
		}

		public static void RegisterHandler(string subFilter, SignatureValidationHandlerBase validationHandler)
		{
			Guard.ThrowExceptionIfNullOrEmpty(subFilter, "subFilter");
			Guard.ThrowExceptionIfNull<SignatureValidationHandlerBase>(validationHandler, "validationHandler");
			SignatureValidationHandlersManager.store.Add(subFilter, validationHandler);
		}

		public static void UnregisterHandler(string subFilter)
		{
			Guard.ThrowExceptionIfNullOrEmpty(subFilter, "subFilter");
			if (SignatureValidationHandlersManager.HasRegisteredHandler(subFilter))
			{
				SignatureValidationHandlersManager.store.Remove(subFilter);
			}
		}

		public static bool HasRegisteredHandler(string subFilter)
		{
			Guard.ThrowExceptionIfNullOrEmpty(subFilter, "subFilter");
			return SignatureValidationHandlersManager.store.ContainsKey(subFilter);
		}

		internal static SignatureValidationHandlerBase GetHandler(string subFilter)
		{
			Guard.ThrowExceptionIfNullOrEmpty(subFilter, "subFilter");
			SignatureValidationHandlerBase result;
			if (SignatureValidationHandlersManager.store.TryGetValue(subFilter, out result))
			{
				return result;
			}
			return null;
		}

		static Dictionary<string, SignatureValidationHandlerBase> store = new Dictionary<string, SignatureValidationHandlerBase>();
	}
}
