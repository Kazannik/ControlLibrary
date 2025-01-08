using System;
using System.Collections.Generic;
using System.Globalization;

namespace ControlLibrary.Structures
{
	public readonly struct Period : IComparable<Period>, IComparable<DateTime>
	{
		private static readonly DateTimeFormatInfo FormatInfo = CultureInfo.CurrentCulture.DateTimeFormat;

		private Period(int year, int month)
		{
			Month = month;
			Year = year;
		}

		public static Period Create(int year, int month)
		{
			if (month > 12)
				throw new ArgumentOutOfRangeException("Номер месяца не может быть больше 12");
			else if (month < 0)
				throw new ArgumentOutOfRangeException("Номер месяца не может быть меньше нуля");

			return new Period(year: year, month: month);
		}

		public static Period Create(DateTime date)
		{
			return Create(year: date.Year, month: date.Month);
		}

		public static Period Create(int year)
		{
			return Create(year: year, month: 0);
		}

		public static Period Today()
		{
			return Create(DateTime.Today);
		}

		public static Period Now()
		{
			return Create(DateTime.Now);
		}

		public static Period MaxValue
		{
			get { return Create(year: 2199, month: 12); }
		}

		public static Period MinValue
		{
			get { return Create(year: 1900, month: 0); }
		}

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
			if (string.IsNullOrWhiteSpace(s))
				throw new ArgumentNullException("Строка не может быть пустой, либо содержать только пробельные символы");
			else if (s.Length != 6)
				throw new ArgumentException("Число знаков в строке должно равняться 6");
			else if (!ParseTry(s, out Period result))
			{
				throw new ArgumentException("Строка не соотвествует формату");
			}
			else
			{
				return result;
			}
		}

		public int Month { get; }

		public int Year { get; }

		public Period ZeroMonth
		{
			get { return new Period(year: Year, month: 0); }
		}

		public Period FirstMonth
		{
			get { return new Period(year: Year, month: 1); }
		}

		public Period LastMonth
		{
			get { return new Period(year: Year, month: 12); }
		}

		public Period PreviouseMonth
		{
			get
			{
				if (Month <= 1)
					return new Period(year: Year - 1, month: 12);
				else
					return new Period(year: Year, month: Month - 1);
			}
		}
		public Period NextMonth
		{
			get
			{
				if (Month == 12)
					return new Period(year: Year + 1, month: 1);
				else
					return new Period(year: Year, month: Month + 1);
			}
		}

		public Period PreviouseYear
		{
			get { return new Period(year: Year - 1, month: Month); }
		}

		public Period NextYear
		{
			get { return new Period(year: Year + 1, month: Month); }
		}

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

		public DateTime ToDate()
		{
			if (Month > 0)
			{
				return new DateTime(year: Year, month: Month, day: 1);
			}
			else
			{
				return new DateTime(year: Year, month: 1, day: 1);
			}
		}

		public new string ToString()
		{
			return Year.ToString("0000") + Month.ToString("00");
		}

		public static implicit operator Period(int year)
		{
			return new Period(year: year, month: 0);
		}

		public static explicit operator int(Period period)
		{
			return period.Year;
		}

		public static implicit operator Period(DateTime date)
		{
			return new Period(year: date.Year, month: date.Month);
		}

		public static explicit operator DateTime(Period period)
		{
			return period.ToDate();
		}

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

		public override int GetHashCode()
		{
			return unchecked(87 * Year.GetHashCode() ^ Month.GetHashCode());
		}

		public static Period operator +(Period a, Period b)
		{
			decimal sum = a.Year * 12 + a.Month + b.Year * 12 + b.Month;
			decimal year = Math.Truncate(sum / 12);
			decimal month = sum - (year * 12);
			return new Period(year: (int)year, month: (int)month);
		}

		public static Period operator -(Period a, Period b)
		{
			decimal sum = a.Year * 12 + a.Month - b.Year * 12 - b.Month;
			decimal year = Math.Truncate(sum / 12);
			decimal month = sum - (year * 12);
			return new Period(year: (int)year, month: (int)month);
		}

		public static Period operator +(Period t, int m)
		{
			decimal sum = t.Year * 12 + t.Month + m;
			decimal year = Math.Truncate(sum / 12);
			decimal month = sum - (year * 12);
			return new Period(year: (int)year, month: (int)month);
		}

		public static Period operator -(Period t, int m)
		{
			decimal sum = t.Year * 12 + t.Month - m;
			decimal year = Math.Truncate(sum / 12);
			decimal month = sum - (year * 12);
			return new Period(year: (int)year, month: (int)month);
		}

		public static bool operator ==(Period x, Period y)
		{
			return Compare(x, y) == 0;
		}

		public static bool operator !=(Period x, Period y)
		{
			return Compare(x, y) != 0;
		}

		public static bool operator >(Period x, Period y)
		{
			return Compare(x, y) > 0;
		}
		public static bool operator <(Period x, Period y)
		{
			return Compare(x, y) < 0;
		}

		public static bool operator >=(Period x, Period y)
		{
			return Compare(x, y) >= 0;
		}
		public static bool operator <=(Period x, Period y)
		{
			return Compare(x, y) <= 0;
		}

		public static bool operator ==(Period x, DateTime y)
		{
			return Compare(x, y) == 0;
		}

		public static bool operator !=(Period x, DateTime y)
		{
			return Compare(x, y) != 0;
		}

		public static bool operator >(Period x, DateTime y)
		{
			return Compare(x, y) > 0;
		}
		public static bool operator <(Period x, DateTime y)
		{
			return Compare(x, y) < 0;
		}

		public static bool operator >=(Period x, DateTime y)
		{
			return Compare(x, y) >= 0;
		}
		public static bool operator <=(Period x, DateTime y)
		{
			return Compare(x, y) <= 0;
		}

		public int CompareTo(Period value)
		{
			return Compare(this, value);
		}

		public int CompareTo(DateTime value)
		{
			return Compare(this, value);
		}

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
			else if (!Equals(x, null) & Equals(y, null))
			{ return 1; }
			else if (Equals(x, null) & !Equals(y, null))
			{ return -1; }
			else { return 0; }
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
			else if (!Equals(x, null) & Equals(y, null))
			{ return 1; }
			else if (Equals(x, null) & !Equals(y, null))
			{ return -1; }
			else { return 0; }
		}

		public class PeriodComparer : IComparer<Period>
		{
			public int Compare(Period x, Period y)
			{
				return Period.Compare(x, y);
			}
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
