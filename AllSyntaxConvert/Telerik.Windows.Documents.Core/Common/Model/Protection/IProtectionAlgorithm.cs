using System;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	interface IProtectionAlgorithm
	{
		byte[] ComputeHash(byte[] buffer);
	}
}
