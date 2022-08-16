using System;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Visible : PseudoSelectorFilter
	{
		public override bool Matches(IDomObject element)
		{
			return Visible.IsVisible(element);
		}

		public static bool IsVisible(IDomObject element)
		{
			IDomObject domObject = ((element is IDomElement) ? element : element.ParentNode);
			while (domObject != null && domObject.NodeType == NodeType.ELEMENT_NODE)
			{
				if (Visible.ElementIsItselfHidden((IDomElement)domObject))
				{
					return false;
				}
				domObject = domObject.ParentNode;
			}
			return true;
		}

		static bool ElementIsItselfHidden(IDomElement el)
		{
			if (el.NodeNameID == 9 && el.Type == "hidden")
			{
				return true;
			}
			if (el.HasStyles)
			{
				if (!(el.Style["display"] == "none"))
				{
					double? num = el.Style.NumberPart("opacity");
					if (num.GetValueOrDefault() != 0.0 || num == null)
					{
						double? num2 = el.Style.NumberPart("width");
						double? num3 = el.Style.NumberPart("height");
						double? num4 = num2;
						if (num4.GetValueOrDefault() != 0.0 || num4 == null)
						{
							double? num5 = num3;
							if (num5.GetValueOrDefault() != 0.0 || num5 == null)
							{
								goto IL_E0;
							}
						}
						return true;
					}
				}
				return true;
			}
			IL_E0:
			string attribute = el.GetAttribute("width");
			string attribute2 = el.GetAttribute("height");
			return attribute == "0" || attribute2 == "0";
		}
	}
}
