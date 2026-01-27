// Ignore Spelling: Previouse

using ControlLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ControlLibrary.Structures
{
	public readonly struct Period : IComparable<Period>, IComparable<DateTime>
	{
		/// <summary>
		/// Представляет нулевое значение. Это поле доступно только для чтения.
		/// </summary>
		public static readonly Period Empty = Create(year: 0, month: 0);

		/// <summary>
		/// Представляет максимальное значение. Это поле доступно только для чтения.
		/// </summary>
		public static readonly Period MaxValue = Create(year: 2199, month: 12);

		/// <summary>
		/// Представляет минимальное значение. Это поле доступно только для чтения.
		/// </summary>
		public static readonly Period MinValue = Create(year: 1900, month: 0);


		private static readonly DateTimeFormatInfo FormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

		private Period(int year, int month)
		{
			Month = month;
			Year = year;
		}

		public static Period Create(int year, int month)
		{
			Argument.AssertNotNull(year, nameof(year));
			Argument.AssertNotNull(month, nameof(month));
			if (month > 12)
				throw new ArgumentOutOfRangeException("Номер месяца не может быть больше 12");
			else if (month < 0)
				throw new ArgumentOutOfRangeException("Номер месяца не может быть меньше нуля");

			return new Period(year: year, month: month);
		}

		public static Period Create(DateTime date) =>
			Create(year: date.Year, month: date.Month);

		public static Period Create(int year) =>
			Create(year: year, month: 0);

		public static Period Today() =>
			Create(DateTime.Today);

		public static Period Now() =>
			Create(DateTime.Now);

		public static bool ParseTry(string s, out Period result)
		{
			if (string.IsNullOrWhiteSpace(s) || s.Length != 6)
			{
				result = default;
				return false;
			}
			else
			{
				if (int.TryParse(s.Substring(0, 4), out int year) &&
					int.TryParse(s.Substring(4), out int month))
				{
					result = Create(year: year, month: month);
					return true;
				}
				else
				{
					result = default;
					return false;
				}
			}
		}

		public static Period Parse(string s)
		{
			return string.IsNullOrWhiteSpace(s)
				? throw new ArgumentNullException("Строка не может быть пустой, либо содержать только пробельные символы", nameof(s))
				: s.Length != 6
				? throw new ArgumentException("Число знаков в строке должно равняться 6")
				: !ParseTry(s, out Period result) ? throw new ArgumentException("Строка не соответствует формату") : result;
		}

		public int Month { get; }

		public int Year { get; }

		public Period ZeroMonth => new Period(year: Year, month: 0);

		public Period FirstMonth => new Period(year: Year, month: 1);

		public Period LastMonth => new Period(year: Year, month: 12);

		public Period PreviouseMonth => Month <= 1 ? new Period(year: Year - 1, month: 12) : new Period(year: Year, month: Month - 1);

		public Period NextMonth => Month == 12 ? new Period(year: Year + 1, month: 1) : new Period(year: Year, month: Month + 1);

		public Period PreviouseYear => new Period(year: Year - 1, month: Month);

		public Period NextYear => new Period(year: Year + 1, month: Month);

		public string ToShortDateString()
		{
			if (Month > 0)
			{
				string monthName = FormatInfo.GetMonthName(Month).ToString();
				return monthName + " " + Year.ToString("0000");
			}
			else
				return Year.ToString("0000");
		}

		public DateTime ToDate() =>
			Month > 0 ? new DateTime(year: Year, month: Month, day: 1) : new DateTime(year: Year, month: 1, day: 1);
		
		public new string ToString() =>
			Year.ToString("0000") + Month.ToString("00");
		
		public static implicit operator Period(int year) =>
			new Period(year: year, month: 0);

		public static explicit operator int(Period period) =>
			period.Year;
		
		public static implicit operator Period(DateTime date) =>
			new Period(year: date.Year, month: date.Month);
		
		public static explicit operator DateTime(Period period) =>
			period.ToDate();

		public override bool Equals(object obj)
		{
			if ((obj == null) || !GetType().Equals(obj.GetType()))
			{
				return false;
			}
			else
			{
				Period p = (Period)obj;
				return (Year == p.Year) && (Month == p.Month);
			}
		}

		public override int GetHashCode() =>
			unchecked((87 * Year.GetHashCode()) ^ Month.GetHashCode());
		

		public static Period operator +(Period a, Period b)
		{
			decimal sum = (a.Year * 12) + a.Month + (b.Year * 12) + b.Month;
			decimal year = Math.Truncate(sum / 12);
			decimal month = sum - (year * 12);
			return new Period(year: (int)year, month: (int)month);
		}

		public static Period operator -(Period a, Period b)
		{
			decimal sum = (a.Year * 12) + a.Month - (b.Year * 12) - b.Month;
			decimal year = Math.Truncate(sum / 12);
			decimal month = sum - (year * 12);
			return new Period(year: (int)year, month: (int)month);
		}

		public static Period operator +(Period t, int m)
		{
			decimal sum = (t.Year * 12) + t.Month + m;
			decimal year = Math.Truncate(sum / 12);
			decimal month = sum - (year * 12);
			return new Period(year: (int)year, month: (int)month);
		}

		public static Period operator -(Period t, int m)
		{
			decimal sum = (t.Year * 12) + t.Month - m;
			decimal year = Math.Truncate(sum / 12);
			decimal month = sum - (year * 12);
			return new Period(year: (int)year, month: (int)month);
		}

		public static bool operator ==(Period x, Period y) => Compare(x, y) == 0;

		public static bool operator !=(Period x, Period y) => Compare(x, y) != 0;

		public static bool operator >(Period x, Period y) => Compare(x, y) > 0;

		public static bool operator <(Period x, Period y) => Compare(x, y) < 0;

		public static bool operator >=(Period x, Period y) => Compare(x, y) >= 0;

		public static bool operator <=(Period x, Period y) => Compare(x, y) <= 0; 

		public static bool operator ==(Period x, DateTime y) => Compare(x, y) == 0;

		public static bool operator !=(Period x, DateTime y) => Compare(x, y) != 0;

		public static bool operator >(Period x, DateTime y) => Compare(x, y) > 0;

		public static bool operator <(Period x, DateTime y) => Compare(x, y) < 0;

		public static bool operator >=(Period x, DateTime y) => Compare(x, y) >= 0;

		public static bool operator <=(Period x, DateTime y) => Compare(x, y) <= 0;

		public int CompareTo(Period value) => Compare(this, value);

		public int CompareTo(DateTime value) => Compare(this, value);

		public static int Compare(Period x, Period y)
		{
			if (!Equals(x, null) & !Equals(y, null))
			{
				try
				{
					int iCompare = decimal.Compare(x.Year, y.Year);
					if (iCompare == 0) { iCompare = decimal.Compare(x.Month, y.Month); }
					return iCompare;
				}
				catch (Exception)
				{ return 0; }
			}
			else
			{
				return !Equals(x, null) & Equals(y, null) ? 1 : Equals(x, null) & !Equals(y, null) ? -1 : 0;
			}
		}

		public static int Compare(Period x, DateTime y)
		{
			if (!Equals(x, null) & !Equals(y, null))
			{
				try
				{
					int iCompare = decimal.Compare(x.Year, y.Year);
					if (iCompare == 0) { iCompare = decimal.Compare(x.Month, y.Month); }
					return iCompare;
				}
				catch (Exception)
				{ return 0; }
			}
			else
			{
				return !Equals(x, null) & Equals(y, null) ? 1 : Equals(x, null) & !Equals(y, null) ? -1 : 0;
			}
		}

		public class PeriodComparer : IComparer<Period>
		{
			public int Compare(Period x, Period y) => Period.Compare(x, y);
		}
	}

	public static class PeriodExtensions
	{
		public static void FirstMonth(ref this Period period)
		{
			period = period.FirstMonth;
		}

		public static void PreviouseMonth(ref this Period period)
		{
			period = period.PreviouseMonth;
		}

		public static void NextMonth(ref this Period period)
		{
			period = period.NextMonth;
		}

		public static void LastMonth(ref this Period period)
		{
			period = period.LastMonth;
		}

		public static void PreviouseYear(ref this Period period)
		{
			period = period.PreviouseYear;
		}

		public static void NextYear(ref this Period period)
		{
			period = period.NextYear;
		}
	}

	public class PeriodEventArgs : EventArgs
	{
		public PeriodEventArgs(Period args)
		{
			Period = args;
		}

		public Period Period { get; }
	}
}
