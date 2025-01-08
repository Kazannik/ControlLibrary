using System;
using System.Collections.Generic;

namespace ControlLibrary.Structures
{
	public readonly struct Version : IComparable<Version>
	{
		private Version (int major, int minor)
		{
			Major = major;
			Minor = minor;
		}

		public static Version Empty()
		{
			return new Version (0, 0);
		}

		public static Version Create(int major)
		{
			return new Version(major, 0);
		}

		public static Version Create(int major, int minor)
		{
			return new Version(major, minor);
		}

		public int Major { get; }

		public int Minor { get; }

		public new string ToString()
		{
			return ((Major > 0 || Minor > 0) ? Major.ToString() : string.Empty) + (Minor > 0 ? "." + Minor.ToString() : string.Empty);
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
			return unchecked(87 * Major.GetHashCode() ^ Minor.GetHashCode());
		}


		public static bool operator ==(Version x, Version y)
		{
			return Compare(x, y) == 0;
		}

		public static bool operator !=(Version x, Version y)
		{
			return Compare(x, y) != 0;
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
			else if (!Equals(x, null) & Equals(y, null))
			{ return 1; }
			else if (Equals(x, null) & !Equals(y, null))
			{ return -1; }
			else { return 0; }
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
