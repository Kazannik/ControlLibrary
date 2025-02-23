using System;
using System.Collections.Generic;
using System.Globalization;

namespace ControlLibrary.Structures
{
	public readonly struct Rating : IComparable<Rating>
	{
		private const int EMPTY_VALUE = 0;
		private const int MIN_VALUE = -5;
		private const int MAX_VALUE = 5;

		/// <summary>
		/// Представляет нулевое значение. Это поле доступно только для чтения.
		/// </summary>
		public static readonly Rating Empty = new Rating(EMPTY_VALUE);
		/// <summary>
		/// Представляет минимальное значение (-5). Это поле доступно только для чтения.
		/// </summary>
		public static readonly Rating MinValue = new Rating(MIN_VALUE);
		/// <summary>
		/// Представляет максимальное значение (5). Это поле доступно только для чтения.
		/// </summary>
		public static readonly Rating MaxValue = new Rating(MAX_VALUE);
		
		private Rating(int value)
		{
			Validate(value: value);
			Value = value;
		}

		public static Rating Create(int value)
		{
			return new Rating(value: value);
		}

		public int Value { get; }

		public bool IsEmpty 
		{
			get { return Value == EMPTY_VALUE; }
		}

		public bool IsMin
		{
			get { return Value == MIN_VALUE; }
		}

		public bool IsMax
		{
			get { return Value == MAX_VALUE; }
		}

		private static void Validate(int value)
		{
			if (value < MIN_VALUE || value > MAX_VALUE)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Не удалось привести value: {0} к типу {1}.", value.GetType().ToString(), typeof(Rating).ToString()));
			}
		}

		public new string ToString()
		{
			return Value.ToString();
		}

		public int GetPercent(int index, int count)
		{
			return 50;
		}

		public static explicit operator string(Rating value)
		{
			return value.ToString();
		}

		public static explicit operator int(Rating value)
		{
			return value.Value;
		}

		public static explicit operator Rating(int value)
		{
			return new Rating(value);
		}

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

		public override int GetHashCode()
		{
			return unchecked((87 * Value.GetHashCode()) ^ Value.GetHashCode());
		}

		public static Rating operator +(Rating a, Rating b)
		{
			return new Rating(value: a.Value + b.Value);
		}

		public static Rating operator ++(Rating value)
		{
			return new Rating(value: value.Value + 1);
		}

		public static Rating operator -(Rating a, Rating b)
		{
			return new Rating(value: a.Value - b.Value);
		}

		public static Rating operator --(Rating value)
		{
			return new Rating(value: value.Value - 1);
		}

		public static bool operator ==(Rating x, Rating y)
		{
			return Compare(x, y) == 0;
		}

		public static bool operator !=(Rating x, Rating y)
		{
			return Compare(x, y) != 0;
		}

		public static bool operator >(Rating x, Rating y)
		{
			return Compare(x, y) > 0;
		}

		public static bool operator <(Rating x, Rating y)
		{
			return Compare(x, y) < 0;
		}

		public static bool operator >=(Rating x, Rating y)
		{
			return Compare(x, y) >= 0;
		}

		public static bool operator <=(Rating x, Rating y)
		{
			return Compare(x, y) <= 0;
		}

		public static Rating operator +(Rating a, int b)
		{
			return new Rating(value: a.Value + b);
		}

		public static Rating operator -(Rating a, int b)
		{
			return new Rating(value: a.Value - b);
		}

		public static bool operator ==(Rating x, int y)
		{
			return Compare(x, y) == 0;
		}

		public static bool operator !=(Rating x, int y)
		{
			return Compare(x, y) != 0;
		}

		public static bool operator >(Rating x, int y)
		{
			return Compare(x, y) > 0;
		}

		public static bool operator <(Rating x, int y)
		{
			return Compare(x, y) < 0;
		}

		public static bool operator >=(Rating x, int y)
		{
			return Compare(x, y) >= 0;
		}

		public static bool operator <=(Rating x, int y)
		{
			return Compare(x, y) <= 0;
		}

		public int CompareTo(Rating other)
		{
			return Compare(this, other);
		}

		public int CompareTo(int other)
		{
			return Compare(this, other);
		}

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
			public int Compare(Rating x, Rating y)
			{
				return Rating.Compare(x, y);
			}
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
			if (!rating.IsMax) {
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