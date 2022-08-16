using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model
{
	sealed class RtfGroup : RtfElement
	{
		public override RtfElementType Type
		{
			get
			{
				return RtfElementType.Group;
			}
		}

		public List<RtfElement> Elements
		{
			get
			{
				return this.elements;
			}
		}

		public bool IsExtensionDestination
		{
			get
			{
				if (this.elements.Count > 0)
				{
					RtfElement rtfElement = this.elements[0];
					if (rtfElement.Type == RtfElementType.Tag)
					{
						RtfTag rtfTag = (RtfTag)rtfElement;
						if ("*".Equals(rtfTag.Name))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public string Destination
		{
			get
			{
				if (this.elements.Count > 0)
				{
					RtfElement rtfElement = this.elements[0];
					if (rtfElement.Type == RtfElementType.Tag)
					{
						RtfTag rtfTag = (RtfTag)rtfElement;
						if ("*".Equals(rtfTag.Name) && this.elements.Count > 1)
						{
							RtfElement rtfElement2 = this.elements[1];
							if (rtfElement2.Type == RtfElementType.Tag)
							{
								RtfTag rtfTag2 = (RtfTag)rtfElement2;
								return rtfTag2.Name;
							}
						}
						return rtfTag.Name;
					}
				}
				return null;
			}
		}

		public RtfGroup SelectChildGroupWithDestination(string destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			foreach (RtfElement rtfElement in this.elements)
			{
				if (rtfElement.Type == RtfElementType.Group)
				{
					RtfGroup rtfGroup = (RtfGroup)rtfElement;
					if (destination.Equals(rtfGroup.Destination))
					{
						return rtfGroup;
					}
				}
			}
			return null;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("{");
			int count = this.elements.Count;
			stringBuilder.Append(count);
			stringBuilder.Append(" items");
			if (count > 0)
			{
				stringBuilder.Append(": [");
				stringBuilder.Append(this.elements[0]);
				if (count > 1)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(this.elements[1]);
					if (count > 2)
					{
						stringBuilder.Append(", ");
						if (count > 3)
						{
							stringBuilder.Append("..., ");
						}
						stringBuilder.Append(this.elements[count - 1]);
					}
				}
				stringBuilder.Append("]");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		protected override bool IsEqual(object obj)
		{
			RtfGroup rtfGroup = obj as RtfGroup;
			return rtfGroup != null && base.IsEqual(obj) && this.elements.Equals(rtfGroup.elements);
		}

		protected override int ComputeHashCode()
		{
			return HashTool.AddHashCode(base.ComputeHashCode(), this.elements);
		}

		readonly List<RtfElement> elements = new List<RtfElement>();
	}
}
