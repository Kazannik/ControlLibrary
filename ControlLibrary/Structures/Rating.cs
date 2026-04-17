using System;
using System.Collections.Generic;
using System.Globalization;

namespace ControlLibrary.Structures
{
	public readonly struct Rating : IComparable<Rating>
	{
		private const int EMPTY_VALUE = 0;
		private const int MIN_VALUE = -10;
		private const int MAX_VALUE = 10;

		/// <summary>
		/// Представляет нулевое значение. Это поле доступно только для чтения.
		/// </summary>
		public static readonly Rating Empty = new Rating(EMPTY_VALUE);
		/// <summary>
		/// Представляет минимальное значение (-10). Это поле доступно только для чтения.
		/// </summary>
		public static readonly Rating MinValue = new Rating(MIN_VALUE);
		/// <summary>
		/// Представляет максимальное значение (10). Это поле доступно только для чтения.
		/// </summary>
		public static readonly Rating MaxValue = new Rating(MAX_VALUE);

		private Rating(string value) : this(int.TryParse(value, out int intValue) ? intValue : 0) { }

		private Rating(int value)
		{
			Validate(value: value);
			Value = value;
		}

		public static Rating Create(int value) => new Rating(value: value);

		public int Value { get; }

		public bool IsEmpty => Value == EMPTY_VALUE;

		public bool IsMin => Value == MIN_VALUE;

		public bool IsMax => Value == MAX_VALUE;

		public bool IsNegative => Value < 0;

		public bool IsPositive => Value > 0;

		private static void Validate(int value)
		{
			if (value < MIN_VALUE || value > MAX_VALUE)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Не удалось привести value: {0} к типу {1}.", value.GetType().ToString(), typeof(Rating).ToString()));
			}
		}

		public new string ToString() => Value.ToString();

		public int GetPercent(int index, int count) => GetPercent(value: Value, index: index, count: count);

		public static int GetPercent(int value, int index, int count)
		{
			if (value == 0) return 0;
			int negative = value < 0 ? -1 : 1;
			int nIndex = value < 0 ? count - index - 1 : index;
			double part = (double)MAX_VALUE / (double)Math.Abs(value);
			double result = (count / part * 100) - (nIndex * 100);
			return result >= 100 ? 100 * negative : result <= 0 ? 0 : (int)result * negative;
		}

		public static int GetPercent(Rating value, int index, int count) =>
			GetPercent(value: value.Value, index: index, count: count);

		public static explicit operator string(Rating value) => value.ToString();

		public static explicit operator int(Rating value) => value.Value;

		public static explicit operator Rating(int value) => new Rating(value);

		public static explicit operator Rating(string value) => new Rating(value);

		public override bool Equals(object obj)
		{
			if ((obj == null) || !GetType().Equals(obj.GetType()))
			{
				return false;
			}
			else
			{
				Rating rating = (Rating)obj;
				return Value == rating.Value;
			}
		}

		public override int GetHashCode() =>
			unchecked((87 * Value.GetHashCode()) ^ Value.GetHashCode());

		public static Rating operator +(Rating a, Rating b) => new Rating(value: a.Value + b.Value);

		public static Rating operator ++(Rating value) => new Rating(value: value.Value + 1);

		public static Rating operator -(Rating a, Rating b) => new Rating(value: a.Value - b.Value);

		public static Rating operator --(Rating value) => new Rating(value: value.Value - 1);

		public static bool operator ==(Rating x, Rating y) => Compare(x, y) == 0;

		public static bool operator !=(Rating x, Rating y) => Compare(x, y) != 0;

		public static bool operator >(Rating x, Rating y) => Compare(x, y) > 0;

		public static bool operator <(Rating x, Rating y) => Compare(x, y) < 0;

		public static bool operator >=(Rating x, Rating y) => Compare(x, y) >= 0;

		public static bool operator <=(Rating x, Rating y) => Compare(x, y) <= 0;

		public static Rating operator +(Rating a, int b) => new Rating(value: a.Value + b);

		public static Rating operator -(Rating a, int b) => new Rating(value: a.Value - b);

		public static bool operator ==(Rating x, int y) => Compare(x, y) == 0;

		public static bool operator !=(Rating x, int y) => Compare(x, y) != 0;

		public static bool operator >(Rating x, int y) => Compare(x, y) > 0;

		public static bool operator <(Rating x, int y) => Compare(x, y) < 0;

		public static bool operator >=(Rating x, int y) => Compare(x, y) >= 0;

		public static bool operator <=(Rating x, int y) => Compare(x, y) <= 0;

		public int CompareTo(Rating other) => Compare(this, other);

		public int CompareTo(int other) => Compare(this, other);

		public static int Compare(Rating x, Rating y)
		{
			if (!Equals(x, null) & !Equals(y, null))
			{
				try
				{
					return decimal.Compare(x.Value, y.Value);
				}
				catch (Exception)
				{ return 0; }
			}
			else
			{
				return !Equals(x, null) & Equals(y, null) ? 1 : Equals(x, null) & !Equals(y, null) ? -1 : 0;
			}
		}

		public static int Compare(Rating x, int y)
		{
			if (!Equals(x, null) & !Equals(y, null))
			{
				try
				{
					return decimal.Compare(x.Value, y);
				}
				catch (Exception)
				{ return 0; }
			}
			else
			{
				return !Equals(x, null) & Equals(y, null) ? 1 : Equals(x, null) & !Equals(y, null) ? -1 : 0;
			}
		}

		public class RatingComparer : IComparer<Rating>
		{
			public int Compare(Rating x, Rating y) => Rating.Compare(x, y);
		}
	}

	public static class RatingExtensions
	{
		public static void Max(ref this Rating rating)
		{
			rating = Rating.MaxValue;
		}

		public static void Min(ref this Rating rating)
		{
			rating = Rating.MinValue;
		}

		public static void Increment(ref this Rating rating)
		{
			if (!rating.IsMax)
			{
				rating = Rating.Create(rating.Value + 1);
			}
		}

		public static void Decrement(ref this Rating rating)
		{
			if (!rating.IsMin)
			{
				rating = Rating.Create(rating.Value - 1);
			}
		}
	}
}