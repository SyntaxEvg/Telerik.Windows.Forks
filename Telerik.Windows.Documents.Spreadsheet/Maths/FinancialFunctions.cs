using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class FinancialFunctions
	{
		static double AccrInt(DateTime issue, DateTime firstInterestData, DateTime settlement, double rate, double par, double frequency, DayCountBasis basis, AccrIntCalcMethod calcMethod)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			int num = FinancialFunctions.DayCount.Freq2Months(frequency);
			int num2 = -num;
			bool flag = FinancialFunctions.Common.LastDayOfMonth(firstInterestData.Year, firstInterestData.Month, firstInterestData.Day);
			DateTime dateTime;
			if (settlement > firstInterestData && calcMethod == AccrIntCalcMethod.FromIssueToSettlement)
			{
				dateTime = FinancialFunctions.DayCount.FindPcdNcd(firstInterestData, settlement, num, basis, flag).Item1;
			}
			else
			{
				dateTime = dayCounter.ChangeMonth(firstInterestData, num2, flag);
			}
			DateTime from = ((issue > dateTime) ? issue : dateTime);
			double num3 = dayCounter.DaysBetween(from, settlement, NumDenumPosition.Numerator);
			FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
			DateTime settlement2 = dateTime;
			DateTime maturity = firstInterestData;
			double num4 = dayCounter2.CoupDays(settlement2, maturity, frequency);
			Func<DateTime, DateTime, double> f = FinancialFunctions.FAccrInt(issue, basis, calcMethod, dayCounter, frequency);
			Tuple<DateTime, DateTime, double> tuple = FinancialFunctions.DayCount.DatesAggregate1(dateTime, issue, num2, basis, f, num3 / num4, flag);
			double item = tuple.Item3;
			return par * rate / frequency * item;
		}

		static double AccrIntM(DateTime issue, DateTime settlement, double rate, double par, DayCountBasis basis)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			double num = dayCounter.DaysBetween(issue, settlement, NumDenumPosition.Numerator);
			double num2 = dayCounter.DaysInYear(issue, settlement);
			return par * rate * num / num2;
		}

		static double Disc(DateTime settlement, DateTime maturity, double pr, double redemption, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("investment must be more than 0", pr > 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption > 0.0);
			return FinancialFunctions.DiscInternal(settlement, maturity, pr, redemption, basis);
		}

		static double Duration(DateTime settlement, DateTime maturity, double coupon, double yld, Frequency frequency, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("coupon must be more than 0", coupon >= 0.0);
			FinancialFunctions.Common.Ensure("yld must be more than 0", yld >= 0.0);
			return FinancialFunctions.DurationInternal(settlement, maturity, coupon, yld, (double)frequency, basis, false);
		}

		static double IntRate(DateTime settlement, DateTime maturity, double investment, double redemption, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("investment must be more than 0", investment > 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption > 0.0);
			return FinancialFunctions.IntRateInternal(settlement, maturity, investment, redemption, basis);
		}

		static double MDuration(DateTime settlement, DateTime maturity, double coupon, double yld, Frequency frequency, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("coupon must be more than 0", coupon >= 0.0);
			FinancialFunctions.Common.Ensure("yld must be more than 0", yld >= 0.0);
			return FinancialFunctions.DurationInternal(settlement, maturity, coupon, yld, (double)frequency, basis, true);
		}

		static double Price(DateTime settlement, DateTime maturity, double rate, double yld, double redemption, Frequency frequency, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("rate must be more than 0", rate > 0.0);
			FinancialFunctions.Common.Ensure("yld must be more than 0", yld > 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption > 0.0);
			return FinancialFunctions.PriceInternal(settlement, maturity, rate, yld, redemption, (double)frequency, basis);
		}

		static double PriceDisc(DateTime settlement, DateTime maturity, double discount, double redemption, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("investment must be more than 0", discount > 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption > 0.0);
			return FinancialFunctions.PriceDiscInternal(settlement, maturity, discount, redemption, basis);
		}

		static double Received(DateTime settlement, DateTime maturity, double investment, double discount, DayCountBasis basis)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			double num = dayCounter.DaysBetween(settlement, maturity, NumDenumPosition.Numerator);
			double num2 = dayCounter.DaysInYear(settlement, maturity);
			double num3 = discount * num / num2;
			FinancialFunctions.Common.Ensure("discount * dim / b must be different from 1", num3 < 1.0);
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("investment must be more than 0", investment > 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", discount > 0.0);
			return FinancialFunctions.ReceivedInternal(settlement, maturity, investment, discount, basis);
		}

		static double Yield(DateTime settlement, DateTime maturity, double rate, double pr, double redemption, Frequency frequency, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("rate must be more than 0", rate > 0.0);
			FinancialFunctions.Common.Ensure("pr must be more than 0", pr > 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption > 0.0);
			double num = (double)frequency;
			Tuple<double, DateTime, double, double, double> priceYieldFactors = FinancialFunctions.GetPriceYieldFactors(settlement, maturity, num, basis);
			double item = priceYieldFactors.Item1;
			double item2 = priceYieldFactors.Item4;
			double item3 = priceYieldFactors.Item5;
			double item4 = priceYieldFactors.Item3;
			if (item > 1.0)
			{
				return FinancialFunctions.Common.FindRoot(FinancialFunctions.FYield(settlement, maturity, rate, pr, redemption, num, basis), 0.05);
			}
			double num2 = redemption / 100.0 + rate / num - (1.0 + item4 / item2 * rate / num);
			double num3 = 1.0 + item4 / item2 * rate / num;
			return num2 / num3 * num * item2 / item3;
		}

		static double YieldDisc(DateTime settlement, DateTime maturity, double pr, double redemption, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("investment must be more than 0", pr > 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption > 0.0);
			return FinancialFunctions.YieldDiscInternal(settlement, maturity, pr, redemption, basis);
		}

		static double YieldMat(DateTime settlement, DateTime maturity, DateTime issue, double rate, double pr, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("maturity must be after issue", maturity > issue);
			FinancialFunctions.Common.Ensure("settlement must be after issue", settlement > issue);
			FinancialFunctions.Common.Ensure("rate must be more than 0", rate > 0.0);
			FinancialFunctions.Common.Ensure("price must be more than 0", pr > 0.0);
			Tuple<double, double, double, double> matFactors = FinancialFunctions.GetMatFactors(settlement, maturity, issue, basis);
			double item = matFactors.Item4;
			double item2 = matFactors.Item2;
			double item3 = matFactors.Item1;
			double item4 = matFactors.Item3;
			double num = item2 / item3 * rate + 1.0 - pr / 100.0 - item4 / item3 * rate;
			double num2 = pr / 100.0 + item4 / item3 * rate;
			double num3 = item3 / item;
			return num / num2 * num3;
		}

		static double DiscInternal(DateTime settlement, DateTime maturity, double pr, double redemption, DayCountBasis basis)
		{
			Tuple<double, double> commonFactors = FinancialFunctions.GetCommonFactors(settlement, maturity, basis);
			double item = commonFactors.Item1;
			double item2 = commonFactors.Item2;
			return (-pr / redemption + 1.0) * item2 / item;
		}

		static double DurationInternal(DateTime settlement, DateTime maturity, double coupon, double yld, double frequency, DayCountBasis basis, bool isMDuration)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			double num = dayCounter.CoupDaysBS(settlement, maturity, frequency);
			double num2 = dayCounter.CoupDays(settlement, maturity, frequency);
			double num3 = dayCounter.CoupNum(settlement, maturity, frequency);
			double num4 = num2 - num;
			double num5 = num4 / num2;
			double num6 = num5 + num3 - 1.0;
			double num7 = yld / frequency + 1.0;
			double num8 = FinancialFunctions.Common.Pow(num7, num6);
			FinancialFunctions.Common.Ensure("(yld / frequency + 1)^((dsc / e) + n -1) must be different from 0)", Math.Abs(num8) > 1E-08);
			double num9 = num6 * 100.0 / num8;
			double num10 = 100.0 / num8;
			Func<Tuple<double, double>, int, Tuple<double, double>> collector = FinancialFunctions.FDuration(coupon, frequency, num5, num7);
			Tuple<double, double> tuple = FinancialFunctions.Common.AggrBetween<Tuple<double, double>>(1, (int)num3, collector, new Tuple<double, double>(0.0, 0.0));
			double item = tuple.Item2;
			double item2 = tuple.Item1;
			double num11 = num9 + item2;
			double num12 = num10 + item;
			FinancialFunctions.Common.Ensure("term6 must be different from 0)", Math.Abs(num12) > 1E-08);
			if (!isMDuration)
			{
				return num11 / num12 / frequency;
			}
			return num11 / num12 / frequency / num7;
		}

		static Tuple<double, double> GetCommonFactors(DateTime settlement, DateTime maturity, DayCountBasis basis)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
			double item = dayCounter2.DaysBetween(settlement, maturity, NumDenumPosition.Numerator);
			FinancialFunctions.DayCount.IDayCounter dayCounter3 = dayCounter;
			double item2 = dayCounter3.DaysInYear(settlement, maturity);
			return new Tuple<double, double>(item, item2);
		}

		static double IntRateInternal(DateTime settlement, DateTime maturity, double investment, double redemption, DayCountBasis basis)
		{
			Tuple<double, double> commonFactors = FinancialFunctions.GetCommonFactors(settlement, maturity, basis);
			double item = commonFactors.Item1;
			double item2 = commonFactors.Item2;
			return (redemption - investment) / investment * item2 / item;
		}

		static double PriceDiscInternal(DateTime settlement, DateTime maturity, double discount, double redemption, DayCountBasis basis)
		{
			Tuple<double, double> commonFactors = FinancialFunctions.GetCommonFactors(settlement, maturity, basis);
			double item = commonFactors.Item1;
			double item2 = commonFactors.Item2;
			return redemption - discount * redemption * item / item2;
		}

		static double ReceivedInternal(DateTime settlement, DateTime maturity, double investment, double discount, DayCountBasis basis)
		{
			Tuple<double, double> commonFactors = FinancialFunctions.GetCommonFactors(settlement, maturity, basis);
			double item = commonFactors.Item1;
			double item2 = commonFactors.Item2;
			double num = discount * item / item2;
			return investment / (1.0 - num);
		}

		static double YieldDiscInternal(DateTime settlement, DateTime maturity, double pr, double redemption, DayCountBasis basis)
		{
			Tuple<double, double> commonFactors = FinancialFunctions.GetCommonFactors(settlement, maturity, basis);
			double item = commonFactors.Item1;
			double item2 = commonFactors.Item2;
			return (redemption - pr) / pr * item2 / item;
		}

		static Tuple<double, double, double, double> GetMatFactors(DateTime settlement, DateTime maturity, DateTime issue, DayCountBasis basis)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			double item = dayCounter.DaysInYear(issue, settlement);
			double num = dayCounter.DaysBetween(issue, maturity, NumDenumPosition.Numerator);
			double num2 = dayCounter.DaysBetween(issue, settlement, NumDenumPosition.Numerator);
			double item2 = num - num2;
			return new Tuple<double, double, double, double>(item, num, num2, item2);
		}

		static Tuple<double, DateTime, double, double, double> GetPriceYieldFactors(DateTime settlement, DateTime maturity, double frequency, DayCountBasis basis)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			double item = dayCounter.CoupNum(settlement, maturity, frequency);
			DateTime dateTime = dayCounter.CoupPCD(settlement, maturity, frequency);
			double num = dayCounter.DaysBetween(dateTime, settlement, NumDenumPosition.Numerator);
			double num2 = dayCounter.CoupDays(settlement, maturity, frequency);
			double item2 = num2 - num;
			return new Tuple<double, DateTime, double, double, double>(item, dateTime, num, num2, item2);
		}

		static double PriceInternal(DateTime settlement, DateTime maturity, double rate, double yld, double redemption, double frequency, DayCountBasis basis)
		{
			Tuple<double, DateTime, double, double, double> priceYieldFactors = FinancialFunctions.GetPriceYieldFactors(settlement, maturity, frequency, basis);
			double item = priceYieldFactors.Item1;
			double item2 = priceYieldFactors.Item4;
			double item3 = priceYieldFactors.Item5;
			double item4 = priceYieldFactors.Item3;
			double num = 100.0 * rate / frequency;
			double num2 = 100.0 * rate / frequency * item4 / item2;
			Func<double, double> func = FinancialFunctions.FPrice(yld, frequency, item2, item3);
			double num3 = redemption / func(item);
			double num4 = 0.0;
			int num5 = 1;
			int num6 = (int)item;
			if (num6 >= num5)
			{
				do
				{
					num4 += num / func((double)num5);
					num5++;
				}
				while (num5 != num6 + 1);
			}
			if (item == 1.0)
			{
				return (redemption + num) / (1.0 + item3 / item2 * yld / frequency) - num2;
			}
			return num3 + num4 - num2;
		}

		static Func<double, double> FYield(DateTime settlement, DateTime maturity, double rate, double pr, double redemption, double frequency, DayCountBasis basis)
		{
			return (double yld) => FinancialFunctions.PriceInternal(settlement, maturity, rate, yld, redemption, frequency, basis) - pr;
		}

		static Func<DateTime, DateTime, double> FAccrInt(DateTime issue, DayCountBasis basis, AccrIntCalcMethod calcMethod, FinancialFunctions.DayCount.IDayCounter dc, double freq)
		{
			return delegate(DateTime pcd, DateTime ncd)
			{
				DateTime dateTime = ((issue > pcd) ? issue : pcd);
				double num;
				if (basis == DayCountBasis.UsPsa30_360)
				{
					FinancialFunctions.DayCount.Method360Us method = ((issue > pcd) ? FinancialFunctions.DayCount.Method360Us.ModifyStartDate : FinancialFunctions.DayCount.Method360Us.ModifyBothDates);
					num = (double)FinancialFunctions.DayCount.DateDiff360US(dateTime, ncd, method);
				}
				else
				{
					num = dc.DaysBetween(dateTime, ncd, NumDenumPosition.Numerator);
				}
				double num2;
				if (basis == DayCountBasis.UsPsa30_360)
				{
					num2 = (double)FinancialFunctions.DayCount.DateDiff360US(pcd, ncd, FinancialFunctions.DayCount.Method360Us.ModifyBothDates);
				}
				else if (basis == DayCountBasis.Actual365)
				{
					num2 = 365.0 / freq;
				}
				else
				{
					num2 = dc.DaysBetween(pcd, ncd, NumDenumPosition.Denumerator);
				}
				if (!(issue < pcd))
				{
					return num / num2;
				}
				return (double)calcMethod;
			};
		}

		static Func<Tuple<double, double>, int, Tuple<double, double>> FDuration(double coupon, double frequency, double x1, double x3)
		{
			return delegate(Tuple<double, double> acc, int index)
			{
				double num = (double)index - 1.0 + x1;
				double num2 = FinancialFunctions.Common.Pow(x3, num);
				FinancialFunctions.Common.Ensure("x6 must be different from 0)", Math.Abs(num2) > 1E-08);
				double num3 = 100.0 * coupon / frequency / num2;
				double item = acc.Item2;
				double item2 = acc.Item1;
				return new Tuple<double, double>(item2 + num3 * num, item + num3);
			};
		}

		static Func<double, double> FPrice(double yld, double frequency, double e, double dsc)
		{
			return (double k) => FinancialFunctions.Common.Pow(1.0 + yld / frequency, k - 1.0 + dsc / e);
		}

		static double DollarDe(double fractionalDollar, double fraction)
		{
			FinancialFunctions.Common.Ensure("fraction must be more than 0", fraction > 0.0);
			return FinancialFunctions.Dollar<double>(fractionalDollar, fraction, new Func<double, double, double, double, double>(FinancialFunctions.DollarDeInternal));
		}

		static double CoupNumber(DateTime d1, DateTime d2, int numMonths, DayCountBasis basis, bool isWholeNumber)
		{
			Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(d1);
			int item = tuple.Item1;
			int item2 = tuple.Item2;
			int item3 = tuple.Item3;
			Tuple<int, int, int> tuple2 = FinancialFunctions.Common.ToTuple(d2);
			int item4 = tuple2.Item1;
			int item5 = tuple2.Item2;
			int item6 = tuple2.Item3;
			double num = (double)((!isWholeNumber) ? 1 : 0);
			double num2 = num;
			bool flag = FinancialFunctions.Common.LastDayOfMonth(item, item2, item3);
			bool flag2 = !flag && item2 != 2;
			bool flag3 = flag2 && item3 > 28;
			bool flag4 = flag3 && item3 < FinancialFunctions.Common.DaysOfMonth(item, item2);
			bool flag5 = ((!flag4) ? flag : FinancialFunctions.Common.LastDayOfMonth(item4, item5, item6));
			bool flag6 = flag5;
			DateTime dateTime2 = FinancialFunctions.DayCount.ChangeMonth(d2, 0, basis, flag6);
			double num3 = ((!(d2 < dateTime2)) ? num2 : (num2 + 1.0));
			double acc = num3;
			DateTime startDate = FinancialFunctions.DayCount.ChangeMonth(dateTime2, numMonths, basis, flag6);
			Tuple<DateTime, DateTime, double> tuple3 = FinancialFunctions.DayCount.DatesAggregate1(startDate, d1, numMonths, basis, (DateTime time, DateTime dateTime) => 1.0, acc, flag6);
			return tuple3.Item3;
		}

		static double DaysBetweenNotNeg(FinancialFunctions.DayCount.IDayCounter dc, DateTime startDate, DateTime endDate)
		{
			double num = dc.DaysBetween(startDate, endDate, NumDenumPosition.Numerator);
			if (num > 0.0)
			{
				return num;
			}
			return 0.0;
		}

		static double DaysBetweenNotNegPsaHack(DateTime startDate, DateTime endDate)
		{
			double num = (double)FinancialFunctions.DayCount.DateDiff360US(startDate, endDate, FinancialFunctions.DayCount.Method360Us.ModifyBothDates);
			if (num > 0.0)
			{
				return num;
			}
			return 0.0;
		}

		static double DaysBetweenNotNegWithHack(FinancialFunctions.DayCount.IDayCounter dc, DateTime startDate, DateTime endDate, DayCountBasis basis)
		{
			if (basis == DayCountBasis.UsPsa30_360)
			{
				return FinancialFunctions.DaysBetweenNotNegPsaHack(startDate, endDate);
			}
			return FinancialFunctions.DaysBetweenNotNeg(dc, startDate, endDate);
		}

		static Func<Tuple<double, double>, int, Tuple<double, double>> FOddFPrice(DateTime settlement, DateTime issue, DayCountBasis basis, FinancialFunctions.DayCount.IDayCounter dc, int numMonthsNeg, double e, DateTime lateCoupon)
		{
			return delegate(Tuple<double, double> t, int index)
			{
				DateTime dateTime = FinancialFunctions.DayCount.ChangeMonth(lateCoupon, numMonthsNeg, basis, false);
				double num = ((basis != DayCountBasis.ActualActual) ? e : FinancialFunctions.DaysBetweenNotNeg(dc, dateTime, lateCoupon));
				double num2 = ((index <= 1) ? FinancialFunctions.DaysBetweenNotNeg(dc, issue, lateCoupon) : num);
				DateTime startDate = ((!(issue > dateTime)) ? dateTime : issue);
				DateTime endDate = ((!(settlement < lateCoupon)) ? lateCoupon : settlement);
				double num3 = FinancialFunctions.DaysBetweenNotNeg(dc, startDate, endDate);
				lateCoupon = dateTime;
				double item = t.Item1;
				double item2 = t.Item2;
				return Tuple.Create<double, double>(item + num2 / num, item2 + num3 / num);
			};
		}

		static Func<double, int, double> FOddFPriceNegative(double rate, double m, double y, double p1)
		{
			return (double acc, int index) => acc + 100.0 * rate / m / FinancialFunctions.Common.Pow(p1, (double)index - 1.0 + y);
		}

		static Func<double, int, double> FOddFPricePositive(double rate, double m, double nq, double y, double p1)
		{
			return (double acc, int index) => acc + 100.0 * rate / m / FinancialFunctions.Common.Pow(p1, (double)index + nq + y);
		}

		static Func<double, double> FOddFYield(DateTime settlement, DateTime maturity, DateTime issue, DateTime firstCoupon, double rate, double pr, double redemption, double frequency, DayCountBasis basis)
		{
			return (double yld) => pr - FinancialFunctions.OddFPrice(settlement, maturity, issue, firstCoupon, rate, yld, redemption, frequency, basis);
		}

		static Func<Tuple<double, double, double>, int, Tuple<double, double, double>> FOddLFunc(DateTime settlement, DateTime maturity, DayCountBasis basis, FinancialFunctions.DayCount.IDayCounter dc, int numMonths, double nc, DateTime earlyCoupon)
		{
			return delegate(Tuple<double, double, double> t, int index)
			{
				DateTime dateTime = FinancialFunctions.DayCount.ChangeMonth(earlyCoupon, numMonths, basis, false);
				double num = FinancialFunctions.DaysBetweenNotNegWithHack(dc, earlyCoupon, dateTime, basis);
				double num2 = ((index >= (int)nc) ? FinancialFunctions.DaysBetweenNotNegWithHack(dc, earlyCoupon, maturity, basis) : num);
				double num3 = num2;
				double num4;
				if (!(dateTime < settlement))
				{
					num4 = ((!(earlyCoupon < settlement)) ? 0.0 : FinancialFunctions.DaysBetweenNotNeg(dc, earlyCoupon, settlement));
				}
				else
				{
					num4 = num3;
				}
				DateTime dateTime2 = ((!(settlement > earlyCoupon)) ? earlyCoupon : settlement);
				DateTime startDate = dateTime2;
				DateTime dateTime3 = ((!(maturity < dateTime)) ? dateTime : maturity);
				DateTime endDate = dateTime3;
				double num5 = FinancialFunctions.DaysBetweenNotNeg(dc, startDate, endDate);
				earlyCoupon = dateTime;
				double item = t.Item3;
				double item2 = t.Item1;
				double item3 = t.Item2;
				return new Tuple<double, double, double>(item2 + num3 / num, item3 + num4 / num, item + num5 / num);
			};
		}

		static double OddFPrice(DateTime settlement, DateTime maturity, DateTime issue, DateTime firstCoupon, double rate, double yld, double redemption, double frequency, DayCountBasis basis)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			int num = FinancialFunctions.DayCount.Freq2Months(frequency);
			int numMonthsNeg = -num;
			double num2 = dayCounter.CoupDays(settlement, firstCoupon, frequency);
			double num3 = dayCounter.CoupNum(settlement, maturity, frequency);
			double num4 = FinancialFunctions.DaysBetweenNotNeg(dayCounter, issue, firstCoupon);
			if (num4 >= num2)
			{
				FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
				double num5 = dayCounter2.CoupNum(issue, firstCoupon, frequency);
				Func<Tuple<double, double>, int, Tuple<double, double>> @object = FinancialFunctions.FOddFPrice(settlement, issue, basis, dayCounter, numMonthsNeg, num2, firstCoupon);
				Tuple<double, double> tuple = FinancialFunctions.Common.AggrBetween<Tuple<double, double>>((int)num5, 1, new Func<Tuple<double, double>, int, Tuple<double, double>>(@object.Invoke), new Tuple<double, double>(0.0, 0.0));
				double item = tuple.Item1;
				double item2 = tuple.Item2;
				double num6;
				if (basis == DayCountBasis.Actual360 || basis == DayCountBasis.Actual365)
				{
					DateTime endDate = dayCounter.CoupNCD(settlement, firstCoupon, frequency);
					num6 = FinancialFunctions.DaysBetweenNotNeg(dayCounter, settlement, endDate);
				}
				else
				{
					DateTime from = dayCounter.CoupPCD(settlement, firstCoupon, frequency);
					double num7 = dayCounter.DaysBetween(from, settlement, NumDenumPosition.Numerator);
					num6 = num2 - num7;
				}
				double num8 = FinancialFunctions.CoupNumber(firstCoupon, settlement, num, basis, true);
				num3 = dayCounter.CoupNum(firstCoupon, maturity, frequency);
				double num9 = yld / frequency + 1.0;
				double num10 = num6 / num2;
				double num11 = redemption / FinancialFunctions.Common.Pow(num9, num10 + num8 + num3);
				double num12 = 100.0 * rate / frequency * item / FinancialFunctions.Common.Pow(num9, num8 + num10);
				double num13 = FinancialFunctions.Common.AggrBetween<double>(1, (int)num3, new Func<double, int, double>(FinancialFunctions.FOddFPricePositive(rate, frequency, num8, num10, num9).Invoke), 0.0);
				double num14 = 100.0 * rate / frequency * item2;
				return num11 + num12 + num13 - num14;
			}
			double num15 = FinancialFunctions.DaysBetweenNotNeg(dayCounter, settlement, firstCoupon);
			double num16 = FinancialFunctions.DaysBetweenNotNeg(dayCounter, issue, settlement);
			double num17 = yld / frequency + 1.0;
			double num18 = num15 / num2;
			double num19 = num17;
			double num20 = redemption / FinancialFunctions.Common.Pow(num19, num3 - 1.0 + num18);
			double num21 = 100.0 * rate / frequency * num4 / num2 / FinancialFunctions.Common.Pow(num19, num18);
			double num22 = FinancialFunctions.Common.AggrBetween<double>(2, (int)num3, new Func<double, int, double>(FinancialFunctions.FOddFPriceNegative(rate, frequency, num18, num19).Invoke), 0.0);
			double num23 = rate / frequency;
			double num24 = num16 / num2 * num23 * 100.0;
			return num20 + num21 + num22 - num24;
		}

		static double OddFYield(DateTime settlement, DateTime maturity, DateTime issue, DateTime firstCoupon, double rate, double pr, double redemption, double frequency, DayCountBasis basis)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
			double num = dayCounter2.DaysBetween(settlement, maturity, NumDenumPosition.Numerator);
			double num2 = pr - 100.0;
			double num3 = rate * num * 100.0 - num2;
			double num4 = num2 / 4.0 + num * num2 / 2.0 + num * 100.0;
			double guess = num3 / num4;
			return FinancialFunctions.Common.FindRoot(new Func<double, double>(FinancialFunctions.FOddFYield(settlement, maturity, issue, firstCoupon, rate, pr, redemption, frequency, basis).Invoke), guess);
		}

		static double OddLFunc(DateTime settlement, DateTime maturity, DateTime lastInterest, double rate, double prOrYld, double redemption, double frequency, DayCountBasis basis, bool isLPrice)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			int numMonths = (int)(12.0 / frequency);
			FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
			double num = dayCounter2.CoupNum(lastInterest, maturity, frequency);
			Func<Tuple<double, double, double>, int, Tuple<double, double, double>> @object = FinancialFunctions.FOddLFunc(settlement, maturity, basis, dayCounter, numMonths, num, lastInterest);
			Tuple<double, double, double> tuple = FinancialFunctions.Common.AggrBetween<Tuple<double, double, double>>(1, (int)num, new Func<Tuple<double, double, double>, int, Tuple<double, double, double>>(@object.Invoke), new Tuple<double, double, double>(0.0, 0.0, 0.0));
			double item = tuple.Item3;
			double item2 = tuple.Item1;
			double item3 = tuple.Item2;
			double num2 = 100.0 * rate / frequency;
			double num3 = item2 * num2 + redemption;
			if (!isLPrice)
			{
				double num4 = item3 * num2 + prOrYld;
				double num5 = frequency / item;
				return (num3 - num4) / num4 * num5;
			}
			double num6 = item * prOrYld / frequency + 1.0;
			double num7 = item3 * num2;
			return num3 / num6 - num7;
		}

		static double OddFPrice(DateTime settlement, DateTime maturity, DateTime issue, DateTime firstCoupon, double rate, double yld, double redemption, Frequency frequency, DayCountBasis basis)
		{
			Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(maturity);
			int item = tuple.Item1;
			int item2 = tuple.Item2;
			int item3 = tuple.Item3;
			bool flag = FinancialFunctions.Common.LastDayOfMonth(item, item2, item3);
			int num = (int)(12.0 / (double)frequency);
			int numMonths = -num;
			Tuple<DateTime, DateTime> tuple2 = FinancialFunctions.DayCount.FindPcdNcd(FinancialFunctions.DayCount.ChangeMonth(maturity, numMonths, basis, flag), firstCoupon, numMonths, basis, flag);
			DateTime item4 = tuple2.Item1;
			FinancialFunctions.Common.Ensure("maturity and firstCoupon must have the same month and day (except for February when leap years are considered)", item4 == firstCoupon);
			FinancialFunctions.Common.Ensure("maturity must be after firstCoupon", maturity > firstCoupon);
			FinancialFunctions.Common.Ensure("firstCoupon must be after settlement", firstCoupon > settlement);
			FinancialFunctions.Common.Ensure("settlement must be after issue", settlement > issue);
			FinancialFunctions.Common.Ensure("rate must be more than 0", rate >= 0.0);
			FinancialFunctions.Common.Ensure("yld must be more than 0", yld >= 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption >= 0.0);
			return FinancialFunctions.OddFPrice(settlement, maturity, issue, firstCoupon, rate, yld, redemption, (double)frequency, basis);
		}

		static double OddFYield(DateTime settlement, DateTime maturity, DateTime issue, DateTime firstCoupon, double rate, double pr, double redemption, Frequency frequency, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after firstCoupon", maturity > firstCoupon);
			FinancialFunctions.Common.Ensure("firstCoupon must be after settlement", firstCoupon > settlement);
			FinancialFunctions.Common.Ensure("settlement must be after issue", settlement > issue);
			FinancialFunctions.Common.Ensure("rate must be more than 0", rate >= 0.0);
			FinancialFunctions.Common.Ensure("pr must be more than 0", pr >= 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption >= 0.0);
			return FinancialFunctions.OddFYield(settlement, maturity, issue, firstCoupon, rate, pr, redemption, (double)frequency, basis);
		}

		static double OddLPrice(DateTime settlement, DateTime maturity, DateTime lastInterest, double rate, double yld, double redemption, Frequency frequency, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("settlement must be after lastInterest", settlement > lastInterest);
			FinancialFunctions.Common.Ensure("rate must be more than 0", rate >= 0.0);
			FinancialFunctions.Common.Ensure("yld must be more than 0", yld >= 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption >= 0.0);
			return FinancialFunctions.OddLFunc(settlement, maturity, lastInterest, rate, yld, redemption, (double)frequency, basis, true);
		}

		static double OddLYield(DateTime settlement, DateTime maturity, DateTime lastInterest, double rate, double pr, double redemption, Frequency frequency, DayCountBasis basis)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("settlement must be after lastInterest", settlement > lastInterest);
			FinancialFunctions.Common.Ensure("rate must be more than 0", rate >= 0.0);
			FinancialFunctions.Common.Ensure("pr must be more than 0", pr >= 0.0);
			FinancialFunctions.Common.Ensure("redemption must be more than 0", redemption >= 0.0);
			return FinancialFunctions.OddLFunc(settlement, maturity, lastInterest, rate, pr, redemption, (double)frequency, basis, false);
		}

		static double CumIpmtInternal(double r, double nper, double pv, double startPeriod, double endPeriod, PaymentDue pd)
		{
			FinancialFunctions.Common.Ensure("r is not raisable to nper (r is negative and nper not an integer)", FinancialFunctions.Common.Raisable(r, nper));
			FinancialFunctions.Common.Ensure("r is not raisable to (per - 1) (r is negative and nper not an integer)", FinancialFunctions.Common.Raisable(r, startPeriod - 1.0));
			FinancialFunctions.Common.Ensure("pv must be more than 0", pv > 0.0);
			FinancialFunctions.Common.Ensure("r must be more than 0", r > 0.0);
			FinancialFunctions.Common.Ensure("nper must be more than 0", nper > 0.0);
			FinancialFunctions.Common.Ensure("1 * pd + 1 - (1 / (1 + r)^nper) / nper has to be <> 0", Math.Abs(FinancialFunctions.AnnuityCertainPvFactor(r, nper, pd)) > 1E-08);
			FinancialFunctions.Common.Ensure("startPeriod must be less or equal to endPeriod", startPeriod <= endPeriod);
			FinancialFunctions.Common.Ensure("startPeriod must be more or equal to 1", startPeriod >= 1.0);
			FinancialFunctions.Common.Ensure("startPeriod and endPeriod must be less or equal to nper", endPeriod <= nper);
			return FinancialFunctions.Common.AggrBetween<double>((int)FinancialFunctions.Common.Ceiling(startPeriod), (int)endPeriod, new Func<double, int, double>(FinancialFunctions.FCumIpmt(r, nper, pv, pd).Invoke), 0.0);
		}

		static double CumPrinc(double r, double nper, double pv, double startPeriod, double endPeriod, PaymentDue pd)
		{
			FinancialFunctions.Common.Ensure("r is not raisable to nper (r is negative and nper not an integer)", FinancialFunctions.Common.Raisable(r, nper));
			FinancialFunctions.Common.Ensure("r is not raisable to (per - 1) (r is negative and nper not an integer)", FinancialFunctions.Common.Raisable(r, startPeriod - 1.0));
			FinancialFunctions.Common.Ensure("pv must be more than 0", pv > 0.0);
			FinancialFunctions.Common.Ensure("r must be more than 0", r > 0.0);
			FinancialFunctions.Common.Ensure("nper must be more than 0", nper > 0.0);
			FinancialFunctions.Common.Ensure("1 * pd + 1 - (1 / (1 + r)^nper) / nper has to be <> 0", Math.Abs(FinancialFunctions.AnnuityCertainPvFactor(r, nper, pd)) > 1E-08);
			FinancialFunctions.Common.Ensure("startPeriod must be less or equal to endPeriod", startPeriod <= endPeriod);
			FinancialFunctions.Common.Ensure("startPeriod must be more or equal to 1", startPeriod >= 1.0);
			FinancialFunctions.Common.Ensure("startPeriod and endPeriod must be less or equal to nper", endPeriod <= nper);
			return FinancialFunctions.Common.AggrBetween<double>((int)FinancialFunctions.Common.Ceiling(startPeriod), (int)endPeriod, new Func<double, int, double>(FinancialFunctions.FCumPrinc(r, nper, pv, pd).Invoke), 0.0);
		}

		static double Ipmt(double r, double per, double nper, double pv, double fv, PaymentDue pd)
		{
			FinancialFunctions.Common.Ensure("r is not raisable to nper (r is negative and nper not an integer)", FinancialFunctions.Common.Raisable(r, nper));
			FinancialFunctions.Common.Ensure("r is not raisable to (per - 1) (r is negative and nper not an integer)", FinancialFunctions.Common.Raisable(r, per - 1.0));
			FinancialFunctions.Common.Ensure("fv or pv need to be different from 0", Math.Abs(fv) >= 1E-08 || pv != 0.0);
			FinancialFunctions.Common.Ensure("r cannot be -100% when nper is <= 0", r != -1.0 || (r == -1.0 && per > 1.0 && nper > 1.0 && pd == PaymentDue.EndOfPeriod));
			FinancialFunctions.Common.Ensure("1 * pd + 1 - (1 / (1 + r)^nper) / nper has to be <> 0", FinancialFunctions.AnnuityCertainPvFactor(r, nper, pd) != 0.0);
			FinancialFunctions.Common.Ensure("per must be in the range 1 to nper", per >= 1.0 && per <= nper);
			FinancialFunctions.Common.Ensure("nper must be more than 0", nper > 0.0);
			if ((int)per == 1 && pd == PaymentDue.BeginningOfPeriod)
			{
				return 0.0;
			}
			if (r == -1.0)
			{
				return -fv;
			}
			return FinancialFunctions.IpmtInternal(r, per, nper, pv, fv, pd);
		}

		static double Ispmt(double r, double per, double nper, double pv)
		{
			return FinancialFunctions.IspmtInternal(r, per, nper, pv);
		}

		static double Ppmt(double r, double per, double nper, double pv, double fv, PaymentDue pd)
		{
			FinancialFunctions.Common.Ensure("r is not raisable to nper (r is negative and nper not an integer)", FinancialFunctions.Common.Raisable(r, nper));
			FinancialFunctions.Common.Ensure("r is not raisable to (per - 1) (r is negative and nper not an integer)", FinancialFunctions.Common.Raisable(r, per - 1.0));
			FinancialFunctions.Common.Ensure("fv or pv need to be different from 0", fv != 0.0 || pv != 0.0);
			FinancialFunctions.Common.Ensure("r cannot be -100% when nper is <= 0", r != -1.0 || (r == -1.0 && per > 1.0 && nper > 1.0 && pd == PaymentDue.EndOfPeriod));
			FinancialFunctions.Common.Ensure("1 * pd + 1 - (1 / (1 + r)^nper) / nper has to be <> 0", FinancialFunctions.AnnuityCertainPvFactor(r, nper, pd) != 0.0);
			FinancialFunctions.Common.Ensure("per must be in the range 1 to nper", per >= 1.0 && per <= nper);
			FinancialFunctions.Common.Ensure("nper must be more than 0", nper > 0.0);
			if ((int)per == 1 && pd == PaymentDue.BeginningOfPeriod)
			{
				return FinancialFunctions.PmtInternal(r, nper, pv, fv, pd);
			}
			if (r == -1.0)
			{
				return 0.0;
			}
			return FinancialFunctions.PpmtInternal(r, per, nper, pv, fv, pd);
		}

		static double IpmtInternal(double r, double per, double nper, double pv, double fv, PaymentDue pd)
		{
			double num = -(pv * FinancialFunctions.FvFactor(r, per - 1.0) * r + FinancialFunctions.PmtInternal(r, nper, pv, fv, PaymentDue.EndOfPeriod) * (FinancialFunctions.FvFactor(r, per - 1.0) - 1.0));
			if (pd == PaymentDue.EndOfPeriod)
			{
				return num;
			}
			return num / (1.0 + r);
		}

		static double IspmtInternal(double r, double per, double nper, double pv)
		{
			double num = -pv * r;
			return num - num / nper * per;
		}

		static double PpmtInternal(double r, double per, double nper, double pv, double fv, PaymentDue pd)
		{
			return FinancialFunctions.PmtInternal(r, nper, pv, fv, pd) - FinancialFunctions.IpmtInternal(r, per, nper, pv, fv, pd);
		}

		static Func<double, int, double> FCumIpmt(double r, double nper, double pv, PaymentDue pd)
		{
			return (double acc, int per) => acc + FinancialFunctions.Ipmt(r, (double)per, nper, pv, 0.0, pd);
		}

		static Func<double, int, double> FCumPrinc(double r, double nper, double pv, PaymentDue pd)
		{
			return (double acc, int per) => acc + FinancialFunctions.Ppmt(r, (double)per, nper, pv, 0.0, pd);
		}

		static double AnnuityCertainFvFactor(double r, double nper, PaymentDue pd)
		{
			return FinancialFunctions.AnnuityCertainPvFactor(r, nper, pd) * FinancialFunctions.FvFactor(r, nper);
		}

		static double AnnuityCertainPvFactor(double r, double nper, PaymentDue pd)
		{
			if (r == 0.0)
			{
				return nper;
			}
			return (1.0 + r * (double)pd) * (1.0 - FinancialFunctions.PvFactor(r, nper)) / r;
		}

		static double Fv(double r, double nper, double pmt, double pv, PaymentDue pd)
		{
			FinancialFunctions.Common.Ensure("r is not raisable to nper (r is negative and nper not an integer", FinancialFunctions.Common.Raisable(r, nper));
			bool c = r != -1.0 || (r == -1.0 && nper > 0.0);
			FinancialFunctions.Common.Ensure("r cannot be -100% when nper is <= 0", c);
			if (pmt == 0.0 && pv == 0.0)
			{
				return 0.0;
			}
			if (r == -1.0 && pd == PaymentDue.BeginningOfPeriod)
			{
				return -(pv * FinancialFunctions.FvFactor(r, nper));
			}
			if (r == -1.0 && pd == PaymentDue.EndOfPeriod)
			{
				return -(pv * FinancialFunctions.FvFactor(r, nper) + pmt);
			}
			return FinancialFunctions.FvInternal(r, nper, pmt, pv, pd);
		}

		static double FvSchedule(double pv, IEnumerable<double> interests)
		{
			double num = pv;
			IEnumerator<double> enumerator = interests.GetEnumerator();
			using (enumerator)
			{
				while (enumerator.MoveNext())
				{
					double num2 = enumerator.Current;
					num *= num2 + 1.0;
				}
			}
			return num;
		}

		static double Nper(double r, double pmt, double pv, double fv, PaymentDue pd)
		{
			if (r != 0.0 || pmt == 0.0)
			{
				return FinancialFunctions.NperInternal(r, pmt, pv, fv, pd);
			}
			return -(fv + pv) / pmt;
		}

		static double Pmt(double r, double nper, double pv, double fv, PaymentDue pd)
		{
			FinancialFunctions.Common.Ensure("r is not raisable to nper (r is negative and nper not an integer", FinancialFunctions.Common.Raisable(r, nper));
			FinancialFunctions.Common.Ensure("fv or pv need to be different from 0", fv != 0.0 || pv != 0.0);
			bool c = r != -1.0 || (r == -1.0 && nper > 0.0 && pd == PaymentDue.EndOfPeriod);
			FinancialFunctions.Common.Ensure("r cannot be -100% when nper is <= 0", c);
			FinancialFunctions.Common.Ensure("1 * pd + 1 - (1 / (1 + r)^nper) / nper has to be <> 0", FinancialFunctions.AnnuityCertainPvFactor(r, nper, pd) != 0.0);
			if (r == -1.0)
			{
				return -fv;
			}
			return FinancialFunctions.PmtInternal(r, nper, pv, fv, pd);
		}

		static double Pv(double r, double nper, double pmt, double fv, PaymentDue pd)
		{
			FinancialFunctions.Common.Ensure("r is not raisable to nper (r is less than -1 and nper not an integer", FinancialFunctions.Common.Raisable(r, nper));
			FinancialFunctions.Common.Ensure("r cannot be -100%", r != -1.0);
			if (Math.Abs(r) > 19.45)
			{
				throw new Exception();
			}
			return FinancialFunctions.PvInternal(r, nper, pmt, fv, pd);
		}

		static double Rate(double nper, double pmt, double pv, double fv, PaymentDue pd, double guess)
		{
			FinancialFunctions.Common.Ensure("pmt or pv need to be different from 0", pmt != 0.0 || pv != 0.0);
			FinancialFunctions.Common.Ensure("nper needs to be more than 0", nper > 0.0);
			FinancialFunctions.Common.Ensure("There must be at least a change in sign in pv, fv and pmt", FinancialFunctions.CheckRate(pmt, pv, fv));
			if (fv == 0.0 && pv == 0.0)
			{
				return (double)((pmt >= 0.0) ? 1 : (-1));
			}
			Func<double, double> @object = FinancialFunctions.FRate(nper, pmt, pv, fv, pd);
			return FinancialFunctions.Common.FindRoot(new Func<double, double>(@object.Invoke), guess);
		}

		static double FvInternal(double r, double nper, double pmt, double pv, PaymentDue pd)
		{
			return -(pv * FinancialFunctions.FvFactor(r, nper) + pmt * FinancialFunctions.AnnuityCertainFvFactor(r, nper, pd));
		}

		static double FvFactor(double r, double nper)
		{
			return Math.Pow(1.0 + r, nper);
		}

		static double NperInternal(double r, double pmt, double pv, double fv, PaymentDue pd)
		{
			double x = FinancialFunctions.NperFactor(r, pmt, -fv, pd) / FinancialFunctions.NperFactor(r, pmt, pv, pd);
			return FinancialFunctions.Common.Ln(x) / FinancialFunctions.Common.Ln(r + 1.0);
		}

		static double NperFactor(double r, double pmt, double v, PaymentDue pd)
		{
			return v * r + pmt * (1.0 + r * (double)pd);
		}

		static double PmtInternal(double r, double nper, double pv, double fv, PaymentDue pd)
		{
			return -(pv + fv * FinancialFunctions.PvFactor(r, nper)) / FinancialFunctions.AnnuityCertainPvFactor(r, nper, pd);
		}

		static double PvInternal(double r, double nper, double pmt, double fv, PaymentDue pd)
		{
			return -(fv * FinancialFunctions.PvFactor(r, nper) + pmt * FinancialFunctions.AnnuityCertainPvFactor(r, nper, pd));
		}

		static double PvFactor(double r, double nper)
		{
			return Math.Pow(1.0 + r, -nper);
		}

		static double RaiseIfNonsense(double value)
		{
			if (double.IsNaN(value) || double.IsInfinity(value))
			{
				throw new Exception("Not a number.");
			}
			return value;
		}

		static Func<double, double> FRate(double nper, double pmt, double pv, double fv, PaymentDue pd)
		{
			return (double r) => FinancialFunctions.Fv(r, nper, pmt, pv, pd) - fv;
		}

		static bool CheckRate(double x, double y, double z)
		{
			bool flag = (FinancialFunctions.Common.Sign(x) != FinancialFunctions.Common.Sign(y) || FinancialFunctions.Common.Sign(y) != FinancialFunctions.Common.Sign(z)) && (FinancialFunctions.Common.Sign(x) != FinancialFunctions.Common.Sign(y) || z == 0.0);
			bool flag2 = flag && (FinancialFunctions.Common.Sign(x) != FinancialFunctions.Common.Sign(z) || y == 0.0);
			return flag2 && (FinancialFunctions.Common.Sign(y) != FinancialFunctions.Common.Sign(z) || x == 0.0);
		}

		static double DollarFr(double fractionalDollar, double fraction)
		{
			FinancialFunctions.Common.Ensure("fraction must be more than 0", fraction > 0.0);
			FinancialFunctions.Common.Ensure("10^(ceiling (log10 (floor fraction))) must be different from 0", FinancialFunctions.Common.Pow(10.0, FinancialFunctions.Common.Ceiling(FinancialFunctions.Common.Log10(FinancialFunctions.Common.Floor(fraction)))) != 0.0);
			return FinancialFunctions.Dollar<double>(fractionalDollar, fraction, new Func<double, double, double, double, double>(FinancialFunctions.DollarFrInternal));
		}

		static double Effect(double nominalRate, double npery)
		{
			FinancialFunctions.Common.Ensure("nominal rate must be more than zero", nominalRate > 0.0);
			FinancialFunctions.Common.Ensure("npery must be more or equal to one", npery >= 1.0);
			return FinancialFunctions.EffectInternal(nominalRate, npery);
		}

		static double Nominal(double effectRate, double npery)
		{
			FinancialFunctions.Common.Ensure("effective rate must be more than zero", effectRate > 0.0);
			FinancialFunctions.Common.Ensure("npery must be more or equal to one", npery >= 1.0);
			double num = FinancialFunctions.Common.Floor(npery);
			return (FinancialFunctions.Common.Pow(effectRate + 1.0, 1.0 / num) - 1.0) * num;
		}

		static T Dollar<T>(double fractionalDollar, double fraction, Func<double, double, double, double, T> f)
		{
			double num = FinancialFunctions.Common.Floor(fraction);
			double num2 = ((fractionalDollar <= 0.0) ? FinancialFunctions.Common.Ceiling(fractionalDollar) : FinancialFunctions.Common.Floor(fractionalDollar));
			double num3 = num2;
			double arg = fractionalDollar - num3;
			double arg2 = FinancialFunctions.Common.Pow(10.0, FinancialFunctions.Common.Ceiling(FinancialFunctions.Common.Log10(num)));
			return f(num, num3, arg, arg2);
		}

		static double DollarDeInternal(double aBase, double dollar, double remainder, double digits)
		{
			return remainder * digits / aBase + dollar;
		}

		static double DollarFrInternal(double aBase, double dollar, double remainder, double digits)
		{
			double num = Math.Abs(digits);
			return remainder * aBase / num + dollar;
		}

		static double EffectInternal(double nominalRate, double npery)
		{
			double num = FinancialFunctions.Common.Floor(npery);
			return FinancialFunctions.Common.Pow(nominalRate / num + 1.0, num) - 1.0;
		}

		static double BillPriceInternal(DateTime settlement, DateTime maturity, double discount)
		{
			double dsm = FinancialFunctions.GetDsm(settlement, maturity, DayCountBasis.ActualActual);
			return 100.0 * (1.0 - discount * dsm / 360.0);
		}

		static double BillYieldInternal(DateTime settlement, DateTime maturity, double pr)
		{
			double dsm = FinancialFunctions.GetDsm(settlement, maturity, DayCountBasis.ActualActual);
			return (100.0 - pr) / pr * 360.0 / dsm;
		}

		static double BillEq(DateTime settlement, DateTime maturity, double discount)
		{
			double dsm = FinancialFunctions.GetDsm(settlement, maturity, DayCountBasis.Actual360);
			double num = (100.0 - discount * 100.0 * dsm / 360.0) / 100.0;
			int num2 = ((dsm != 366.0) ? 365 : 366);
			double x = FinancialFunctions.Common.Pow(dsm / (double)num2, 2.0) - (2.0 * dsm / (double)num2 - 1.0) * (1.0 - 1.0 / num);
			FinancialFunctions.Common.Ensure("2. * dsm / days - 1. must be different from 0", 2.0 * dsm / (double)num2 - 1.0 != 0.0);
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("maturity must be less than one year after settlement", maturity <= FinancialFunctions.Common.AddYears(settlement, 1));
			FinancialFunctions.Common.Ensure("investment must be more than 0", discount > 0.0);
			if (dsm <= 182.0)
			{
				return 365.0 * discount / (360.0 - discount * dsm);
			}
			return 2.0 * (FinancialFunctions.Common.Sqr(x) - dsm / (double)num2) / (2.0 * dsm / (double)num2 - 1.0);
		}

		static double BillPrice(DateTime settlement, DateTime maturity, double discount)
		{
			double dsm = FinancialFunctions.GetDsm(settlement, maturity, DayCountBasis.ActualActual);
			FinancialFunctions.Common.Ensure("a result less than zero triggers an exception", 100.0 * (1.0 - discount * dsm / 360.0) > 0.0);
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("maturity must be less than one year after settlement", maturity <= FinancialFunctions.Common.AddYears(settlement, 1));
			FinancialFunctions.Common.Ensure("discount must be more than 0", discount > 0.0);
			return FinancialFunctions.BillPriceInternal(settlement, maturity, discount);
		}

		static double BillYield(DateTime settlement, DateTime maturity, double pr)
		{
			FinancialFunctions.Common.Ensure("maturity must be after settlement", maturity > settlement);
			FinancialFunctions.Common.Ensure("maturity must be less than one year after settlement", maturity <= FinancialFunctions.Common.AddYears(settlement, 1));
			FinancialFunctions.Common.Ensure("pr must be more than 0", pr > 0.0);
			return FinancialFunctions.BillYieldInternal(settlement, maturity, pr);
		}

		static double GetDsm(DateTime settlement, DateTime maturity, DayCountBasis basis)
		{
			FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
			FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
			return dayCounter2.DaysBetween(settlement, maturity, NumDenumPosition.Numerator);
		}

		static double Irr(IEnumerable<double> cfs, double guess)
		{
			FinancialFunctions.Common.Ensure("There must be one positive and one negative cash flow", FinancialFunctions.ValidCfs(cfs));
			return FinancialFunctions.Common.FindRoot((double d) => FinancialFunctions.NpvInternal(d, cfs), guess);
		}

		static double Mirr(IEnumerable<double> cfs, double financeRate, double reinvestRate)
		{
			FinancialFunctions.Common.Ensure("financeRate cannot be -100%", financeRate != -1.0);
			FinancialFunctions.Common.Ensure("reinvestRate cannot be -100%", reinvestRate != -1.0);
			FinancialFunctions.Common.Ensure("cfs must contain more than one cashflow", cfs.Count<double>() != 1);
			FinancialFunctions.Common.Ensure("The NPV calculated using financeRate and the negative cashflows in cfs must be different from zero", FinancialFunctions.NpvInternal(financeRate, cfs.Map(delegate(double cf)
			{
				if (cf < 0.0)
				{
					return cf;
				}
				return 0.0;
			})) != 0.0);
			return FinancialFunctions.MirrInternal(cfs, financeRate, reinvestRate);
		}

		static double Npv(double r, IEnumerable<double> cfs)
		{
			FinancialFunctions.Common.Ensure("r cannot be -100%", r != -1.0);
			return FinancialFunctions.NpvInternal(r, cfs);
		}

		static double Xirr(IEnumerable<double> cfs, IEnumerable<DateTime> dates, double guess)
		{
			FinancialFunctions.Common.Ensure("There must be one positive and one negative cash flow", FinancialFunctions.ValidCfs(cfs));
			FinancialFunctions.Common.Ensure("In dates, one date is less than the first date", dates.ToList<DateTime>().Any((DateTime time) => time <= dates.First<DateTime>()));
			FinancialFunctions.Common.Ensure("cfs and dates must have the same length", cfs.Count<double>() == dates.Count<DateTime>());
			return FinancialFunctions.Common.FindRoot((double r) => FinancialFunctions.XnpvInternal(r, cfs, dates), guess);
		}

		static double Xnpv(double r, IEnumerable<double> cfs, IEnumerable<DateTime> dates)
		{
			FinancialFunctions.Common.Ensure("r cannot be -100%", r != -1.0);
			FinancialFunctions.Common.Ensure("In dates, one date is less than the first date", dates.ToList<DateTime>().Any((DateTime time) => time <= dates.First<DateTime>()));
			FinancialFunctions.Common.Ensure("cfs and dates must have the same length", cfs.Count<double>() == dates.Count<DateTime>());
			return FinancialFunctions.XnpvInternal(r, cfs, dates);
		}

		static double MirrInternal(IEnumerable<double> cfs, double financeRate, double reinvestRate)
		{
			double num = (double)cfs.Count<double>();
			List<double> list = cfs.ToList<double>();
			IEnumerable<double> cfs2 = list.Map((double input) => Math.Max(input, 0.0));
			IEnumerable<double> cfs3 = list.Map((double input) => Math.Min(input, 0.0));
			return Math.Pow(-FinancialFunctions.NpvInternal(reinvestRate, cfs2) * Math.Pow(1.0 + reinvestRate, num) / (FinancialFunctions.NpvInternal(financeRate, cfs3) * (1.0 + financeRate)), 1.0 / (num - 1.0)) - 1.0;
		}

		static double NpvInternal(double r, IEnumerable<double> cfs)
		{
			double num = 0.0;
			List<double> list = cfs.ToList<double>();
			for (int i = 0; i < list.Count<double>(); i++)
			{
				num += list[i] * FinancialFunctions.PvFactor(r, (double)(i + 1));
			}
			return num;
		}

		static bool ValidCfs(IEnumerable<double> cfs)
		{
			return FinancialFunctions.ValidCfs(cfs, false, false);
		}

		static bool ValidCfs(IEnumerable<double> cfs, bool pos, bool neg)
		{
			if (pos && neg)
			{
				return true;
			}
			if (cfs == null || !cfs.Any<double>())
			{
				return false;
			}
			FunctionalList<double> functionalList = new FunctionalList<double>(cfs);
			double head = functionalList.Head;
			FunctionalList<double> tail = functionalList.Tail;
			if (head <= 0.0)
			{
				return FinancialFunctions.ValidCfs(tail, pos, true);
			}
			return FinancialFunctions.ValidCfs(tail, true, neg);
		}

		static double XnpvInternal(double r, IEnumerable<double> cfs, IEnumerable<DateTime> dates)
		{
			IList<DateTime> list = (dates as IList<DateTime>) ?? dates.ToList<DateTime>();
			DateTime d0 = list.First<DateTime>();
			IEnumerable<double> source = list.Zip(cfs, (DateTime d, double cf) => cf / Math.Pow(1.0 + r, (double)FinancialFunctions.Common.Days(d, d0) / 365.0));
			return source.Sum();
		}

		public static double ACCRINT(DateTime issue, DateTime firstInterestData, DateTime settlement, double rate, double par, double frequency = 1.0, int basis = 0)
		{
			return FinancialFunctions.AccrInt(issue, firstInterestData, settlement, rate, par, frequency, (DayCountBasis)basis, AccrIntCalcMethod.FromIssueToSettlement);
		}

		public static double ACCRINTM(DateTime issue, DateTime settlement, double rate, double par, int basis = 0)
		{
			return FinancialFunctions.AccrIntM(issue, settlement, rate, par, (DayCountBasis)basis);
		}

		public static double AMORDEGRC(double cost, DateTime datePurchased, DateTime firstPeriod, double salvage, double period, double rate, int basis)
		{
			return FinancialFunctions.Depreciation.AmorDegrc(cost, datePurchased, firstPeriod, salvage, period, rate, (DayCountBasis)basis, true);
		}

		public static double AMORLINC(double cost, DateTime datePurchased, DateTime firstPeriod, double salvage, double period, double rate, int basis)
		{
			return FinancialFunctions.Depreciation.AmorLinc(cost, datePurchased, firstPeriod, salvage, period, rate, (DayCountBasis)basis);
		}

		public static double COUPDAYS(DateTime settlement, DateTime maturity, int frequency, int basis)
		{
			return FinancialFunctions.DayCount.CoupDays(settlement, maturity, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double COUPDAYSBS(DateTime settlement, DateTime maturity, int frequency, int basis)
		{
			return FinancialFunctions.DayCount.CoupDaysBS(settlement, maturity, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double COUPDAYSNC(DateTime settlement, DateTime maturity, int frequency, int basis)
		{
			return FinancialFunctions.DayCount.CoupDaysNC(settlement, maturity, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static DateTime COUPNCD(DateTime settlement, DateTime maturity, int frequency, int basis)
		{
			return FinancialFunctions.DayCount.CoupNCD(settlement, maturity, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double COUPNUM(DateTime settlement, DateTime maturity, int frequency, int basis)
		{
			return FinancialFunctions.DayCount.CoupNum(settlement, maturity, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static DateTime COUPPCD(DateTime settlement, DateTime maturity, int frequency, int basis)
		{
			return FinancialFunctions.DayCount.CoupPCD(settlement, maturity, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double CUMIPMT(double rate, double nper, double pv, double startPeriod, double endPeriod, int typ)
		{
			return FinancialFunctions.CumIpmtInternal(rate, nper, pv, startPeriod, endPeriod, (PaymentDue)typ);
		}

		public static double CUMPRINC(double rate, double nper, double pv, double startPeriod, double endPeriod, int typ)
		{
			return FinancialFunctions.CumPrinc(rate, nper, pv, startPeriod, endPeriod, (PaymentDue)typ);
		}

		public static double DB(double cost, double salvage, double life, double period, double month)
		{
			return FinancialFunctions.Depreciation.Db(cost, salvage, life, period, month);
		}

		public static double DDB(double cost, double salvage, double life, double period)
		{
			return FinancialFunctions.Depreciation.Ddb(cost, salvage, life, period, 2.0);
		}

		public static double DISC(DateTime settlement, DateTime maturity, double pr, double redemption, int basis)
		{
			return FinancialFunctions.Disc(settlement, maturity, pr, redemption, (DayCountBasis)basis);
		}

		public static double DOLLARDE(double fractionalDollar, double fraction)
		{
			return FinancialFunctions.DollarDe(fractionalDollar, fraction);
		}

		public static double DOLLARFR(double decimalDollar, double fraction)
		{
			return FinancialFunctions.DollarFr(decimalDollar, fraction);
		}

		public static double DURATION(DateTime settlement, DateTime maturity, double coupon, double yld, int frequency, int basis)
		{
			return FinancialFunctions.Duration(settlement, maturity, coupon, yld, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double EFFECT(double nominalRate, double npery)
		{
			return FinancialFunctions.Effect(nominalRate, npery);
		}

		public static double FV(double rate, double nPer, double pmt, double pv = 0.0, int due = 0)
		{
			return FinancialFunctions.Fv(rate, nPer, pmt, pv, (PaymentDue)due);
		}

		public static double FVSCHEDULE(double principal, IEnumerable<double> schedule)
		{
			return FinancialFunctions.FvSchedule(principal, schedule);
		}

		public static double IPMT(double rate, double per, double nPer, double pv, double fv = 0.0, int due = 0)
		{
			return FinancialFunctions.Ipmt(rate, per, nPer, pv, fv, (PaymentDue)due);
		}

		public static double IRR(double[] values, double guess = 0.1)
		{
			return FinancialFunctions.Irr(values, guess);
		}

		public static double ISPMT(double rate, double per, double nper, double pv)
		{
			return FinancialFunctions.Ispmt(rate, per, nper, pv);
		}

		public static double INRATE(DateTime settlement, DateTime maturity, double investment, double redemption, int basis)
		{
			return FinancialFunctions.IntRate(settlement, maturity, investment, redemption, (DayCountBasis)basis);
		}

		public static double MDURATION(DateTime settlement, DateTime maturity, double coupon, double yld, int frequency, int basis)
		{
			return FinancialFunctions.MDuration(settlement, maturity, coupon, yld, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double MIRR(double[] values, double financeRate, double reinvestRate)
		{
			return FinancialFunctions.Mirr(values, financeRate, reinvestRate);
		}

		public static double NPER(double rate, double pmt, double pv, double fv = 0.0, int due = 0)
		{
			return FinancialFunctions.RaiseIfNonsense(FinancialFunctions.Nper(rate, pmt, pv, fv, (PaymentDue)due));
		}

		public static double NPV(double rate, double[] values)
		{
			return FinancialFunctions.Npv(rate, values);
		}

		public static double NOMINAL(double effectRate, double npery)
		{
			return FinancialFunctions.Nominal(effectRate, npery);
		}

		public static double ODDFPRICE(DateTime settlement, DateTime maturity, DateTime issue, DateTime firstCoupon, double rate, double yld, double redemption, int frequency, int basis)
		{
			return FinancialFunctions.OddFPrice(settlement, maturity, issue, firstCoupon, rate, yld, redemption, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double ODDFYIELD(DateTime settlement, DateTime maturity, DateTime issue, DateTime firstCoupon, double rate, double pr, double redemption, int frequency, int basis)
		{
			return FinancialFunctions.OddFYield(settlement, maturity, issue, firstCoupon, rate, pr, redemption, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double ODDLPRICE(DateTime settlement, DateTime maturity, DateTime lastInterest, double rate, double yld, double redemption, int frequency, int basis)
		{
			return FinancialFunctions.OddLPrice(settlement, maturity, lastInterest, rate, yld, redemption, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double ODDLYIELD(DateTime settlement, DateTime maturity, DateTime lastInterest, double rate, double pr, double redemption, int frequency, int basis)
		{
			return FinancialFunctions.OddLYield(settlement, maturity, lastInterest, rate, pr, redemption, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double PDURATION(double rate, double pv, double fv)
		{
			return (Math.Log(fv) - Math.Log(pv)) / Math.Log(1.0 + rate);
		}

		public static double PMT(double rate, double nPer, double pv, double fv = 0.0, int due = 0)
		{
			return FinancialFunctions.Pmt(rate, nPer, pv, fv, (PaymentDue)due);
		}

		public static double PPMT(double rate, double per, double nPer, double pv, double fv = 0.0, int due = 0)
		{
			return FinancialFunctions.Ppmt(rate, per, nPer, pv, fv, (PaymentDue)due);
		}

		public static double PV(double rate, double nPer, double pmt, double fv = 0.0, int due = 0)
		{
			return FinancialFunctions.RaiseIfNonsense(FinancialFunctions.Pv(rate, nPer, pmt, fv, (PaymentDue)due));
		}

		public static double PRICE(DateTime settlement, DateTime maturity, double rate, double yld, double redemption, int frequency, int basis)
		{
			return FinancialFunctions.Price(settlement, maturity, rate, yld, redemption, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double PRICEDISC(DateTime settlement, DateTime maturity, double discount, double redemption, int basis)
		{
			return FinancialFunctions.PriceDisc(settlement, maturity, discount, redemption, (DayCountBasis)basis);
		}

		public static double RATE(double nPer, double pmt, double pv, double fv = 0.0, int due = 0, double guess = 0.1)
		{
			return FinancialFunctions.RaiseIfNonsense(FinancialFunctions.Rate(nPer, pmt, pv, fv, (PaymentDue)due, 0.1));
		}

		public static double RRI(double nper, double pv, double fv)
		{
			if (pv == 0.0)
			{
				return 0.0;
			}
			if (nper == 0.0)
			{
				return 0.0;
			}
			return Math.Pow(fv / pv, 1.0 / nper) - 1.0;
		}

		public static double Received(DateTime settlement, DateTime maturity, double investment, double discount, int basis)
		{
			return FinancialFunctions.Received(settlement, maturity, investment, discount, (DayCountBasis)basis);
		}

		public static double SLN(double cost, double salvage, double life)
		{
			return FinancialFunctions.Depreciation.Sln(cost, salvage, life);
		}

		public static double SYD(double cost, double salvage, double life, double period)
		{
			return FinancialFunctions.Depreciation.Syd(cost, salvage, life, period);
		}

		public static double TBILLEQ(DateTime settlement, DateTime maturity, double discount)
		{
			return FinancialFunctions.BillEq(settlement, maturity, discount);
		}

		public static double TBILLPRICE(DateTime settlement, DateTime maturity, double discount)
		{
			return FinancialFunctions.BillPrice(settlement, maturity, discount);
		}

		public static double TBILLYIELD(DateTime settlement, DateTime maturity, double pr)
		{
			return FinancialFunctions.BillYield(settlement, maturity, pr);
		}

		public static double VDB(double cost, double salvage, double life, double startPeriod, double endPeriod, double factor, int noSwitch)
		{
			return FinancialFunctions.Depreciation.Vdb(cost, salvage, life, startPeriod, endPeriod, factor, (VdbSwitch)noSwitch);
		}

		public static double VDB(double cost, double salvage, double life, double startPeriod, double endPeriod, double factor)
		{
			return FinancialFunctions.Depreciation.Vdb(cost, salvage, life, startPeriod, endPeriod, factor, VdbSwitch.SwitchToStraightLine);
		}

		public static double VDB(double cost, double salvage, double life, double startPeriod, double endPeriod)
		{
			return FinancialFunctions.RaiseIfNonsense(FinancialFunctions.Depreciation.Vdb(cost, salvage, life, startPeriod, endPeriod, 2.0, VdbSwitch.SwitchToStraightLine));
		}

		public static double XIRR(IEnumerable<double> values, IEnumerable<DateTime> dates, double guess)
		{
			return FinancialFunctions.Xirr(values, dates, guess);
		}

		public static double XIRR(IEnumerable<double> values, IEnumerable<DateTime> dates)
		{
			return FinancialFunctions.Xirr(values, dates, 0.1);
		}

		public static double XNPV(double rate, IEnumerable<double> values, IEnumerable<DateTime> dates)
		{
			return FinancialFunctions.Xnpv(rate, values, dates);
		}

		public static double YIELD(DateTime settlement, DateTime maturity, double rate, double pr, double redemption, int frequency, int basis)
		{
			return FinancialFunctions.Yield(settlement, maturity, rate, pr, redemption, (Frequency)frequency, (DayCountBasis)basis);
		}

		public static double YIELDDISC(DateTime settlement, DateTime maturity, double pr, double redemption, int basis)
		{
			return FinancialFunctions.YieldDisc(settlement, maturity, pr, redemption, (DayCountBasis)basis);
		}

		public static double YIELDMAT(DateTime settlement, DateTime maturity, DateTime issue, double rate, double pr, int basis)
		{
			return FinancialFunctions.YieldMat(settlement, maturity, issue, rate, pr, (DayCountBasis)basis);
		}

		public static double YEARFRAC(DateTime startDate, DateTime endDate, int basis = 0)
		{
			return FinancialFunctions.DayCount.YearFrac(startDate, endDate, (DayCountBasis)basis);
		}

		static class Depreciation
		{
			static double DeprBetweenPeriods(double cost, double salvage, double life, double startPeriod, double endPeriod, double factor, bool straightLine)
			{
				return FinancialFunctions.Depreciation.TotalDepr(cost, salvage, life, endPeriod, factor, straightLine) - FinancialFunctions.Depreciation.TotalDepr(cost, salvage, life, startPeriod, factor, straightLine);
			}

			static double DeprCoeff(double assetLife)
			{
				Func<double, double, bool> func = (double x1, double x2) => x1 <= assetLife && assetLife <= x2;
				if (func(3.0, 4.0))
				{
					return 1.5;
				}
				if (func(5.0, 6.0))
				{
					return 2.0;
				}
				if (assetLife <= 6.0)
				{
					return 1.0;
				}
				return 2.5;
			}

			static Func<double, double, double> FDb(double cost, double life, double period, double month, double rate)
			{
				return delegate(double totDepr, double per)
				{
					int num = (int)month;
					int num2 = (int)period - 1;
					int num3 = ((num == 12) ? ((int)life - 1) : ((int)life));
					double num5;
					for (;;)
					{
						int num4 = (int)per;
						if (num4 == 0)
						{
							num5 = cost * rate * month / 12.0;
							if (num2 == 0)
							{
								break;
							}
							per += 1.0;
							totDepr = num5;
						}
						else if (num2 == num3)
						{
							if (num4 >= num3)
							{
								goto IL_96;
							}
							per += 1.0;
							totDepr += FinancialFunctions.Depreciation.DeprForPeriod(cost, totDepr, rate);
						}
						else
						{
							if (num4 == num2)
							{
								goto Block_7;
							}
							per += 1.0;
							totDepr += FinancialFunctions.Depreciation.DeprForPeriod(cost, totDepr, rate);
						}
					}
					return num5;
					IL_96:
					if (num == 12)
					{
						return FinancialFunctions.Depreciation.DeprForPeriod(cost, totDepr, rate);
					}
					return (cost - totDepr) * rate * (12.0 - month) / 12.0;
					Block_7:
					return FinancialFunctions.Depreciation.DeprForPeriod(cost, totDepr, rate);
				};
			}

			static Func<double, double, double> FDepr(double cost, double salvage, double life)
			{
				return (double totDepr, double aPeriod) => FinancialFunctions.Depreciation.SLNInternal(cost - totDepr, salvage, life - aPeriod);
			}

			static double DaysInYear(DateTime date, DayCountBasis basis)
			{
				if (basis != DayCountBasis.ActualActual)
				{
					FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
					FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
					return dayCounter2.DaysInYear(date, date);
				}
				return (double)((!FinancialFunctions.Common.IsLeapYear(date)) ? 365 : 366);
			}

			static double DDBInternal(double cost, double salvage, double life, double period, double factor)
			{
				if (period >= 2.0)
				{
					return FinancialFunctions.Depreciation.DeprBetweenPeriods(cost, salvage, life, period - 1.0, period, factor, false);
				}
				return FinancialFunctions.Depreciation.TotalDepr(cost, salvage, life, period, factor, false);
			}

			static double DeprForPeriod(double cost, double totDepr, double rate)
			{
				return (cost - totDepr) * rate;
			}

			static double DeprRate(double cost, double salvage, double life)
			{
				if (Math.Abs(cost) >= 1E-08)
				{
					return Math.Round(1.0 - Math.Pow(salvage / cost, 1.0 / life), 3);
				}
				return 0.0;
			}

			static Tuple<double, double> FirstDeprLinc(double cost, DateTime datePurch, DateTime firstP, double salvage, double rate, double assLife, DayCountBasis basis)
			{
				Func<DateTime, DateTime> func = FinancialFunctions.Depreciation.FFirstDeprLinc(basis);
				FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
				double num = FinancialFunctions.Depreciation.DaysInYear(datePurch, basis);
				Tuple<DateTime, DateTime> tuple = new Tuple<DateTime, DateTime>(func(datePurch), func(firstP));
				DateTime item = tuple.Item2;
				DateTime item2 = tuple.Item1;
				FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
				DateTime from = item2;
				DateTime t = item;
				double num2 = dayCounter2.DaysBetween(from, t, NumDenumPosition.Numerator);
				double num3 = num2 / num * rate * cost;
				double num4 = ((num3 != 0.0) ? num3 : (cost * rate));
				double item3 = ((num3 != 0.0) ? (assLife + 1.0) : assLife);
				double num5 = cost - salvage;
				if (num4 > num5)
				{
					return new Tuple<double, double>(num5, item3);
				}
				return new Tuple<double, double>(num4, item3);
			}

			static Func<double, double, double, double, double> FAmorDegrc(double salvage, double period, bool excelComplaint, double assetLife)
			{
				return delegate(double countedPeriod, double depr, double deprRate, double remainCost)
				{
					while (countedPeriod <= period)
					{
						double num = countedPeriod + 1.0;
						double x = assetLife - num;
						double num2 = ((!FinancialFunctions.Common.AreEqual(x, 2.0)) ? (deprRate * remainCost) : (remainCost * 0.5));
						double num3 = ((!FinancialFunctions.Common.AreEqual(x, 2.0)) ? deprRate : 1.0);
						double num4 = ((remainCost >= salvage) ? num2 : ((remainCost - salvage >= 0.0) ? (remainCost - salvage) : 0.0));
						double num5 = num4;
						double num6 = remainCost - num5;
						remainCost = num6;
						deprRate = num3;
						depr = num5;
						countedPeriod = num;
					}
					return FinancialFunctions.Common.Round(excelComplaint, depr);
				};
			}

			static Func<double, double, double> FTotalDepr(double cost, double salvage, double life, double period, double factor, bool straightLine)
			{
				return delegate(double totDepr, double per)
				{
					double num;
					Func<double, double> func;
					Func<double, double, double> func2;
					double num4;
					for (;;)
					{
						num = FinancialFunctions.Common.Rest(period);
						func = (double d) => FinancialFunctions.Common.Min((cost - d) * factor / life, cost - salvage - d);
						func2 = FinancialFunctions.Depreciation.FDepr(cost, salvage, life);
						Tuple<double, double> tuple = new Tuple<double, double>(func(totDepr), func2(totDepr, per));
						double item = tuple.Item2;
						double item2 = tuple.Item1;
						bool flag = straightLine && item2 < item;
						bool flag2 = flag;
						double num2 = ((!flag2) ? item2 : item);
						double num3 = num2;
						num4 = totDepr + num3;
						if ((int)period == 0)
						{
							break;
						}
						if ((int)per == (int)period - 1)
						{
							goto IL_C1;
						}
						per += 1.0;
						totDepr = num4;
					}
					return num4 * num;
					IL_C1:
					double num5 = func(num4);
					double num6 = func2(num4, per + 1.0);
					double num7;
					if (!straightLine || num5 >= num6)
					{
						num7 = num5;
					}
					else
					{
						num7 = (((int)period != (int)life) ? num6 : 0.0);
					}
					double num8 = num7;
					return num4 + num8 * num;
				};
			}

			static double SLNInternal(double cost, double salvage, double life)
			{
				return (cost - salvage) / life;
			}

			static double TotalDepr(double cost, double salvage, double life, double period, double factor, bool straightLine)
			{
				Func<double, double, double> func = FinancialFunctions.Depreciation.FTotalDepr(cost, salvage, life, period, factor, straightLine);
				return func(0.0, 0.0);
			}

			internal static double AmorDegrc(double cost, DateTime datePurchased, DateTime firstPeriod, double salvage, double period, double rate, DayCountBasis basis, bool excelComplaint)
			{
				double assetLife = 1.0 / rate;
				Func<double, double, bool> func = FinancialFunctions.Depreciation.FAmorDegrc(assetLife);
				FinancialFunctions.Common.Ensure("Assset life cannot be between 0 and 3", !func(0.0, 3.0));
				FinancialFunctions.Common.Ensure("Assset life cannot be between 4. and 5.", !func(4.0, 5.0));
				FinancialFunctions.Common.Ensure("Cost must be 0 or more", cost >= 0.0);
				FinancialFunctions.Common.Ensure("Salvage must be 0 or more", salvage >= 0.0);
				FinancialFunctions.Common.Ensure("Salvage must be less than cost", salvage < cost);
				FinancialFunctions.Common.Ensure("Period must be 0 or more", period >= 0.0);
				FinancialFunctions.Common.Ensure("Rate must be 0 or more", rate >= 0.0);
				FinancialFunctions.Common.Ensure("DatePurchased must be less than FirstPeriod", datePurchased < firstPeriod);
				FinancialFunctions.Common.Ensure("basis cannot be Actual360", basis != DayCountBasis.Actual360);
				double num = FinancialFunctions.Common.Ceiling(1.0 / rate);
				if (cost == salvage || period > num)
				{
					return 0.0;
				}
				double num2 = FinancialFunctions.Depreciation.DeprCoeff(num);
				double num3 = rate * num2;
				Tuple<double, double> tuple = FinancialFunctions.Depreciation.FirstDeprLinc(cost, datePurchased, firstPeriod, salvage, num3, num, basis);
				double item = tuple.Item1;
				double item2 = tuple.Item2;
				double num4 = FinancialFunctions.Common.Round(excelComplaint, item);
				Func<double, double, double, double, double> func2 = FinancialFunctions.Depreciation.FAmorDegrc(salvage, period, excelComplaint, item2);
				if (period != 0.0)
				{
					return func2(1.0, 0.0, num3, cost - num4);
				}
				return num4;
			}

			internal static double AmorLinc(double cost, DateTime datePurchased, DateTime firstPeriod, double salvage, double period, double rate, DayCountBasis basis)
			{
				double num = FinancialFunctions.Common.Ceiling(1.0 / rate);
				Func<double, double, double, double> func = FinancialFunctions.Depreciation.FAmorLinc(period);
				if (cost == salvage || period > num)
				{
					return 0.0;
				}
				Tuple<double, double> tuple = FinancialFunctions.Depreciation.FirstDeprLinc(cost, datePurchased, firstPeriod, salvage, rate, num, basis);
				double item = tuple.Item1;
				if (period == 0.0)
				{
					return item;
				}
				return func(1.0, rate * cost, cost - salvage - item);
			}

			internal static double Db(double cost, double salvage, double life, double period, double month)
			{
				FinancialFunctions.Common.Ensure("Cost must be 0 or more", cost >= 0.0);
				FinancialFunctions.Common.Ensure("Salvage must be 0 or more", salvage >= 0.0);
				FinancialFunctions.Common.Ensure("Life must be 0 or more", life > 0.0);
				FinancialFunctions.Common.Ensure("Month must be 0 or more", month > 0.0);
				FinancialFunctions.Common.Ensure("Period must be less than life", period <= life + 1.0);
				FinancialFunctions.Common.Ensure("Period must be more than 0", period > 0.0);
				FinancialFunctions.Common.Ensure("Month must be less or equal to 12", month <= 12.0);
				double rate = FinancialFunctions.Depreciation.DeprRate(cost, salvage, life);
				Func<double, double, double> func = FinancialFunctions.Depreciation.FDb(cost, life, period, month, rate);
				return func(0.0, 0.0);
			}

			internal static double Ddb(double cost, double salvage, double life, double period, double factor)
			{
				FinancialFunctions.Common.Ensure("Cost must be 0 or more", cost >= 0.0);
				FinancialFunctions.Common.Ensure("Salvage must be 0 or more", salvage >= 0.0);
				FinancialFunctions.Common.Ensure("Life must be 0 or more", life > 0.0);
				FinancialFunctions.Common.Ensure("Month must be 0 or more", factor > 0.0);
				FinancialFunctions.Common.Ensure("Period must be less than life", period <= life);
				FinancialFunctions.Common.Ensure("Period must be more than 0", period > 0.0);
				if ((int)period == 0)
				{
					return FinancialFunctions.Common.Min(cost * factor / life, cost - salvage);
				}
				return FinancialFunctions.Depreciation.DDBInternal(cost, salvage, life, period, factor);
			}

			internal static double Sln(double cost, double salvage, double life)
			{
				return FinancialFunctions.Depreciation.SLNInternal(cost, salvage, life);
			}

			internal static double Syd(double cost, double salvage, double life, double period)
			{
				FinancialFunctions.Common.Ensure("Salvage must be 0 or more", salvage >= 0.0);
				FinancialFunctions.Common.Ensure("Life must be 0 or more", life > 0.0);
				FinancialFunctions.Common.Ensure("Period must be less than life", period <= life);
				FinancialFunctions.Common.Ensure("Period must be more than 0", period > 0.0);
				return (cost - salvage) * (life - period + 1.0) * 2.0 / (life * (life + 1.0));
			}

			internal static double Vdb(double cost, double salvage, double life, double startPeriod, double endPeriod, double factor, VdbSwitch bflag)
			{
				FinancialFunctions.Common.Ensure("Cost must be 0 or more", cost >= 0.0);
				FinancialFunctions.Common.Ensure("Salvage must be 0 or more", salvage >= 0.0);
				FinancialFunctions.Common.Ensure("Life must be 0 or more", life > 0.0);
				FinancialFunctions.Common.Ensure("Month must be 0 or more", factor > 0.0);
				FinancialFunctions.Common.Ensure("Start period must be 1 or more", startPeriod >= 0.0);
				FinancialFunctions.Common.Ensure("End period must be 1 or more", endPeriod >= 0.0);
				FinancialFunctions.Common.Ensure("If bflag is set to SwitchToStraightLine, then life, startPeriod and endPeriod cannot all have the same value", bflag == VdbSwitch.DontSwitchToStraightLine || life != startPeriod || startPeriod != endPeriod);
				if (bflag == VdbSwitch.DontSwitchToStraightLine)
				{
					return FinancialFunctions.Depreciation.DeprBetweenPeriods(cost, salvage, life, startPeriod, endPeriod, factor, false);
				}
				return FinancialFunctions.Depreciation.DeprBetweenPeriods(cost, salvage, life, startPeriod, endPeriod, factor, true);
			}

			static Func<double, double, bool> FAmorDegrc(double assetLife)
			{
				return (double x1, double x2) => x1 <= assetLife && assetLife <= x2;
			}

			static Func<double, double, double, double> FAmorLinc(double period)
			{
				return delegate(double countedPeriod, double depr, double availDepr)
				{
					while (countedPeriod <= period)
					{
						double num = ((depr <= availDepr) ? depr : availDepr);
						double num2 = num;
						double num3 = availDepr - num2;
						double num4 = ((num3 >= 0.0) ? num3 : 0.0);
						double num5 = num4;
						availDepr = num5;
						depr = num2;
						countedPeriod += 1.0;
					}
					return depr;
				};
			}

			static Func<DateTime, DateTime> FFirstDeprLinc(DayCountBasis basis)
			{
				return delegate(DateTime dateTime)
				{
					Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(dateTime);
					int item = tuple.Item1;
					int item2 = tuple.Item2;
					int item3 = tuple.Item3;
					bool flag = basis == DayCountBasis.ActualActual || basis == DayCountBasis.Actual365;
					bool flag2 = flag && FinancialFunctions.Common.IsLeapYear(dateTime);
					bool flag3 = flag2 && item2 == 2;
					bool flag4 = flag3 && item3 >= 28;
					if (flag4)
					{
						return FinancialFunctions.Common.Date(item, item2, 28);
					}
					return dateTime;
				};
			}
		}

		static class DayCount
		{
			public static FinancialFunctions.DayCount.IDayCounter DayCounterFactory(DayCountBasis basis)
			{
				switch (basis)
				{
				case DayCountBasis.UsPsa30_360:
					return new FinancialFunctions.DayCount.UsPsa30360();
				case DayCountBasis.ActualActual:
					return new FinancialFunctions.DayCount.ActualActual();
				case DayCountBasis.Actual360:
					return new FinancialFunctions.DayCount.Actual360();
				case DayCountBasis.Actual365:
					return new FinancialFunctions.DayCount.Actual365();
				case DayCountBasis.Europ30_360:
					return new FinancialFunctions.DayCount.Europ30360();
				default:
					throw new Exception("Should not get here.");
				}
			}

			static double ActualCoupDays(DateTime settl, DateTime mat, double freq)
			{
				DateTime before = FinancialFunctions.DayCount.FindPreviousCouponDate(settl, mat, freq, DayCountBasis.ActualActual);
				DateTime after = FinancialFunctions.DayCount.FindNextCouponDate(settl, mat, freq, DayCountBasis.ActualActual);
				return (double)FinancialFunctions.Common.Days(after, before);
			}

			public static DateTime ChangeMonth(DateTime orgDate, int numMonths, DayCountBasis basis, bool returnLastDay)
			{
				DateTime result = orgDate.AddMonths(numMonths);
				Tuple<int, int, int> tuple = new Tuple<int, int, int>(result.Year, result.Month, result.Day);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				if (returnLastDay)
				{
					return new DateTime(item, item2, DateTime.DaysInMonth(item, item2));
				}
				return result;
			}

			static bool ConsiderAsBisestile(DateTime d1, DateTime d2)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(d1);
				int item = tuple.Item1;
				Tuple<int, int, int> tuple2 = FinancialFunctions.Common.ToTuple(d2);
				int item2 = tuple2.Item1;
				int item3 = tuple2.Item2;
				int item4 = tuple2.Item3;
				return (item == item2 && FinancialFunctions.Common.IsLeapYear(d1)) || (item3 == 2 && item4 == 29) || FinancialFunctions.DayCount.IsFeb29BetweenConsecutiveYears(d1, d2);
			}

			static bool LessOrEqualToAYearApart(DateTime d1, DateTime d2)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(d1);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				int item3 = tuple.Item3;
				Tuple<int, int, int> tuple2 = FinancialFunctions.Common.ToTuple(d2);
				int item4 = tuple2.Item1;
				int item5 = tuple2.Item2;
				int item6 = tuple2.Item3;
				return item == item4 || (item4 == item + 1 && (item2 > item5 || (item2 == item5 && item3 >= item6)));
			}

			internal static double CoupDays(DateTime settlement, DateTime maturity, Frequency frequency, DayCountBasis basis)
			{
				FinancialFunctions.Common.Ensure("settlement must be before maturity", maturity > settlement);
				FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
				FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
				double num = (double)frequency;
				return dayCounter2.CoupDays(settlement, maturity, num);
			}

			internal static double CoupDaysBS(DateTime settlement, DateTime maturity, Frequency frequency, DayCountBasis basis)
			{
				FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
				return dayCounter.CoupDaysBS(settlement, maturity, (double)frequency);
			}

			internal static double CoupDaysNC(DateTime settlement, DateTime maturity, Frequency frequency, DayCountBasis basis)
			{
				FinancialFunctions.Common.Ensure("settlement must be before maturity", maturity > settlement);
				FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
				return dayCounter.CoupDaysNC(settlement, maturity, (double)frequency);
			}

			internal static DateTime CoupNCD(DateTime settlement, DateTime maturity, Frequency frequency, DayCountBasis basis)
			{
				FinancialFunctions.Common.Ensure("settlement must be before maturity", maturity > settlement);
				FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
				FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
				double num = (double)frequency;
				return dayCounter2.CoupNCD(settlement, maturity, num);
			}

			internal static double CoupNum(DateTime settlement, DateTime maturity, Frequency frequency, DayCountBasis basis)
			{
				FinancialFunctions.Common.Ensure("settlement must be before maturity", maturity > settlement);
				FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
				FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
				double num = (double)frequency;
				return dayCounter2.CoupNum(settlement, maturity, num);
			}

			internal static DateTime CoupPCD(DateTime settlement, DateTime maturity, Frequency frequency, DayCountBasis basis)
			{
				FinancialFunctions.Common.Ensure("settlement must be before maturity", maturity > settlement);
				FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
				FinancialFunctions.DayCount.IDayCounter dayCounter2 = dayCounter;
				double num = (double)frequency;
				return dayCounter2.CoupPCD(settlement, maturity, num);
			}

			static int DateDiff360(int sd, int sm, int sy, int ed, int em, int ey)
			{
				return (ey - sy) * 360 + (em - sm) * 30 + ed - sd;
			}

			static int DateDiff360Eu(DateTime arg2, DateTime arg1)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(arg2);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				int item3 = tuple.Item3;
				Tuple<int, int, int> tuple2 = FinancialFunctions.Common.ToTuple(arg1);
				int item4 = tuple2.Item1;
				int item5 = tuple2.Item2;
				int item6 = tuple2.Item3;
				Tuple<int, int, int, int, int, int, DateTime, Tuple<DateTime>> tuple3 = new Tuple<int, int, int, int, int, int, DateTime, Tuple<DateTime>>(item3, item2, item, item6, item5, item4, arg2, new Tuple<DateTime>(arg1));
				int item7 = tuple3.Item3;
				int item8 = tuple3.Item2;
				int num = tuple3.Item1;
				int item9 = tuple3.Item6;
				int item10 = tuple3.Item5;
				int num2 = tuple3.Item4;
				int num3 = ((num != 31) ? num : 30);
				num = num3;
				int num4 = ((num2 != 31) ? num2 : 30);
				num2 = num4;
				return FinancialFunctions.DayCount.DateDiff360(num, item8, item7, num2, item10, item9);
			}

			public static int DateDiff360US(DateTime startDate, DateTime endDate, FinancialFunctions.DayCount.Method360Us method360)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(startDate);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				int item3 = tuple.Item3;
				Tuple<int, int, int> tuple2 = FinancialFunctions.Common.ToTuple(endDate);
				int item4 = tuple2.Item1;
				int item5 = tuple2.Item2;
				int item6 = tuple2.Item3;
				Tuple<int, int, int, int, int, int, DateTime, Tuple<DateTime>> tuple3 = new Tuple<int, int, int, int, int, int, DateTime, Tuple<DateTime>>(item3, item2, item, item6, item5, item4, startDate, new Tuple<DateTime>(endDate));
				int item7 = tuple3.Item3;
				DateTime item8 = tuple3.Item7;
				int item9 = tuple3.Item2;
				int num = tuple3.Item1;
				int item10 = tuple3.Item6;
				DateTime item11 = tuple3.Rest.Item1;
				int item12 = tuple3.Item5;
				int num2 = tuple3.Item4;
				bool flag = FinancialFunctions.Common.LastDayOfFebruary(item11) && (FinancialFunctions.Common.LastDayOfFebruary(item8) || method360 == FinancialFunctions.DayCount.Method360Us.ModifyBothDates);
				if (flag)
				{
					num2 = 30;
				}
				bool flag2 = num2 == 31 && (num >= 30 || method360 == FinancialFunctions.DayCount.Method360Us.ModifyBothDates);
				if (flag2)
				{
					num2 = 30;
				}
				if (num == 31)
				{
					num = 30;
				}
				if (FinancialFunctions.Common.LastDayOfFebruary(item8))
				{
					num = 30;
				}
				return FinancialFunctions.DayCount.DateDiff360(num, item9, item7, num2, item12, item10);
			}

			static int DateDiff365(DateTime arg2, DateTime arg1)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(arg2);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				int item3 = tuple.Item3;
				Tuple<int, int, int> tuple2 = FinancialFunctions.Common.ToTuple(arg1);
				int item4 = tuple2.Item1;
				int item5 = tuple2.Item2;
				int item6 = tuple2.Item3;
				Tuple<int, int, int, int, int, int, DateTime, Tuple<DateTime>> tuple3 = new Tuple<int, int, int, int, int, int, DateTime, Tuple<DateTime>>(item3, item2, item, item6, item5, item4, arg2, new Tuple<DateTime>(arg1));
				int item7 = tuple3.Item3;
				int item8 = tuple3.Item2;
				int num = tuple3.Item1;
				int item9 = tuple3.Item6;
				int item10 = tuple3.Item5;
				int num2 = tuple3.Item4;
				bool flag = num > 28 && item8 == 2;
				if (flag)
				{
					num = 28;
				}
				bool flag2 = num2 > 28 && item10 == 2;
				if (flag2)
				{
					num2 = 28;
				}
				Tuple<DateTime, DateTime> tuple4 = new Tuple<DateTime, DateTime>(FinancialFunctions.Common.Date(item7, item8, num), FinancialFunctions.Common.Date(item9, item10, num2));
				DateTime item11 = tuple4.Item1;
				DateTime item12 = tuple4.Item2;
				return (item9 - item7) * 365 + FinancialFunctions.Common.Days(item12, item11);
			}

			internal static Tuple<DateTime, DateTime, double> DatesAggregate1(DateTime startDate, DateTime endDate, int numMonths, DayCountBasis basis, Func<DateTime, DateTime, double> f, double acc, bool returnLastMonth)
			{
				Func<DateTime, DateTime, double, Tuple<DateTime, DateTime, double>> func = FinancialFunctions.DayCount.FDatesAggregator(endDate, numMonths, basis, f, returnLastMonth);
				return func(startDate, endDate, acc);
			}

			internal static double YearFrac(DateTime startDate, DateTime endDate, DayCountBasis basis)
			{
				FinancialFunctions.Common.Ensure("startDate must be before endDate", startDate < endDate);
				FinancialFunctions.DayCount.IDayCounter dayCounter = FinancialFunctions.DayCount.DayCounterFactory(basis);
				return dayCounter.DaysBetween(startDate, endDate, NumDenumPosition.Numerator) / dayCounter.DaysInYear(startDate, endDate);
			}

			static Tuple<DateTime, DateTime> FindCouponDates(DateTime settl, DateTime mat, double freq, DayCountBasis basis)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(mat);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				int item3 = tuple.Item3;
				bool returnLastMonth = FinancialFunctions.Common.LastDayOfMonth(item, item2, item3);
				int numMonths = -FinancialFunctions.DayCount.Freq2Months(freq);
				return FinancialFunctions.DayCount.FindPcdNcd(mat, settl, numMonths, basis, returnLastMonth);
			}

			static DateTime FindNextCouponDate(DateTime settl, DateTime mat, double freq, DayCountBasis basis)
			{
				return FinancialFunctions.DayCount.FindCouponDates(settl, mat, freq, basis).Item2;
			}

			internal static Tuple<DateTime, DateTime> FindPcdNcd(DateTime startDate, DateTime endDate, int numMonths, DayCountBasis basis, bool returnLastMonth)
			{
				Tuple<DateTime, DateTime, double> tuple = FinancialFunctions.DayCount.DatesAggregate1(startDate, endDate, numMonths, basis, FinancialFunctions.DayCount.FPcdNcd, 0.0, returnLastMonth);
				DateTime item = tuple.Item1;
				DateTime item2 = tuple.Item2;
				return new Tuple<DateTime, DateTime>(item, item2);
			}

			static DateTime FindPreviousCouponDate(DateTime settl, DateTime mat, double freq, DayCountBasis basis)
			{
				return FinancialFunctions.DayCount.FindCouponDates(settl, mat, freq, basis).Item1;
			}

			internal static int Freq2Months(double freq)
			{
				return 12 / (int)freq;
			}

			static bool IsFeb29BetweenConsecutiveYears(DateTime arg2, DateTime arg1)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(arg2);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				Tuple<int, int, int> tuple2 = FinancialFunctions.Common.ToTuple(arg1);
				int item3 = tuple2.Item1;
				int item4 = tuple2.Item2;
				if (item == item3 && FinancialFunctions.Common.IsLeapYear(arg2))
				{
					return item2 <= 2 && item4 > 2;
				}
				if (item == item3)
				{
					return false;
				}
				if (item3 != item + 1)
				{
					throw new Exception("isFeb29BetweenConsecutiveYears: function called with non consecutive years");
				}
				if (!FinancialFunctions.Common.IsLeapYear(arg2))
				{
					return FinancialFunctions.Common.IsLeapYear(arg1) && item4 > 2;
				}
				return item2 <= 2;
			}

			static double NumberOfCoupons(DateTime settl, DateTime mat, double freq, DayCountBasis basis)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(mat);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				DateTime d = FinancialFunctions.DayCount.FindPreviousCouponDate(settl, mat, freq, basis);
				Tuple<int, int, int> tuple2 = FinancialFunctions.Common.ToTuple(d);
				int item3 = tuple2.Item1;
				int item4 = tuple2.Item2;
				double num = (double)((item - item3) * 12 + item2 - item4);
				return num * freq / 12.0;
			}

			static Func<DateTime, DateTime, double, Tuple<DateTime, DateTime, double>> FDatesAggregator(DateTime endDate, int numMonths, DayCountBasis basis, Func<DateTime, DateTime, double> f, bool returnLastMonth)
			{
				return delegate(DateTime frontDate, DateTime trailingDate, double acc)
				{
					while (!((numMonths <= 0) ? (frontDate <= endDate) : (frontDate >= endDate)))
					{
						DateTime dateTime = frontDate;
						DateTime dateTime2 = FinancialFunctions.DayCount.ChangeMonth(frontDate, numMonths, basis, returnLastMonth);
						double num = acc + f(dateTime2, dateTime);
						acc = num;
						trailingDate = dateTime;
						frontDate = dateTime2;
					}
					return new Tuple<DateTime, DateTime, double>(frontDate, trailingDate, acc);
				};
			}

			static readonly Func<DateTime, DateTime, double> FPcdNcd = (DateTime d1, DateTime d2) => 0.0;

			public enum Method360Us
			{
				ModifyStartDate,
				ModifyBothDates
			}

			public interface IDayCounter
			{
				DateTime ChangeMonth(DateTime dateTime, int num, bool flag);

				double CoupDays(DateTime settlement, DateTime maturity, double num);

				double CoupDaysBS(DateTime settlement, DateTime maturity, double num);

				double CoupDaysNC(DateTime settlement, DateTime maturity, double num);

				DateTime CoupNCD(DateTime settlement, DateTime maturity, double num);

				double CoupNum(DateTime settlement, DateTime maturity, double num);

				DateTime CoupPCD(DateTime settlement, DateTime maturity, double num);

				double DaysBetween(DateTime from, DateTime t, NumDenumPosition numDenumPosition);

				double DaysInYear(DateTime a, DateTime b);
			}

			sealed class UsPsa30360 : FinancialFunctions.DayCount.IDayCounter
			{
				public DateTime ChangeMonth(DateTime dateTime, int num, bool flag)
				{
					return FinancialFunctions.DayCount.ChangeMonth(dateTime, num, DayCountBasis.UsPsa30_360, flag);
				}

				public double CoupDays(DateTime settlement, DateTime maturity, double num)
				{
					return 360.0 / num;
				}

				public double CoupDaysBS(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.DayCount.DateDiff360US(this.CoupPCD(settlement, maturity, num), settlement, FinancialFunctions.DayCount.Method360Us.ModifyStartDate);
				}

				public double CoupDaysNC(DateTime settlement, DateTime maturity, double num)
				{
					DateTime startDate = FinancialFunctions.DayCount.FindPreviousCouponDate(settlement, maturity, num, DayCountBasis.UsPsa30_360);
					DateTime endDate = FinancialFunctions.DayCount.FindNextCouponDate(settlement, maturity, num, DayCountBasis.UsPsa30_360);
					int num2 = FinancialFunctions.DayCount.DateDiff360US(startDate, endDate, FinancialFunctions.DayCount.Method360Us.ModifyBothDates);
					int num3 = FinancialFunctions.DayCount.DateDiff360US(startDate, settlement, FinancialFunctions.DayCount.Method360Us.ModifyStartDate);
					return (double)(num2 - num3);
				}

				public DateTime CoupNCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindNextCouponDate(settlement, maturity, num, DayCountBasis.UsPsa30_360);
				}

				public double CoupNum(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.NumberOfCoupons(settlement, maturity, num, DayCountBasis.UsPsa30_360);
				}

				public DateTime CoupPCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindPreviousCouponDate(settlement, maturity, num, DayCountBasis.UsPsa30_360);
				}

				public double DaysBetween(DateTime settlement, DateTime maturity, NumDenumPosition numDenumPosition)
				{
					return (double)FinancialFunctions.DayCount.DateDiff360US(settlement, maturity, FinancialFunctions.DayCount.Method360Us.ModifyStartDate);
				}

				public double DaysInYear(DateTime a, DateTime b)
				{
					return 360.0;
				}
			}

			sealed class Actual360 : FinancialFunctions.DayCount.IDayCounter
			{
				public DateTime ChangeMonth(DateTime dateTime, int num, bool flag)
				{
					return FinancialFunctions.DayCount.ChangeMonth(dateTime, num, DayCountBasis.Actual360, flag);
				}

				public double CoupDays(DateTime settlement, DateTime maturity, double num)
				{
					return 360.0 / num;
				}

				public double CoupDaysBS(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.Common.Days(settlement, this.CoupPCD(settlement, maturity, num));
				}

				public double CoupDaysNC(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.Common.Days(this.CoupNCD(settlement, maturity, num), settlement);
				}

				public DateTime CoupNCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindNextCouponDate(settlement, maturity, num, DayCountBasis.Actual360);
				}

				public double CoupNum(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.NumberOfCoupons(settlement, maturity, num, DayCountBasis.Actual360);
				}

				public DateTime CoupPCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindPreviousCouponDate(settlement, maturity, num, DayCountBasis.Actual360);
				}

				public double DaysBetween(DateTime issue, DateTime settlement, NumDenumPosition numDenumPosition)
				{
					return (double)((numDenumPosition == NumDenumPosition.Numerator) ? FinancialFunctions.Common.Days(settlement, issue) : FinancialFunctions.DayCount.DateDiff360US(settlement, issue, FinancialFunctions.DayCount.Method360Us.ModifyStartDate));
				}

				public double DaysInYear(DateTime a, DateTime b)
				{
					return 360.0;
				}
			}

			sealed class Actual365 : FinancialFunctions.DayCount.IDayCounter
			{
				public DateTime ChangeMonth(DateTime dateTime, int num, bool flag)
				{
					return FinancialFunctions.DayCount.ChangeMonth(dateTime, num, DayCountBasis.Actual365, flag);
				}

				public double CoupDays(DateTime settlement, DateTime maturity, double num)
				{
					return 365.0 / num;
				}

				public double CoupDaysBS(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.Common.Days(settlement, this.CoupPCD(settlement, maturity, num));
				}

				public double CoupDaysNC(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.Common.Days(this.CoupNCD(settlement, maturity, num), settlement);
				}

				public DateTime CoupNCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindNextCouponDate(settlement, maturity, num, DayCountBasis.Actual365);
				}

				public double CoupNum(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.NumberOfCoupons(settlement, maturity, num, DayCountBasis.Actual365);
				}

				public DateTime CoupPCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindPreviousCouponDate(settlement, maturity, num, DayCountBasis.Actual365);
				}

				public double DaysBetween(DateTime maturity, DateTime settlement, NumDenumPosition numDenumPosition)
				{
					return (double)((numDenumPosition == NumDenumPosition.Numerator) ? FinancialFunctions.Common.Days(settlement, maturity) : FinancialFunctions.DayCount.DateDiff365(maturity, settlement));
				}

				public double DaysInYear(DateTime settlement, DateTime dateTime)
				{
					return 365.0;
				}
			}

			sealed class ActualActual : FinancialFunctions.DayCount.IDayCounter
			{
				public DateTime ChangeMonth(DateTime dateTime, int num, bool flag)
				{
					return FinancialFunctions.DayCount.ChangeMonth(dateTime, num, DayCountBasis.ActualActual, flag);
				}

				public double CoupDays(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.ActualCoupDays(settlement, maturity, num);
				}

				public double CoupDaysBS(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.Common.Days(settlement, this.CoupPCD(settlement, maturity, num));
				}

				public double CoupDaysNC(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.Common.Days(this.CoupNCD(settlement, maturity, num), settlement);
				}

				public DateTime CoupNCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindNextCouponDate(settlement, maturity, num, DayCountBasis.ActualActual);
				}

				public double CoupNum(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.NumberOfCoupons(settlement, maturity, num, DayCountBasis.ActualActual);
				}

				public DateTime CoupPCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindPreviousCouponDate(settlement, maturity, num, DayCountBasis.ActualActual);
				}

				public double DaysBetween(DateTime startDate, DateTime endDate, NumDenumPosition numDenumPosition)
				{
					return (double)FinancialFunctions.Common.Days(endDate, startDate);
				}

				public double DaysInYear(DateTime a, DateTime b)
				{
					if (FinancialFunctions.DayCount.LessOrEqualToAYearApart(a, b))
					{
						return (double)((!FinancialFunctions.DayCount.ConsiderAsBisestile(a, b)) ? 365 : 366);
					}
					int num = FinancialFunctions.Common.Days(FinancialFunctions.Common.Date(a.Year + 1, 1, 1), FinancialFunctions.Common.Date(b.Year, 1, 1));
					return (double)num / (double)(a.Year - b.Year + 1);
				}
			}

			sealed class Europ30360 : FinancialFunctions.DayCount.IDayCounter
			{
				public DateTime ChangeMonth(DateTime settlement, int num, bool flag)
				{
					return FinancialFunctions.DayCount.ChangeMonth(settlement, num, DayCountBasis.Europ30_360, flag);
				}

				public double CoupDays(DateTime settlement, DateTime maturity, double num)
				{
					return 360.0 / num;
				}

				public double CoupDaysBS(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.DayCount.DateDiff360Eu(this.CoupPCD(settlement, maturity, num), settlement);
				}

				public double CoupDaysNC(DateTime settlement, DateTime maturity, double num)
				{
					return (double)FinancialFunctions.DayCount.DateDiff360Eu(settlement, this.CoupNCD(settlement, maturity, num));
				}

				public DateTime CoupNCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindNextCouponDate(settlement, maturity, num, DayCountBasis.Europ30_360);
				}

				public double CoupNum(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.NumberOfCoupons(settlement, maturity, num, DayCountBasis.Europ30_360);
				}

				public DateTime CoupPCD(DateTime settlement, DateTime maturity, double num)
				{
					return FinancialFunctions.DayCount.FindPreviousCouponDate(settlement, maturity, num, DayCountBasis.Europ30_360);
				}

				public double DaysBetween(DateTime settlement, DateTime maturity, NumDenumPosition numDenumPosition)
				{
					return (double)FinancialFunctions.DayCount.DateDiff360Eu(settlement, maturity);
				}

				public double DaysInYear(DateTime settlement, DateTime maturity)
				{
					return 360.0;
				}
			}
		}

		static class Common
		{
			internal static DateTime AddYears(DateTime d, int n)
			{
				return d.AddYears(n);
			}

			public static T AggrBetween<T>(int start, int end, Func<T, int, T> collector, T seed)
			{
				IEnumerable<int> source = Range.Create(start, end);
				return source.Aggregate(seed, collector);
			}

			public static Tuple<int, int, int> ToTuple(DateTime d1)
			{
				return new Tuple<int, int, int>(d1.Year, d1.Month, d1.Day);
			}

			internal static bool AreEqual(double x, double y)
			{
				return Math.Abs(x - y) < 1E-08;
			}

			static double Bisection(Func<double, double> f, double a, double b, int count, double precision)
			{
				Func<Func<double, double>, double, double, int, double, double> func = FinancialFunctions.Common.FBisection(200);
				return func(f, a, b, count, precision);
			}

			internal static double Ceiling(double x)
			{
				return Math.Ceiling(x);
			}

			internal static DateTime Date(int y, int m, int d)
			{
				return new DateTime(y, m, d);
			}

			internal static int Days(DateTime after, DateTime before)
			{
				return (after - before).Days;
			}

			internal static int DaysOfMonth(int y, int m)
			{
				return DateTime.DaysInMonth(y, m);
			}

			internal static void Ensure(string s, bool c)
			{
				if (c)
				{
					return;
				}
				throw new Exception(s);
			}

			static Tuple<double, double> FindBounds(Func<double, double> f, double guess, double minBound, double maxBound, double precision)
			{
				if (guess <= minBound || guess >= maxBound)
				{
					throw new Exception(string.Format("guess needs to be between {0} and {1}", minBound, maxBound));
				}
				Func<double, double> func = FinancialFunctions.Common.FAdjustMin(minBound, precision);
				Func<double, double> func2 = FinancialFunctions.Common.FAdjustMax(maxBound, precision);
				FinancialFunctions.Common.FindBoundsWorker findBoundsWorker = new FinancialFunctions.Common.FindBoundsWorker(f, 1.6, 60, new Func<double, double>(func.Invoke), new Func<double, double>(func2.Invoke));
				double low = func(guess - 0.01);
				double up = func2(guess + 0.01);
				return findBoundsWorker.Invoke(low, up, 60);
			}

			internal static double FindRoot(Func<double, double> f, double guess)
			{
				double? num = FinancialFunctions.Common.Newton(f, guess, 0, 1E-07);
				if (num != null && FinancialFunctions.Common.Sign(guess) == FinancialFunctions.Common.Sign(num.Value))
				{
					return num.Value;
				}
				Tuple<double, double> tuple = FinancialFunctions.Common.FindBounds(f, guess, -1.0, double.MaxValue, 1E-07);
				double item = tuple.Item2;
				double item2 = tuple.Item1;
				return FinancialFunctions.Common.Bisection(f, item2, item, 0, 1E-07);
			}

			internal static double Floor(double x)
			{
				return Math.Floor(x);
			}

			internal static bool IsLeapYear(DateTime d)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(d);
				int item = tuple.Item1;
				return DateTime.IsLeapYear(item);
			}

			internal static bool LastDayOfFebruary(DateTime d)
			{
				Tuple<int, int, int> tuple = FinancialFunctions.Common.ToTuple(d);
				int item = tuple.Item1;
				int item2 = tuple.Item2;
				int item3 = tuple.Item3;
				return item2 == 2 && FinancialFunctions.Common.LastDayOfMonth(item, item2, item3);
			}

			internal static bool LastDayOfMonth(int y, int m, int d)
			{
				return DateTime.DaysInMonth(y, m) == d;
			}

			internal static double Ln(double x)
			{
				return Math.Log(x);
			}

			internal static double Log10(double x)
			{
				return Math.Log(x, 10.0);
			}

			internal static double Min(double x, double y)
			{
				return Math.Min(x, y);
			}

			static double? Newton(Func<double, double> f, double x, int count, double precision)
			{
				Func<Func<double, double>, double, int, double, double?> func = FinancialFunctions.Common.FNewtonIterator(20);
				return func(f, x, count, precision);
			}

			internal static double Pow(double x, double y)
			{
				return Math.Pow(x, y);
			}

			internal static bool Raisable(double b, double p)
			{
				return 1.0 + b >= 0.0 || Math.Abs(p - (double)((int)p)) <= 1E-08;
			}

			internal static double Rest(double x)
			{
				return x - (double)((int)x);
			}

			internal static double Round(bool excelComplaint, double x)
			{
				if (!excelComplaint)
				{
					return Math.Round(x, MidpointRounding.AwayFromZero);
				}
				double value = Math.Round(x, 13, MidpointRounding.AwayFromZero);
				return Math.Round(value, MidpointRounding.AwayFromZero);
			}

			internal static int Sign(double x)
			{
				return Math.Sign(x);
			}

			internal static double Sqr(double x)
			{
				return Math.Sqrt(x);
			}

			static Func<double, double> FAdjustMin(double minBound, double precision)
			{
				return delegate(double value)
				{
					if (value <= minBound)
					{
						return minBound + precision;
					}
					return value;
				};
			}

			static Func<double, double> FAdjustMax(double maxBound, double precision)
			{
				return delegate(double value)
				{
					if (value >= maxBound)
					{
						return maxBound - precision;
					}
					return value;
				};
			}

			static Func<Func<double, double>, double, double> Derivative(double precision)
			{
				return (Func<double, double> f, double x) => (f(x + precision) - f(x - precision)) / (2.0 * precision);
			}

			static Func<Func<double, double>, double, int, double, double?> FNewtonIterator(int maxIterations = 20)
			{
				return delegate(Func<double, double> f, double x, int count, double precision)
				{
					double num3;
					for (;;)
					{
						Func<Func<double, double>, double, double> func = FinancialFunctions.Common.Derivative(precision);
						double num = f(x);
						double num2 = func(f, x);
						num3 = x - num / num2;
						if (Math.Abs(num3 - x) < precision)
						{
							break;
						}
						if (count > maxIterations)
						{
							goto IL_45;
						}
						count++;
						x = num3;
					}
					return new double?(num3);
					IL_45:
					return null;
				};
			}

			static Func<Func<double, double>, double, double, int, double, double> FBisection(int maxIterations = 200)
			{
				return delegate(Func<double, double> f, double a, double b, int count, double precision)
				{
					while (a != b)
					{
						double num = f(a);
						if (Math.Abs(num) < precision)
						{
							return a;
						}
						double num2 = f(b);
						if (Math.Abs(num2) < precision)
						{
							return b;
						}
						int num3 = count + 1;
						if (num3 > maxIterations)
						{
							throw new Exception(string.Format("No root found in {0} iterations", maxIterations));
						}
						if (num * num2 > 0.0)
						{
							throw new Exception(string.Format("({0},{1}) don't bracket the root", a, b));
						}
						double num4 = a + 0.5 * (b - a);
						double num5 = f(num4);
						if (Math.Abs(num5) < precision)
						{
							return num4;
						}
						if (num * num5 >= 0.0)
						{
							if (num * num5 <= 0.0)
							{
								throw new Exception("Bisection: It should never get here");
							}
							count = num3;
							a = num4;
						}
						else
						{
							count = num3;
							b = num4;
						}
					}
					throw new Exception(string.Format("(a=b={0}) impossible to start Bisection", a));
				};
			}

			class FindBoundsWorker
			{
				public FindBoundsWorker(Func<double, double> f, double factor, int maxTries, Func<double, double> adjValueToMin, Func<double, double> adjValueToMax)
				{
					this.f = f;
					this.factor = factor;
					this.maxTries = maxTries;
					this.adjValueToMin = adjValueToMin;
					this.adjValueToMax = adjValueToMax;
				}

				public Tuple<double, double> Invoke(double low, double up, int tries)
				{
					double num2;
					double num3;
					for (;;)
					{
						int num = tries - 1;
						if (num == 0)
						{
							break;
						}
						num2 = this.adjValueToMin(low);
						num3 = this.adjValueToMax(up);
						double num4 = this.f(num2);
						double num5 = this.f(num3);
						if (Math.Abs(num4 * num5) < 1E-08)
						{
							goto Block_1;
						}
						if (num4 * num5 < 0.0)
						{
							goto Block_2;
						}
						if (num4 * num5 <= 0.0)
						{
							goto IL_C9;
						}
						tries = num;
						up = num3 + this.factor * (num3 - num2);
						low = num2 + this.factor * (num2 - num3);
					}
					throw new Exception(string.Format("Not found an interval comprising the root after {0} tries, last tried was ({1}, {2})", this.maxTries, low, up));
					Block_1:
					return new Tuple<double, double>(num2, num3);
					Block_2:
					return new Tuple<double, double>(num2, num3);
					IL_C9:
					throw new Exception(string.Format("FindBounds: one of the values {0}, {1}) cannot be used to evaluate the objective function", num2, num3));
				}

				readonly Func<double, double> adjValueToMax;

				readonly Func<double, double> adjValueToMin;

				readonly Func<double, double> f;

				readonly double factor;

				readonly int maxTries;
			}
		}
	}
}
