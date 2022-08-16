using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class PlotAreaElement : ChartElementBase
	{
		public PlotAreaElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.importedSeriesGroups = new List<SeriesGroup>();
			this.importedAxes = new List<Axis>();
		}

		public override string ElementName
		{
			get
			{
				return "plotArea";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public void CopyPropertiesFrom(DocumentChart chart)
		{
			this.chart = chart;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			foreach (SeriesGroup seriesGroup in this.chart.SeriesGroups)
			{
				switch (seriesGroup.SeriesType)
				{
				case SeriesType.Bar:
				{
					BarChartElement barChartElement = base.CreateElement<BarChartElement>("barChart");
					barChartElement.Chart = this.chart;
					barChartElement.SeriesGroup = seriesGroup as BarSeriesGroup;
					barChartElement.CopyPropertiesFrom();
					yield return barChartElement;
					break;
				}
				case SeriesType.Line:
				{
					LineChartElement lineChartElement = base.CreateElement<LineChartElement>("lineChart");
					lineChartElement.Chart = this.chart;
					lineChartElement.SeriesGroup = seriesGroup as LineSeriesGroup;
					lineChartElement.CopyPropertiesFrom();
					yield return lineChartElement;
					break;
				}
				case SeriesType.Pie:
				{
					DoughnutSeriesGroup doughnutGroup = seriesGroup as DoughnutSeriesGroup;
					PieSeriesGroup pieGroup = seriesGroup as PieSeriesGroup;
					if (doughnutGroup != null)
					{
						DoughnutChartElement doughnutChartElement = base.CreateElement<DoughnutChartElement>("doughnutChart");
						doughnutChartElement.Chart = this.chart;
						doughnutChartElement.SeriesGroup = doughnutGroup;
						doughnutChartElement.CopyPropertiesFrom();
						yield return doughnutChartElement;
					}
					else if (pieGroup != null)
					{
						PieChartElement pieChartElement = base.CreateElement<PieChartElement>("pieChart");
						pieChartElement.Chart = this.chart;
						pieChartElement.SeriesGroup = pieGroup;
						yield return pieChartElement;
					}
					break;
				}
				case SeriesType.Area:
				{
					AreaChartElement areaChartElement = base.CreateElement<AreaChartElement>("areaChart");
					areaChartElement.Chart = this.chart;
					areaChartElement.SeriesGroup = seriesGroup as AreaSeriesGroup;
					areaChartElement.CopyPropertiesFrom();
					yield return areaChartElement;
					break;
				}
				case SeriesType.Scatter:
				{
					ScatterChartElement scatterChartElement = base.CreateElement<ScatterChartElement>("scatterChart");
					scatterChartElement.Chart = this.chart;
					scatterChartElement.CopyPropertiesFrom(seriesGroup as ScatterSeriesGroup);
					yield return scatterChartElement;
					break;
				}
				case SeriesType.Bubble:
				{
					BubbleChartElement scatterChartElement2 = base.CreateElement<BubbleChartElement>("bubbleChart");
					scatterChartElement2.Chart = this.chart;
					scatterChartElement2.CopyPropertiesFrom(seriesGroup as BubbleSeriesGroup);
					yield return scatterChartElement2;
					break;
				}
				default:
					throw new NotSupportedException();
				}
			}
			if (this.chart.PrimaryAxes != null)
			{
				yield return this.CreateElementForAxis(context, this.chart.PrimaryAxes.CategoryAxis, 0, 1, AxisPosition.Bottom);
				yield return this.CreateElementForAxis(context, this.chart.PrimaryAxes.ValueAxis, 1, 0, AxisPosition.Left);
			}
			if (this.chart.SecondaryAxes != null)
			{
				yield return this.CreateElementForAxis(context, this.chart.SecondaryAxes.CategoryAxis, 2, 3, AxisPosition.Bottom);
				yield return this.CreateElementForAxis(context, this.chart.SecondaryAxes.ValueAxis, 3, 2, AxisPosition.Left);
			}
			yield break;
		}

		AxisElementBase CreateElementForAxis(IOpenXmlExportContext context, Axis axis, int axisIndex, int crossingAxisIndex, AxisPosition position)
		{
			switch (axis.AxisType)
			{
			case AxisType.Value:
			{
				ValueAxisElement valueAxisElement = base.CreateElement<ValueAxisElement>("valAx");
				valueAxisElement.CopyPropertiesFrom(context, axis, axisIndex, crossingAxisIndex, position);
				return valueAxisElement;
			}
			case AxisType.Category:
			{
				CategoryAxisElement categoryAxisElement = base.CreateElement<CategoryAxisElement>("catAx");
				categoryAxisElement.CopyPropertiesFrom(context, axis, axisIndex, crossingAxisIndex, position);
				return categoryAxisElement;
			}
			case AxisType.Date:
			{
				DateAxisElement dateAxisElement = base.CreateElement<DateAxisElement>("dateAx");
				dateAxisElement.CopyPropertiesFrom(context, axis, axisIndex, crossingAxisIndex, position);
				return dateAxisElement;
			}
			default:
				throw new NotSupportedException();
			}
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, DocumentChart chart)
		{
			context.PairSeriesGroupsWithAxes();
			foreach (SeriesGroup seriesGroup in this.importedSeriesGroups)
			{
				chart.SeriesGroups.Add(seriesGroup);
			}
			if (this.importedAxes.Count > 4 || this.importedAxes.Count % 2 != 0)
			{
				throw new InvalidOperationException("The number of axes is invalid.");
			}
			if (this.importedAxes.Count == 2)
			{
				chart.PrimaryAxes = new AxisGroup();
			}
			else if (this.importedAxes.Count == 4)
			{
				chart.PrimaryAxes = new AxisGroup();
				chart.SecondaryAxes = new AxisGroup();
			}
			int num = 0;
			foreach (Axis axis in this.importedAxes)
			{
				switch (num)
				{
				case 0:
					chart.PrimaryAxes.CategoryAxis = axis;
					break;
				case 1:
					chart.PrimaryAxes.ValueAxis = axis;
					break;
				case 2:
					chart.SecondaryAxes.CategoryAxis = axis;
					break;
				case 3:
					chart.SecondaryAxes.ValueAxis = axis;
					break;
				default:
					throw new InvalidOperationException("The number of axes is invalid.");
				}
				num++;
			}
		}

		protected override void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
			if (childElement is ChartGroupElementBase)
			{
				this.CreateSeriesGroup(context, childElement as ChartGroupElementBase);
				return;
			}
			if (childElement is AxisElementBase)
			{
				this.CreateAxis(context, childElement as AxisElementBase);
			}
		}

		void CreateSeriesGroup(IOpenXmlImportContext context, ChartGroupElementBase chartGroupElement)
		{
			string elementName;
			if ((elementName = chartGroupElement.ElementName) != null)
			{
				if (PImplD_F77F89940B024C6D8302951D4E56A706.method0x600027a1 == null)
				{
					PImplD_F77F89940B024C6D8302951D4E56A706.method0x600027a1 = new Dictionary<string, int>(7)
					{
						{ "barChart", 0 },
						{ "lineChart", 1 },
						{ "pieChart", 2 },
						{ "doughnutChart", 3 },
						{ "areaChart", 4 },
						{ "scatterChart", 5 },
						{ "bubbleChart", 6 }
					};
				}
				int num;
				if (PImplD_F77F89940B024C6D8302951D4E56A706.method0x600027a1.TryGetValue(elementName, out num))
				{
					SeriesGroup seriesGroup;
					switch (num)
					{
					case 0:
						seriesGroup = new BarSeriesGroup();
						break;
					case 1:
						seriesGroup = new LineSeriesGroup();
						break;
					case 2:
						seriesGroup = new PieSeriesGroup();
						break;
					case 3:
						seriesGroup = new DoughnutSeriesGroup();
						break;
					case 4:
						seriesGroup = new AreaSeriesGroup();
						break;
					case 5:
						seriesGroup = new ScatterSeriesGroup();
						break;
					case 6:
						seriesGroup = new BubbleSeriesGroup();
						break;
					default:
						goto IL_E6;
					}
					chartGroupElement.CopyPropertiesTo(context, seriesGroup);
					this.importedSeriesGroups.Add(seriesGroup);
					return;
				}
			}
			IL_E6:
			throw new NotSupportedException();
		}

		void CreateAxis(IOpenXmlImportContext context, AxisElementBase axisElement)
		{
			string elementName;
			if ((elementName = axisElement.ElementName) != null)
			{
				Axis axis;
				if (!(elementName == "catAx"))
				{
					if (!(elementName == "valAx"))
					{
						if (!(elementName == "dateAx"))
						{
							goto IL_4D;
						}
						axis = new DateAxis();
					}
					else
					{
						axis = new ValueAxis();
					}
				}
				else
				{
					axis = new CategoryAxis();
				}
				AxisGroupName groupName;
				switch (this.currentAxisCollectionIndex)
				{
				case 0:
					groupName = AxisGroupName.Primary;
					break;
				case 1:
					groupName = AxisGroupName.Primary;
					break;
				case 2:
					groupName = AxisGroupName.Secondary;
					break;
				case 3:
					groupName = AxisGroupName.Secondary;
					break;
				default:
					throw new InvalidOperationException("The number of axes is invalid.");
				}
				axisElement.CopyPropertiesTo(context, axis, groupName);
				this.importedAxes.Add(axis);
				this.currentAxisCollectionIndex++;
				return;
			}
			IL_4D:
			throw new NotSupportedException();
		}

		protected override void ClearOverride()
		{
			this.importedAxes.Clear();
			this.importedSeriesGroups.Clear();
			this.currentAxisCollectionIndex = 0;
			this.chart = null;
		}

		const int MaxNumberOfAxes = 4;

		public const int PrimaryCategoryAxisIndex = 0;

		public const int PrimaryValueAxisIndex = 1;

		public const int SecondaryCategoryAxisIndex = 2;

		public const int SecondaryValueAxisIndex = 3;

		readonly List<SeriesGroup> importedSeriesGroups;

		readonly List<Axis> importedAxes;

		DocumentChart chart;

		int currentAxisCollectionIndex;
	}
}
