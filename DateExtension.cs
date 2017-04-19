using System;

namespace WinToolkit
{
	public class DateFormatFromTo
	{
		public DateTime DtFrom { get; set; }
		public DateTime DtTo { get; set; }

	}
	public static class DateTimeExtension
	{
		public static bool IsDateRangeInsideDateRange(object startDateOne, object endDateOne, object startDateTwo, object endDateTwo)
		{
			DateTime s1 = Convert.ToDateTime(startDateOne);
			DateTime e1 = Convert.ToDateTime(endDateOne);
			DateTime s2 = Convert.ToDateTime(startDateTwo);
			DateTime e2 = Convert.ToDateTime(endDateTwo);
			//DateOne inside DateTwo??
			return s1 >= s2 && e1 <= e2;
		}

		public static DateTime TrimDatePart(object timeobj)
		{
			DateTime time = Convert.ToDateTime(timeobj);
			return new DateTime(2005, 05, 05, time.Hour, time.Minute, time.Second);
		}

		public static DateFormatFromTo GetDateFormatFromTo(DateTime from, DateTime to)
		{
			bool oneDayAdded = false;
			if (DiffMinutes(to, from) < 0)
			{
				to = to.AddDays(1);
				oneDayAdded = true;
			}
			from = BuildDateTime("2000/01/01", from.Hour, from.Minute);
			to = BuildDateTime((!oneDayAdded) ? "2000/01/01" : "2000/01/02", to.Hour, to.Minute);
			return new DateFormatFromTo() { DtFrom = from, DtTo = to };
		}

		public static string ToString_IncludingMilliseconds(this DateTime dt)
		{
			return dt.ToString("dd/MM/yyyy hh:mm:ss.fff tt");
		}

		public static Object TrimSecondsAndMilliSeconds(this Object dt)
		{
			if (dt == null)
				return null;
			DateTime tmpDate = Convert.ToDateTime(dt);
			return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, tmpDate.Hour, tmpDate.Minute, 0, 0);
		}

		public static string ToString_DayMonthYear(this DateTime dt)
		{
			return dt.ToString("dd/MM/yyyy");
		}

		public static string ToString_DMY_HMS(this DateTime dt)
		{
			return dt.ToString("yyyyMMdd-HHmmss");
		}
		public static string ToString_YMDHMSF(this DateTime dt)
		{
			return dt.ToString("yyyyMMddHHmmssfff");
		}
		/// <summary>
		/// Converts the input date in a printable format (dd/MM/yyyy   hh:mm tt)
		/// </summary>
		/// <returns></returns>
		public static string ToString_PrintableFormat(this DateTime obj)
		{
			return obj.ToString("dd/MM/yyyy   hh:mm tt");
		}

		public static bool IsGreaterThan(this DateTime dt, DateTime dateTime)
		{
			return DateTime.Compare(dt, dateTime) > 0;
		}

		public static bool IsGreaterThanOrEqual(this DateTime dt, DateTime dateTime)
		{
			return DateTime.Compare(dt, dateTime) >= 0;
		}

		public static bool IsNewerThan(this DateTime dt, DateTime dateTime)
		{
			return IsGreaterThan(dt, dateTime);
		}

		public static bool IsLaterThan(this DateTime dt, DateTime dateTime)
		{
			return IsGreaterThan(dt, dateTime);
		}

		public static bool IsLessThan(this DateTime dt, DateTime dateTime)
		{
			return DateTime.Compare(dt, dateTime) < 0;
		}

		public static bool IsLessThanOrEqual(this DateTime dt, DateTime dateTime)
		{
			return DateTime.Compare(dt, dateTime) <= 0;
		}

		public static bool IsOlderThan(this DateTime dt, DateTime dateTime)
		{
			return IsLessThan(dt, dateTime);
		}
		public static bool IsEarlierThan(this DateTime dt, DateTime dateTime)
		{
			return IsLessThan(dt, dateTime);
		}
		public static bool IsEqualTo(this DateTime dt, DateTime dateTime)
		{
			return DateTime.Compare(dt, dateTime) == 0;
		}

		public static double DiffSeconds(this DateTime dt, DateTime dateTime)
		{
			return dt.Subtract(dateTime).TotalSeconds;
		}

		public static double DiffMilliSeconds(this DateTime dt, DateTime dateTime)
		{
			return dt.Subtract(dateTime).TotalMilliseconds;
		}

		public static double DiffMinutes(this DateTime dt, DateTime dateTime)
		{
			return dt.Subtract(dateTime).TotalMinutes;
		}

		public static double DiffHours(this DateTime dt, DateTime dateTime)
		{
			return dt.Subtract(dateTime).TotalHours;
		}

		public static double DiffDays(this DateTime dt, DateTime dateTime)
		{
			return dt.Subtract(dateTime).TotalDays;
		}

		public static double DiffMonths(this DateTime dt, DateTime dateTime)
		{
			return Math.Round(dt.Subtract(dateTime).TotalDays / 30);
		}

		public static int DiffWholeDays(this DateTime dt, DateTime dateTime)
		{
			return Convert.ToInt32(dt.EndOfDayDateTime().Subtract(dateTime.StartOfDayDateTime()).TotalDays);
		}

		public static DateTime StartOfYear(this DateTime obj)
		{
			return new DateTime(Convert.ToDateTime(obj).Year, 1, 1);
		}

		public static DateTime EndOfYear(this DateTime obj)
		{
			return new DateTime(Convert.ToDateTime(obj).Year, 12, 31);
		}

		public static Object StartOfDayDateTime(this Object obj)
		{
			if (!(obj is DateTime)) return null;
			return StartOfDayDateTime((DateTime)obj);
		}

		public static Object EndOfDayDateTime(this Object obj)
		{
			if (!(obj is DateTime)) return null;
			return EndOfDayDateTime((DateTime)obj);
		}

		public static DateTime StartOfDayDateTime(this DateTime dateTime)
		{
			return dateTime.Date;
		}

		public static DateTime EndOfDayDateTime(this DateTime dateTime)
		{
			DateTime endOfDay = dateTime.Date;
			return endOfDay.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
		}

		public static DateTime BuildDateTime(string date, int hour, int minute)
		{
			DateTime timeObject = new DateTime(DateTime.Parse(date).Year, DateTime.Parse(date).Month, DateTime.Parse(date).Day);
			timeObject = timeObject.AddHours(hour);
			timeObject = timeObject.AddMinutes(minute);
			return timeObject;
		}

		public static DateTime BuildDateTimeFromParts(int? yearsPart, int? monthsPart, int? daysPart, int? hoursPart, int? minutesPart, int? secondsPart)
		{
			DateTime defaulDateTime = default(DateTime);
			int years = yearsPart ?? defaulDateTime.Year;
			int months = monthsPart ?? defaulDateTime.Month;
			int days = daysPart ?? defaulDateTime.Day;
			int hours = hoursPart ?? defaulDateTime.Hour;
			int minutes = minutesPart ?? defaulDateTime.Minute;
			int seconds = secondsPart ?? defaulDateTime.Second;

			return new DateTime(years, months, days, hours, minutes, seconds);
		}

		public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			int diff = dt.DayOfWeek - startOfWeek;
			if (diff < 0)
			{
				diff += 7;
			}

			return dt.AddDays(-1 * diff).Date;
		}

		public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			return StartOfWeek(dt, startOfWeek).AddDays(7).Date;
		}


		#region Intersection between Dates
		public static bool IsCompletelyEqual(DateTime firstfromDate, DateTime firstToDate, DateTime secondFromDate, DateTime secondToDate, DateTimeTypesOfCompare compareType = DateTimeTypesOfCompare.DateTime)
		{
			if (compareType == DateTimeTypesOfCompare.DateOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).Date) == 0 && Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).Date) == 0;
			if (compareType == DateTimeTypesOfCompare.TimeOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).TimeOfDay == Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).TimeOfDay && Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).TimeOfDay == Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).TimeOfDay;

			return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds())) == 0 && Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds())) == 0;
		}
		public static bool IsFirstCompletelyIntersectWithSecond(DateTime firstfromDate, DateTime firstToDate, DateTime secondFromDate, DateTime secondToDate, DateTimeTypesOfCompare compareType = DateTimeTypesOfCompare.DateTime)
		{
			if (compareType == DateTimeTypesOfCompare.DateOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).Date) > 0
					&& Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).Date) < 0;
			if (compareType == DateTimeTypesOfCompare.TimeOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).TimeOfDay >
					   Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).TimeOfDay
					   &&
					   Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).TimeOfDay <
					   Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).TimeOfDay;

			return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds())) > 0 && Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds())) < 0;
		}
		public static bool IsFirstIntersectAtStartWithSecond(DateTime firstfromDate, DateTime firstToDate, DateTime secondFromDate, DateTime secondToDate, DateTimeTypesOfCompare compareType = DateTimeTypesOfCompare.DateTime)
		{
			if (compareType == DateTimeTypesOfCompare.DateOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).Date) == 0
					&& Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).Date) < 0;
			if (compareType == DateTimeTypesOfCompare.TimeOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).TimeOfDay ==
					   Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).TimeOfDay
					   &&
					   Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).TimeOfDay <
					   Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).TimeOfDay;


			return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds())) == 0 && Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds())) < 0;
		}
		public static bool IsFirstIntersectAtEndWithSecond(DateTime firstfromDate, DateTime firstToDate, DateTime secondFromDate, DateTime secondToDate, DateTimeTypesOfCompare compareType = DateTimeTypesOfCompare.DateTime)
		{
			if (compareType == DateTimeTypesOfCompare.DateOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).Date) > 0
					&& Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).Date) == 0;
			if (compareType == DateTimeTypesOfCompare.TimeOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).TimeOfDay > Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).TimeOfDay
					&& Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).TimeOfDay == Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).TimeOfDay;

			return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds())) > 0 && Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds())) == 0;
		}
		public static bool IsFirstIntersectAtMiddelWithSecond(DateTime firstfromDate, DateTime firstToDate, DateTime secondFromDate, DateTime secondToDate, DateTimeTypesOfCompare compareType = DateTimeTypesOfCompare.DateTime)
		{
			if (compareType == DateTimeTypesOfCompare.DateOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).Date) < 0
				&& Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).Date) < 0 &&
				   Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).Date) > 0;

			if (compareType == DateTimeTypesOfCompare.TimeOnly)
				return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).TimeOfDay < Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).TimeOfDay
			&& Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).TimeOfDay < Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds()).TimeOfDay &&
			   Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).TimeOfDay > Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds()).TimeOfDay;

			return Convert.ToDateTime(firstfromDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds())) < 0
				&& Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondToDate.TrimSecondsAndMilliSeconds())) < 0 &&
				   Convert.ToDateTime(firstToDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondFromDate.TrimSecondsAndMilliSeconds())) > 0;
		}
		public static bool IsDateRangeCompletelyCovered(DateTime rangeStart, DateTime rangeEnd, DateTime itemStart, Object itemEnd, DateTimeTypesOfCompare compareType = DateTimeTypesOfCompare.DateTime)
		{
			if (compareType == DateTimeTypesOfCompare.DateOnly)
				return Convert.ToDateTime(itemStart.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(rangeStart.TrimSecondsAndMilliSeconds()).Date) <= 0
					&& (itemEnd == null || Convert.ToDateTime(itemEnd.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(rangeEnd.TrimSecondsAndMilliSeconds()).Date) >= 0);

			if (compareType == DateTimeTypesOfCompare.TimeOnly)
				return Convert.ToDateTime(itemStart.TrimSecondsAndMilliSeconds()).TimeOfDay <= Convert.ToDateTime(rangeStart.TrimSecondsAndMilliSeconds()).TimeOfDay
					&& (itemEnd == null || Convert.ToDateTime(itemEnd.TrimSecondsAndMilliSeconds()).TimeOfDay >= Convert.ToDateTime(rangeEnd.TrimSecondsAndMilliSeconds()).TimeOfDay);


			return Convert.ToDateTime(itemStart.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(rangeStart.TrimSecondsAndMilliSeconds())) <= 0 && (itemEnd == null || Convert.ToDateTime(itemEnd.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(rangeEnd.TrimSecondsAndMilliSeconds())) >= 0);
		}
		public static Object ReturnGreat(Object firstDate, Object secondDate, DateTimeTypesOfCompare compareType = DateTimeTypesOfCompare.DateTime)
		{
			if (firstDate == null && secondDate == null)
				return null;
			if (firstDate == null)
				return secondDate;
			if (secondDate == null)
				return firstDate;

			if (compareType == DateTimeTypesOfCompare.DateOnly)
				return Convert.ToDateTime(firstDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondDate.TrimSecondsAndMilliSeconds()).Date) >= 1 ? firstDate.TrimSecondsAndMilliSeconds() : secondDate.TrimSecondsAndMilliSeconds();

			if (compareType == DateTimeTypesOfCompare.TimeOnly)
				return Convert.ToDateTime(firstDate.TrimSecondsAndMilliSeconds()).TimeOfDay >= Convert.ToDateTime(secondDate.TrimSecondsAndMilliSeconds()).TimeOfDay ? firstDate.TrimSecondsAndMilliSeconds() : secondDate.TrimSecondsAndMilliSeconds();

			return Convert.ToDateTime(firstDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondDate.TrimSecondsAndMilliSeconds())) >= 1 ? firstDate.TrimSecondsAndMilliSeconds() : secondDate.TrimSecondsAndMilliSeconds();
		}
		public static Object ReturnSmall(Object firstDate, Object secondDate, DateTimeTypesOfCompare compareType = DateTimeTypesOfCompare.DateTime)
		{
			if (firstDate == null && secondDate == null)
				return null;
			if (firstDate == null)
				return secondDate;
			if (secondDate == null)
				return firstDate;

			if (compareType == DateTimeTypesOfCompare.DateOnly)
				return Convert.ToDateTime(firstDate.TrimSecondsAndMilliSeconds()).Date.CompareTo(Convert.ToDateTime(secondDate.TrimSecondsAndMilliSeconds()).Date) <= 1 ? firstDate.TrimSecondsAndMilliSeconds() : secondDate.TrimSecondsAndMilliSeconds();

			if (compareType == DateTimeTypesOfCompare.TimeOnly)
				return Convert.ToDateTime(firstDate.TrimSecondsAndMilliSeconds()).TimeOfDay <= Convert.ToDateTime(secondDate.TrimSecondsAndMilliSeconds()).TimeOfDay ? firstDate.TrimSecondsAndMilliSeconds() : secondDate.TrimSecondsAndMilliSeconds();

			return Convert.ToDateTime(firstDate.TrimSecondsAndMilliSeconds()).CompareTo(Convert.ToDateTime(secondDate.TrimSecondsAndMilliSeconds())) <= 1 ? firstDate.TrimSecondsAndMilliSeconds() : secondDate.TrimSecondsAndMilliSeconds();
		}

		public static RelationType DeduceRelationTypeTo(this DateTime dt, DateTime comparedTo, DateTimeTypesOfCompare type)
		{
			RelationType timeCompareResult;
			RelationType dateCompareResult;

			if (dt.Hour > comparedTo.Hour) timeCompareResult = RelationType.LaterThan;
			else if (dt.Hour < comparedTo.Hour) timeCompareResult = RelationType.EarlierThan;
			else
			{
				if (dt.Minute > comparedTo.Minute) timeCompareResult = RelationType.LaterThan;
				else if (dt.Minute < comparedTo.Minute) timeCompareResult = RelationType.EarlierThan;
				else
				{
					if (dt.Second > comparedTo.Second) timeCompareResult = RelationType.LaterThan;
					else if (dt.Second < comparedTo.Second) timeCompareResult = RelationType.EarlierThan;
					else timeCompareResult = RelationType.Equal;
				}
			}

			if (dt.Year > comparedTo.Year) dateCompareResult = RelationType.LaterThan;
			else if (dt.Year < comparedTo.Year) dateCompareResult = RelationType.EarlierThan;
			else
			{
				if (dt.Month > comparedTo.Month) dateCompareResult = RelationType.LaterThan;
				else if (dt.Month < comparedTo.Month) dateCompareResult = RelationType.EarlierThan;
				else
				{
					if (dt.Day > comparedTo.Day) dateCompareResult = RelationType.LaterThan;
					else if (dt.Day < comparedTo.Day) dateCompareResult = RelationType.EarlierThan;
					else dateCompareResult = RelationType.Equal;
				}
			}

			switch (type)
			{
				case DateTimeTypesOfCompare.DateOnly: return dateCompareResult;
				case DateTimeTypesOfCompare.TimeOnly: return timeCompareResult;
				case DateTimeTypesOfCompare.DateTime: return dateCompareResult == RelationType.Equal ? timeCompareResult : dateCompareResult;
				default: return default(RelationType);
			}
		}

		public static bool IsEarlierThan(this DateTime dt, DateTime comparedTo, DateTimeTypesOfCompare type)
		{
			return dt.DeduceRelationTypeTo(comparedTo, type) == RelationType.EarlierThan;
		}

		public static bool IsEqual(this DateTime dt, DateTime comparedTo, DateTimeTypesOfCompare type)
		{
			return dt.DeduceRelationTypeTo(comparedTo, type) == RelationType.Equal;
		}

		public static bool IsLaterThan(this DateTime dt, DateTime comparedTo, DateTimeTypesOfCompare type)
		{
			return dt.DeduceRelationTypeTo(comparedTo, type) == RelationType.LaterThan;
		}

		#endregion

		public static bool IsOlderThanWeek(object dateToCheck)
		{
			if (dateToCheck == null) return false;
			return DateTime.Now.DiffDays(Convert.ToDateTime(dateToCheck)) > 7;
		}

		public static DateTime GetServerDateTime()
		{
			return DateTime.Now;
		}
	}

	public enum DateTimeTypesOfCompare
	{
		DateTime = 1,
		DateOnly = 2,
		TimeOnly = 3
	}

	public enum RelationType
	{
		EarlierThan, Equal, LaterThan
	}
}
