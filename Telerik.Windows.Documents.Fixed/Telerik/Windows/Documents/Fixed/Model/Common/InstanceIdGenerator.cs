using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Common
{
	static class InstanceIdGenerator
	{
		public static int GetNextId()
		{
			return InstanceIdGenerator.idGenerator.GetNext();
		}

		static readonly IdGenerator idGenerator = new IdGenerator();
	}
}
