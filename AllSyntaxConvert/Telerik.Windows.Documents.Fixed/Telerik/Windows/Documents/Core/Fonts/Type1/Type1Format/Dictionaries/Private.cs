using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Dictionaries
{
	class Private : PostScriptObject
	{
		public Private()
		{
			this.subrs = base.CreateProperty<PostScriptArray>(new PropertyDescriptor
			{
				Name = "Subrs"
			});
			this.otherSubrs = base.CreateProperty<PostScriptArray>(new PropertyDescriptor
			{
				Name = "OtherSubrs"
			});
			this.subroutines = new Dictionary<int, byte[]>();
		}

		public PostScriptArray Subrs
		{
			get
			{
				return this.subrs.GetValue();
			}
		}

		public PostScriptArray OtherSubrs
		{
			get
			{
				return this.otherSubrs.GetValue();
			}
		}

		public byte[] GetSubr(int index)
		{
			byte[] array;
			if (!this.subroutines.TryGetValue(index, out array))
			{
				array = this.Subrs.GetElementAs<PostScriptString>(index).ToByteArray();
				this.subroutines[index] = array;
			}
			return array;
		}

		readonly Property<PostScriptArray> subrs;

		readonly Property<PostScriptArray> otherSubrs;

		readonly Dictionary<int, byte[]> subroutines;
	}
}
