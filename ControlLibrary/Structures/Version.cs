using System;
using System.Collections.Generic;

namespace ControlLibrary.Structures
{
	public readonly struct Version : IComparable<Version>
	{
		/// <summary>
		/// Представляет пустое значение. Это поле доступно только для чтения.
		/// </summary>
		public static readonly Version Empty = new Version(-1, -1, string.Empty);

		private Version(int major, int minor, string guid)
		{
			Major = major;
			Minor = minor;
			Guid = guid;
		}

		public static Version Create(int major, string guid)
		{
			return new Version(major, -1, guid);
		}

		public static Version Create(int major, int minor, string guid)
		{
			return new Version(major, minor, guid);
		}

		public int Major { get; }

		public int Minor { get; }

		public string Guid { get; }

		public new string ToString()
		{
			return ((Major >= 0 || Minor >= 0) ? Major.ToString() : string.Empty) + (Minor >= 0 ? "." + Minor.ToString() : string.Empty);
		}

		public static explicit operator string(Version value)
		{
			return value.ToString();
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !GetType().Equals(obj.GetType()))
			{
				return false;
			}
			else
			{
				Version version = (Version)obj;
				return (Major == version.Major) && (Minor == version.Minor);
			}
		}

		public override int GetHashCode()
		{
			return unchecked((87 * Major.GetHashCode()) ^ Minor.GetHashCode() ^ Guid.GetHashCode());
		}

		public static bool operator ==(Version x, Version y)
		{
			return Compare(x, y) == 0;
		}

		public static bool operator !=(Version x, Version y)
		{
			return Compare(x, y) != 0;
		}

		public static bool operator ==(Version x, int major)
		{
			return Compare(x, major) == 0;
		}

		public static bool operator !=(Version x, int major)
		{
			return Compare(x, major) != 0;
		}

		public int CompareTo(Version other)
		{
			return Compare(this, other);
		}

		public static int Compare(Version x, Version y)
		{
			if (!Equals(x, null) & !Equals(y, null))
			{
				try
				{
					int iCompare = decimal.Compare(x.Major, y.Major);
					if (iCompare == 0) { iCompare = decimal.Compare(x.Minor, y.Minor); }
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

		public static int Compare(Version x, int major)
		{
			if (!Equals(x, null) & !Equals(major, null))
			{
				try
				{
					int iCompare = decimal.Compare(x.Major, major);
					if (iCompare == 0) { iCompare = decimal.Compare(x.Minor, -1); }
					return iCompare;
				}
				catch (Exception)
				{ return 0; }
			}
			else
			{
				return !Equals(x, null) & Equals(major, null) ? 1 : Equals(x, null) & !Equals(major, null) ? -1 : 0;
			}
		}

		public class VersionComparer : IComparer<Version>
		{
			public int Compare(Version x, Version y)
			{
				return Version.Compare(x, y);
			}
		}
	}
}
