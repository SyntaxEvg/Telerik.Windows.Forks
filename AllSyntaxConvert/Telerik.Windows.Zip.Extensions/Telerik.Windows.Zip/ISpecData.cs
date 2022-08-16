using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	interface ISpecData
	{
		bool TryReadBlock(BinaryReader reader);

		void WriteBlock(BinaryWriter writer);
	}
}
