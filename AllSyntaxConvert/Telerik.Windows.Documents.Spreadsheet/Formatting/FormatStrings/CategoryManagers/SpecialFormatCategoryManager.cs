using System;
using System.Collections.Generic;
using System.Globalization;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.CategoryManagers
{
	public static class SpecialFormatCategoryManager
	{
		public static Dictionary<CultureInfo, IList<SpecialFormatInfo>> CultureInfoToFormatInfo
		{
			get
			{
				return SpecialFormatCategoryManager.cultureInfoToFormatInfo;
			}
		}

		static SpecialFormatCategoryManager()
		{
			SpecialFormatCategoryManager.InitCultureInfoToSpecialFormat();
		}

		static void InitCultureInfoToSpecialFormat()
		{
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("en-US", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Zip Code", "00000"),
				new SpecialFormatInfo("Zip Code +4", "00000-0000"),
				new SpecialFormatInfo("Phone Number", "[<=9999999]###-####;(###) ###-####"),
				new SpecialFormatInfo("Social Security Number", "000-00-0000")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("eu", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Post-kodea", "00000"),
				new SpecialFormatInfo("Posta-kodea + 4", "00000-0000"),
				new SpecialFormatInfo("Telefono-zenbakia", "[<=9999999]###-####;(###) ###-####"),
				new SpecialFormatInfo("Gizarte-segurantzako zenbakia", "000-00-0000"),
				new SpecialFormatInfo("Ehunekoa 0", "[$-42D]0"),
				new SpecialFormatInfo("Ehunekoa 0,00", "[$-42D]0.00")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("hr-HR", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Poštanski broj", "00000"),
				new SpecialFormatInfo("JMBG", "00000-0000"),
				new SpecialFormatInfo("Broj telefona", "[<=9999999]###-####;(###) ###-####"),
				new SpecialFormatInfo("Broj osiguranja", "000-00-0000")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("da", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Postnummer", "####"),
				new SpecialFormatInfo("Telefonnumer", "## ## ## ##"),
				new SpecialFormatInfo("Personnumer", "## ## ##-####")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("nl-NL", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Rekeningnummer", "00.00.00.000"),
				new SpecialFormatInfo("Telefoonnummer", "0#########"),
				new SpecialFormatInfo("Sofi-nummer", "####-##-###"),
				new SpecialFormatInfo("Gratis telefoonnummer", "0#-#######"),
				new SpecialFormatInfo("Plaats", "0##-#######"),
				new SpecialFormatInfo("Regionaal", "0###-######")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("en-CA", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Phone Number", "[<=9999999]###-####;###-###-####"),
				new SpecialFormatInfo("Social Insurance Number", "000 000 000")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("fi", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Postinumero", "00000"),
				new SpecialFormatInfo("Postinumero ulkomaille", "\"FIN-\"00000"),
				new SpecialFormatInfo("ISBN", "##-####-###-#"),
				new SpecialFormatInfo("Henkilotunnus", "######-####")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("fr-CA", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Numéro de sécurité sociale", "000 000 000"),
				new SpecialFormatInfo("Numéro de téléphone", "[<=9999999]###-####;###-###-####")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("fr-FR", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Code postal", "00000"),
				new SpecialFormatInfo("Numéro de sécurité sociale", "[>=3000000000000]#\" \"##\" \"##\" \"##\" \"###\" \"###\" | \"##;#\" \"##\" \"##\" \"##\" \"###\" \"###"),
				new SpecialFormatInfo("Numéro de téléphone", "0#\" \"##\" \"##\" \"##\" \"##"),
				new SpecialFormatInfo("Numéro de téléphone (Belgique)", "0##-000\" \"00\" \"00"),
				new SpecialFormatInfo("Numéro de téléphone (Canada)", "[>=10000000000]#-###-###-###;[>=10000000](###)\" \"###-####;000-0000"),
				new SpecialFormatInfo("Numéro de téléphone (Luxembourg)", "##\" \"00\" \"00"),
				new SpecialFormatInfo("Numéro de téléphone (Maroc)", "#\" \"00\" \"00\" \"00"),
				new SpecialFormatInfo("Numéro de téléphone (Suisse)", "0##\"/\"000\" \"00\" \"00")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("de-DE", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Postleitzahl", "00000"),
				new SpecialFormatInfo("Postleitzahl (A)", "\\A-00000"),
				new SpecialFormatInfo("Postleitzahl (CH)", "C\\H-00000"),
				new SpecialFormatInfo("Postleitzahl (D)", "\\D-00000"),
				new SpecialFormatInfo("Postleitzahl (L)", "L-00000"),
				new SpecialFormatInfo("Versicherungsnachweis-Nr. (D)", "\\[@\\]"),
				new SpecialFormatInfo("Sozialversicherungsnummer (A)", "0000-00 00 00"),
				new SpecialFormatInfo("Sozialversicherungsnummer (CH)", "000\\.00\\.000\\.000"),
				new SpecialFormatInfo("ISBN-Format (ISBN x-xxx-xxxxx-x)", "I\\S\\B\\N #-###-#####-#"),
				new SpecialFormatInfo("ISBN-Format (ISBN x-xxxx-xxxx-x)", "I\\S\\B\\N #-####-####-#"),
				new SpecialFormatInfo("ISBN-Format (ISBN x-xxxxx-xxx-x)", "I\\S\\B\\N #-#####-###-#")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("hu", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Irányítószám", "\"H-\"0000"),
				new SpecialFormatInfo("Adószám", "00000000-0-00"),
				new SpecialFormatInfo("Telefonszám", "[>=3620000000]# (##) ###-###;[>=20000000]# (##) ###-###;# (#) ###-##-##"),
				new SpecialFormatInfo("Társadalombiztosítási szám", "000000-0-00"),
				new SpecialFormatInfo("Bankkártya", "0000 0000 0000 0000"),
				new SpecialFormatInfo("Mobilszém", "[<=999999999](##) ###-##-##;[<=6999999999]0# (##)###-##-##;# (##) ###-##-##")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("it-IT", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("C.A.P.", "00000"),
				new SpecialFormatInfo("Codice fiscale", "############"),
				new SpecialFormatInfo("Numero telefonico", "[<=9999999]####-####;(0###) ####-####"),
				new SpecialFormatInfo("Numero Previdenza Sociale", "000-00-0000")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("nb", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Postnummer", "[<=9999]0000;General"),
				new SpecialFormatInfo("N-Postnummer", "[<=9999]\"N-\"0000;General"),
				new SpecialFormatInfo("Telefonnummer", "[<=99999999]##_ ##_ ##_ ##;(+##)_ ##_ ##_ ##_ ##"),
				new SpecialFormatInfo("Personnummer", "000000-00000")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("pl", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Kod pocztowy", "00-000"),
				new SpecialFormatInfo("Numer telefonu - 6 cyfr", "[<=999999]###-###;(###) ###-###"),
				new SpecialFormatInfo("Numer telefonu - 7 cyfr", "[<=9999999]###-##-##;(###) ###-##-##"),
				new SpecialFormatInfo("Numer PESEL", "00000000000"),
				new SpecialFormatInfo("Numer NIP", "000-000-00-00")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("pt-BR", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("CEP", "00000"),
				new SpecialFormatInfo("CEP + 3", "00000-000"),
				new SpecialFormatInfo("Telefone", "[<=9999999]###-####;(###) ###-####"),
				new SpecialFormatInfo("CIC", "000000000-00")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("ro", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Cod poștal", "00000"),
				new SpecialFormatInfo("Cod poștal + 4", "00000-0000"),
				new SpecialFormatInfo("Număr de telefon", "[<=9999999]###-####;(###) ###-####"),
				new SpecialFormatInfo("Cod numeric personal", "000-00-0000")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("ru", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Почтовый индекс", "000000"),
				new SpecialFormatInfo("Индекс + 4", "00000-0000"),
				new SpecialFormatInfo("Номер телефона", "[<=9999999]###-####;(###) ###-####"),
				new SpecialFormatInfo("Табельный номер", "0000")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("sk", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("PSč", "000 00"),
				new SpecialFormatInfo("PSč (bez nedzery)", "00000"),
				new SpecialFormatInfo("Telefônne číslo", "[<=99999]### ##;## ## ##"),
				new SpecialFormatInfo("Telefônne číslo (dlhé)", "[<=9999999]### ## ##;## ## ## ##")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("es-ES_tradnl", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Còdigo postal", "00000"),
				new SpecialFormatInfo("Còdigo postal + 4", "00000-0000"),
				new SpecialFormatInfo("Nùmero de telèfono", "[<=9999999]###-####;(###) ###-####"),
				new SpecialFormatInfo("Nùmero del seguro social", "000-00-0000")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("sv-SE", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Postnummer", "000 00"),
				new SpecialFormatInfo("S-Postnummer", "\"S-\"000 00"),
				new SpecialFormatInfo("ISBN", "##-####-###-#"),
				new SpecialFormatInfo("Personnummer", "######-####")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("tr", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Posta Kodu", "00000"),
				new SpecialFormatInfo("Telefon Numarası", "[<=9999999]###-####;(###) ###-####"),
				new SpecialFormatInfo("(Sosyal) Sigorta Numarası", "00000000"),
				new SpecialFormatInfo("Yüzde Oranı 0", "[$-41F]0"),
				new SpecialFormatInfo("Yüzde Oranı 0,00", "[$-41F]0.00")
			});
			SpecialFormatCategoryManager.AddCultureInfoToSpecialFormat("vi", new List<SpecialFormatInfo>
			{
				new SpecialFormatInfo("Metro Phone Number", "[<=9999999][$-1000000]###-####;[$-1000000](#) ###-####"),
				new SpecialFormatInfo("Suburb Phone Number", "[<=999999][$-1000000]###-###;[$-1000000](##) ###-###")
			});
		}

		static void AddCultureInfoToSpecialFormat(string cultureCode, IList<SpecialFormatInfo> info)
		{
			try
			{
				CultureInfo key = new CultureInfo(cultureCode);
				SpecialFormatCategoryManager.cultureInfoToFormatInfo.Add(key, info);
			}
			catch (CultureNotFoundException)
			{
			}
		}

		static readonly Dictionary<CultureInfo, IList<SpecialFormatInfo>> cultureInfoToFormatInfo = new Dictionary<CultureInfo, IList<SpecialFormatInfo>>();
	}
}
