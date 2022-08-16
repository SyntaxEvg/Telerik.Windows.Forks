using System;
using System.Collections.Generic;

namespace JBig2Decoder
{
	abstract class Flags
	{
		public int getFlagValue(string key)
		{
			return this.flags[key];
		}

		public abstract void setFlags(int flagsAsInt);

		protected int flagsAsInt;

		protected Dictionary<string, int> flags = new Dictionary<string, int>();
	}
}
