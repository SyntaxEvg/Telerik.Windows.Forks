using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using CsQuery.Implementation;
using CsQuery.Utility;

namespace CsQuery.HtmlParser
{
	static class HtmlStyles
	{
		static HtmlStyles()
		{
			XmlDocument xmlDocument = new XmlDocument();
			Stream resourceStream = Support.GetResourceStream("CsQuery.Resources." + HtmlStyles.CssDefs);
			xmlDocument.Load(resourceStream);
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			xmlNamespaceManager.AddNamespace("cssmd", "http://schemas.microsoft.com/Visual-Studio-Intellisense/css");
			XmlNodeList xmlNodeList = xmlDocument.DocumentElement.SelectNodes("cssmd:property-set/cssmd:property-def", xmlNamespaceManager);
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				CssStyle cssStyle = new CssStyle();
				cssStyle.Name = xmlNode.Attributes["_locID"].Value;
				string value = xmlNode.Attributes["type"].Value;
				string key;
				if ((key = value) != null)
				{
					if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6000e31-1 == null)
					{
						PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6000e31-1 = new Dictionary<string, int>(8)
						{
							{ "length", 0 },
							{ "color", 1 },
							{ "composite", 2 },
							{ "enum", 3 },
							{ "enum-length", 4 },
							{ "font", 5 },
							{ "string", 6 },
							{ "url", 7 }
						};
					}
					int num;
					if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6000e31-1.TryGetValue(key, out num))
					{
						switch (num)
						{
						case 0:
							cssStyle.Type = CSSStyleType.Unit;
							break;
						case 1:
							cssStyle.Type = CSSStyleType.Color;
							break;
						case 2:
							cssStyle.Type = CSSStyleType.Composite;
							cssStyle.Format = xmlNode.Attributes["syntax"].Value;
							break;
						case 3:
						case 4:
							if (value == "enum-length")
							{
								cssStyle.Type = CSSStyleType.UnitOption;
							}
							else
							{
								cssStyle.Type = CSSStyleType.Option;
							}
							cssStyle.Options = new HashSet<string>(xmlNode.Attributes["enum"].Value.Split(HtmlStyles.StringSep, StringSplitOptions.RemoveEmptyEntries));
							break;
						case 5:
							cssStyle.Type = CSSStyleType.Font;
							break;
						case 6:
							cssStyle.Type = CSSStyleType.String;
							break;
						case 7:
							cssStyle.Type = CSSStyleType.Url;
							break;
						default:
							goto IL_242;
						}
						cssStyle.Description = xmlNode.Attributes["description"].Value;
						HtmlStyles.StyleDefs[cssStyle.Name] = cssStyle;
						continue;
					}
				}
				IL_242:
				throw new NotImplementedException("Error parsing css xml: unknown type '" + value + "'");
			}
		}

		public static Dictionary<string, CssStyle> StyleDefs = new Dictionary<string, CssStyle>();

		static char[] StringSep = new char[] { ' ' };

		static string CssDefs = "css3.xml";
	}
}
