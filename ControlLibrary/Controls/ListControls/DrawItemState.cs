using System;
using System.Drawing;

namespace ControlLibrary.Controls
{
	[Flags]
	public enum DrawState : byte
	{
		/// <summary>
		/// В текущий момент состояние элемента не определено.
		/// </summary>
		None = 0,
		/// <summary>
		/// Элемент выбран.
		/// </summary>
		Selected = 1,
		/// <summary>
		/// Элемент недоступен.
		/// </summary>
		Disabled = 4,
		/// <summary>
		/// Элемент имеет фокус.
		/// </summary>
		Focus = 16,
		/// <summary>
		/// Элемент находится в видимом состоянии, используемом по умолчанию.
		/// </summary>
		Default = 32,
		/// <summary>
		/// Элемент отслеживается мышью (это значит, что он выделяется, когда на него наведен указатель мыши).
		/// </summary>
		HotLight = 64,
		/// <summary>
		/// Элемент неактивен.
		/// </summary>
		Inactive = 128
	}

	public static class Drawing
	{
		public static Color GetBackColor(DrawState state,Color backColor)
		{
			if (state == DrawState.None)
			{
				return backColor;
			}
			else if ((state & DrawState.Selected) == DrawState.Selected)
			{
				return SystemColors.Highlight;
			}
			else if ((state & DrawState.Disabled) == DrawState.Disabled)
			{
				return SystemColors.Control;
			}
			else if ((state & DrawState.Focus) == DrawState.Focus)
			{
				return SystemColors.Window;
			}
			else if ((state & DrawState.HotLight) == DrawState.HotLight)
			{
				return SystemColors.Window;
			}
			else if ((state & DrawState.Inactive) == DrawState.Inactive)
			{
				return backColor;
			}
			else
			{
				return backColor;
			}
		}

		public static Color GetForeColor(DrawState state, Color foreColor)
		{
			if (state == DrawState.None)
			{
				return foreColor;
			}
			else if ((state & DrawState.Selected) == DrawState.Selected)
			{
				return SystemColors.HighlightText;
			}
			else if ((state & DrawState.Disabled) == DrawState.Disabled)
			{
				return SystemColors.GrayText;
			}
			else if ((state & DrawState.Focus) == DrawState.Focus)
			{
				return SystemColors.WindowText;
			}
			else if ((state & DrawState.HotLight) == DrawState.HotLight)
			{
				return SystemColors.HotTrack;
			}
			else if ((state & DrawState.Inactive) == DrawState.Inactive)
			{
				return foreColor;
			}
			else
			{
				return SystemColors.WindowText;
			}
		}
	}
}
