using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class FunctionInfo
	{
		public FunctionCategory Category
		{
			get
			{
				return this.category;
			}
		}

		public string Description
		{
			get
			{
				return this.description;
			}
		}

		public string DescriptionLocalizationKey
		{
			get
			{
				return this.descriptionLocalizationKey;
			}
		}

		public int RequiredArgumentsCount
		{
			get
			{
				return this.requiredArgumentsInfos.Count<ArgumentInfo>();
			}
		}

		public int OptionalArgumentsCount
		{
			get
			{
				return this.optionalArgumentsInfos.Count<ArgumentInfo>();
			}
		}

		public int OptionalArgumentsRepetitionCount
		{
			get
			{
				return this.optionalArgumentsRepetitionCount;
			}
		}

		public bool IsDefaultValueFunction
		{
			get
			{
				return this.isDefaultValueFunction;
			}
		}

		public CellValueFormat Format
		{
			get
			{
				return this.format;
			}
		}

		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		internal int TotalArgumentsCount
		{
			get
			{
				return this.RequiredArgumentsCount + this.OptionalArgumentsCount * this.OptionalArgumentsRepetitionCount;
			}
		}

		internal List<ArgumentInfo> RequiredArguments
		{
			get
			{
				return this.requiredArgumentsInfos;
			}
		}

		internal List<ArgumentInfo> OptionalArguments
		{
			get
			{
				return this.optionalArgumentsInfos;
			}
		}

		public FunctionInfo(string name, FunctionCategory category, string description, string descriptionLocalizationKey = null)
			: this(name, category, description, Enumerable.Empty<ArgumentInfo>(), Enumerable.Empty<ArgumentInfo>(), CellValueFormat.GeneralFormat, 1, false, descriptionLocalizationKey)
		{
		}

		public FunctionInfo(string name, FunctionCategory category, string description, CellValueFormat format, string descriptionLocalizationKey = null)
			: this(name, category, description, Enumerable.Empty<ArgumentInfo>(), Enumerable.Empty<ArgumentInfo>(), format, 1, false, descriptionLocalizationKey)
		{
		}

		public FunctionInfo(string name, FunctionCategory category, string description, IEnumerable<ArgumentInfo> requiredArgumentsInfos, bool isDefaultValueFunction = false, string descriptionLocalizationKey = null)
			: this(name, category, description, requiredArgumentsInfos, Enumerable.Empty<ArgumentInfo>(), CellValueFormat.GeneralFormat, 1, isDefaultValueFunction, descriptionLocalizationKey)
		{
		}

		public FunctionInfo(string name, FunctionCategory category, string description, IEnumerable<ArgumentInfo> requiredArgumentsInfos, CellValueFormat format, bool isDefaultValueFunction = false, string descriptionLocalizationKey = null)
			: this(name, category, description, requiredArgumentsInfos, Enumerable.Empty<ArgumentInfo>(), format, 1, isDefaultValueFunction, descriptionLocalizationKey)
		{
		}

		public FunctionInfo(string name, FunctionCategory category, string description, IEnumerable<ArgumentInfo> requiredArgumentsInfos, IEnumerable<ArgumentInfo> optionalArgumentsInfos, int optionalArgumentsRepeatCount = 1, bool isDefaultValueFunction = false, string descriptionLocalizationKey = null)
			: this(name, category, description, requiredArgumentsInfos, optionalArgumentsInfos, CellValueFormat.GeneralFormat, optionalArgumentsRepeatCount, isDefaultValueFunction, descriptionLocalizationKey)
		{
		}

		public FunctionInfo(string name, FunctionCategory category, string description, IEnumerable<ArgumentInfo> requiredArgumentsInfos, IEnumerable<ArgumentInfo> optionalArgumentsInfos, CellValueFormat format, int optionalArgumentsRepeatCount = 1, bool isDefaultValueFunction = false, string descriptionLocalizationKey = null)
		{
			this.name = name;
			this.category = category;
			this.description = description;
			this.optionalArgumentsRepetitionCount = optionalArgumentsRepeatCount;
			this.descriptionLocalizationKey = descriptionLocalizationKey;
			this.requiredArgumentsInfos = new List<ArgumentInfo>(requiredArgumentsInfos);
			this.optionalArgumentsInfos = new List<ArgumentInfo>(optionalArgumentsInfos);
			this.format = format;
			this.isDefaultValueFunction = isDefaultValueFunction;
		}

		internal IEnumerable<ArgumentInfo> GetAllArguments()
		{
			List<ArgumentInfo> list = new List<ArgumentInfo>();
			list.AddRange(this.requiredArgumentsInfos);
			for (int i = 0; i < this.OptionalArgumentsRepetitionCount; i++)
			{
				list.AddRange(this.optionalArgumentsInfos);
			}
			return list;
		}

		internal IEnumerable<ArgumentInfo> GetArguments(int currentArgumentIndex)
		{
			List<ArgumentInfo> list = new List<ArgumentInfo>();
			list.AddRange(this.requiredArgumentsInfos);
			int num = 1;
			list.AddRange(this.GetOptionalArguments(num));
			if (list.Count == 1)
			{
				num++;
				list.AddRange(this.GetOptionalArguments(num));
			}
			num++;
			while (currentArgumentIndex >= list.Count && this.optionalArgumentsInfos.Count<ArgumentInfo>() > 0)
			{
				list.AddRange(this.GetOptionalArguments(num));
				num++;
			}
			return list;
		}

		List<ArgumentInfo> GetOptionalArguments(int count)
		{
			List<ArgumentInfo> list = new List<ArgumentInfo>();
			if (this.optionalArgumentsInfos.Count<ArgumentInfo>() == 0)
			{
				return list;
			}
			foreach (ArgumentInfo argumentInfo in this.optionalArgumentsInfos)
			{
				if (count == 1)
				{
					list.Add(new ArgumentInfo(argumentInfo.Name + count, argumentInfo.Description, argumentInfo.Type, true, null, null));
				}
				else
				{
					list.Add(new ArgumentInfo(argumentInfo.Name + count, argumentInfo.Description, argumentInfo.Type, false, null, null));
				}
			}
			return list;
		}

		readonly string name;

		readonly FunctionCategory category;

		readonly string description;

		readonly string descriptionLocalizationKey;

		readonly bool isDefaultValueFunction;

		readonly List<ArgumentInfo> requiredArgumentsInfos;

		readonly List<ArgumentInfo> optionalArgumentsInfos;

		readonly int optionalArgumentsRepetitionCount;

		readonly CellValueFormat format;
	}
}
