using System;

namespace Telerik.Windows.Zip
{
	[Flags]
	enum DeflatingFlags : ushort
	{
		Normal = 0,
		Maximum = 2,
		Fast = 4,
		SuperFast = 6
	}
}
