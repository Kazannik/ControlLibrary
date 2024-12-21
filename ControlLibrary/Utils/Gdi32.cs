using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ControlLibrary.Utils
{
	public static class Gdi32
	{
		#region Constants

		public const int SRCCOPY = 0xCC0020;
		public const byte AC_SRC_OVER = 0x00;
		public const byte AC_SRC_ALPHA = 0x01;

		#endregion

		[StructLayout(LayoutKind.Sequential)]
		public struct BLENDFUNCTION
		{
			private readonly byte BlendOp;
			private readonly byte BlendFlags;
			private readonly byte SourceConstantAlpha;
			private readonly byte AlphaFormat;

			public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
			{
				BlendOp = op;
				BlendFlags = flags;
				SourceConstantAlpha = alpha;
				AlphaFormat = format;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RGBQUAD
		{
			public byte rgbBlue;
			public byte rgbGreen;
			public byte rgbRed;
			public byte rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAP
		{
			public int bmType;
			public int bmWidth;
			public int bmHeight;
			public int bmWidthBytes;
			public short bmPlanes;
			public short bmBitsPixel;
			public IntPtr bmBits;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFOHEADER
		{
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int bitClrImportant;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SIZE
		{
			public int cx;
			public int cy;

			public SIZE(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DIBSECTION
		{
			public BITMAP dsBm;
			public BITMAPINFOHEADER dsBmih;
			public int dsBitField1;
			public int dsBitField2;
			public int dsBitField3;
			public IntPtr dshSection;
			public int dsOffset;
		}

		[DllImport("gdi32.dll")]
		public static extern int BitBlt(
			IntPtr hdcDest,     // handle to destination DC (device context)
			int nXDest,         // x-coord of destination upper-left corner
			int nYDest,         // y-coord of destination upper-left corner
			int nWidth,         // width of destination rectangle
			int nHeight,        // height of destination rectangle
			IntPtr hdcSrc,      // handle to source DC
			int nXSrc,          // x-coordinate of source upper-left corner
			int nYSrc,          // y-coordinate of source upper-left corner
			int dwRop  // raster operation code
			);

		[DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
		public static extern bool AlphaBlend(
			IntPtr hdcDest,
			int nXOriginDest,
			int nYOriginDest,
			int nWidthDest,
			int nHeightDest,
			IntPtr hdcSrc,
			int nXOriginSrc,
			int nYOriginSrc,
			int nWidthSrc,
			int nHeightSrc,
			BLENDFUNCTION blendFunction);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

		[DllImport("gdi32.dll")]
		public static extern void DeleteObject(IntPtr obj);

		[DllImport("gdi32.dll", EntryPoint = "GetObject")]
		public static extern int GetObjectDIBSection(IntPtr hObject, int nCount, ref DIBSECTION lpObject);

		[DllImport("gdi32.dll", EntryPoint = "GetObject")]
		public static extern int GetObjectBitmap(IntPtr hObject, int nCount, [Out] IntPtr lpObject);

		public static void GetBitmapDimentions(IntPtr ipd, out int width, out int height)
		{
			// get the info about the HBITMAP inside the IPictureDisp
			var dibsection = new DIBSECTION();
			_ = GetObjectDIBSection(ipd, Marshal.SizeOf(dibsection), ref dibsection);
			width = dibsection.dsBm.bmWidth;
			height = dibsection.dsBm.bmHeight;
		}

		public static void ConvertPixelByPixel(IntPtr ipd, out int width, out int height)
		{
			// get the info about the HBITMAP inside the IPictureDisp
			DIBSECTION dibsection = new DIBSECTION();
			var res = GetObjectDIBSection(ipd, Marshal.SizeOf(dibsection), ref dibsection);
			width = dibsection.dsBm.bmWidth;
			height = dibsection.dsBm.bmHeight;
			unsafe
			{
				//Check is that 32bit bitmap
				if (dibsection.dsBmih.biBitCount == 32)
				{
					// get a pointer to the raw bits
					RGBQUAD* pBits = (RGBQUAD*)(void*)dibsection.dsBm.bmBits;

					// copy each pixel manually and premultiply the color values
					for (int x = 0; x < dibsection.dsBmih.biWidth; x++)
						for (int y = 0; y < dibsection.dsBmih.biHeight; y++)
						{
							int offset = y * dibsection.dsBmih.biWidth + x;
							if (pBits[offset].rgbReserved > 0 && (pBits[offset].rgbBlue > pBits[offset].rgbReserved || pBits[offset].rgbGreen > pBits[offset].rgbReserved || pBits[offset].rgbRed > pBits[offset].rgbReserved))
							{
								pBits[offset].rgbBlue = (byte)((((int)pBits[offset].rgbBlue * (int)pBits[offset].rgbReserved + 1) * 257) >> 16);
								pBits[offset].rgbGreen = (byte)((((int)pBits[offset].rgbGreen * (int)pBits[offset].rgbReserved + 1) * 257) >> 16);
								pBits[offset].rgbRed = (byte)((((int)pBits[offset].rgbRed * (int)pBits[offset].rgbReserved + 1) * 257) >> 16);
							}
						}
				}
			}
		}

		public static Bitmap ConvertPixelByPixel(stdole.IPictureDisp ipd)
		{
			DIBSECTION dibsection = new DIBSECTION();
			GetObjectDIBSection((IntPtr)ipd.Handle, Marshal.SizeOf(dibsection), ref dibsection);
			int width = dibsection.dsBm.bmWidth;
			int height = dibsection.dsBm.bmHeight;
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			unsafe
			{
				RGBQUAD* pBits = (RGBQUAD*)(void*)dibsection.dsBm.bmBits;
				for (int x = 0; x < dibsection.dsBmih.biWidth; x++)
					for (int y = 0; y < dibsection.dsBmih.biHeight; y++)
					{
						int offset = y * dibsection.dsBmih.biWidth + x;
						if (pBits[offset].rgbReserved != 0)
						{
							bitmap.SetPixel(x, y, Color.FromArgb(pBits[offset].rgbReserved, pBits[offset].rgbRed, pBits[offset].rgbGreen, pBits[offset].rgbBlue));
						}
						else
						{
							bitmap.SetPixel(x, y, Color.FromArgb(255, pBits[offset].rgbRed, pBits[offset].rgbGreen, pBits[offset].rgbBlue));
						}
					}
			}
			return bitmap;
		}

		[DllImport("user32.dll", EntryPoint = "GetDC", CharSet = CharSet.Auto)]
		public static extern IntPtr GetDeviceContext(IntPtr hWnd);

		[DllImport("gdi32", EntryPoint = "CreateCompatibleBitmap")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

		public static void NativeDraw(IntPtr destDC, IntPtr hBitmap, int x, int y, int iconSize, Boolean isHidden = false)
		{
			IntPtr destCDC = CreateCompatibleDC(destDC);
			IntPtr oldSource = SelectObject(destCDC, hBitmap);
			AlphaBlend(destDC, x, y, iconSize, iconSize, destCDC, 0, 0, iconSize, iconSize, new BLENDFUNCTION(AC_SRC_OVER, 0, (byte)(isHidden ? 0x7f : 0xff), AC_SRC_ALPHA));
			SelectObject(destCDC, oldSource);
			DeleteObject(destCDC);
			DeleteObject(oldSource);
			DeleteObject(hBitmap);
		}

		public static void NativeDraw(IntPtr destDC, IntPtr hBitmap, int x, int y, int iconSizeWidth, int iconSizeHeight, Boolean isHidden = false)
		{
			IntPtr destCDC = CreateCompatibleDC(destDC);
			IntPtr oldSource = SelectObject(destCDC, hBitmap);
			AlphaBlend(destDC, x, y, iconSizeWidth, iconSizeHeight, destCDC, 0, 0, iconSizeWidth, iconSizeHeight, new BLENDFUNCTION(AC_SRC_OVER, 0, (byte)(isHidden ? 0x7f : 0xff), AC_SRC_ALPHA));
			SelectObject(destCDC, oldSource);
			DeleteObject(destCDC);
			DeleteObject(oldSource);
			DeleteObject(hBitmap);
		}

		public static void NativeDrawCrop(IntPtr destDC, IntPtr hBitmap, int x, int y, int xOrig, int yOrigin, int iconSizeWidth, int iconSizeHeight, Boolean isHidden = false)
		{
			IntPtr destCDC = CreateCompatibleDC(destDC);
			IntPtr oldSource = SelectObject(destCDC, hBitmap);
			AlphaBlend(destDC, x, y, iconSizeWidth, iconSizeHeight, destCDC, xOrig, yOrigin, iconSizeWidth, iconSizeHeight, new BLENDFUNCTION(AC_SRC_OVER, 0, (byte)(isHidden ? 0x7f : 0xff), AC_SRC_ALPHA));
			SelectObject(destCDC, oldSource);
			DeleteObject(destCDC);
			DeleteObject(oldSource);
			DeleteObject(hBitmap);
		}

		public static void NativeDraw(IntPtr destDC, IntPtr hBitmap, int x, int y, int iconSizeWidth, int iconSizeHeight, int iconSizeWidthDest, int iconSizeHeightDest, Boolean isHidden = false)
		{
			IntPtr destCDC = CreateCompatibleDC(destDC);
			IntPtr oldSource = SelectObject(destCDC, hBitmap);
			AlphaBlend(destDC, x, y, iconSizeWidthDest, iconSizeHeightDest, destCDC, 0, 0, iconSizeWidth, iconSizeHeight, new BLENDFUNCTION(AC_SRC_OVER, 0, (byte)(isHidden ? 0x7f : 0xff), AC_SRC_ALPHA));
			SelectObject(destCDC, oldSource);
			DeleteObject(destCDC);
			DeleteObject(oldSource);
			DeleteObject(hBitmap);
		}

		public static Bitmap RoundCorners(Bitmap StartImage, int cornerRadius, Brush backgroundColor)
		{
			if (cornerRadius == 0)
			{
				Bitmap roundedImage = new Bitmap(StartImage.Width, StartImage.Height);
				var r = new Rectangle(0, 0, StartImage.Width - 2, StartImage.Height - 2);
				using (Graphics g = Graphics.FromImage(roundedImage))
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					//g.CompositingQuality = CompositingQuality.HighQuality;
					g.InterpolationMode = InterpolationMode.NearestNeighbor;
					g.FillRectangle(backgroundColor, r);
					g.DrawRectangle(Pens.Gray, r);
					return roundedImage;
				}
			}
			else
			{
				var d = cornerRadius * 2;
				Bitmap roundedImage = new Bitmap(StartImage.Width, StartImage.Height);
				var r = new Rectangle(0, 0, StartImage.Width - d, StartImage.Height - d);
				using (Graphics g = Graphics.FromImage(roundedImage))
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					//g.CompositingQuality = CompositingQuality.HighQuality;
					g.InterpolationMode = InterpolationMode.NearestNeighbor;
					GraphicsPath gp = new GraphicsPath();
					gp.AddArc(r.X, r.Y, d, d, 180, 90);
					gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
					gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
					gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
					gp.AddLine(r.X, r.Y + r.Height - d, r.X, r.Y + d / 2);

					g.FillPath(backgroundColor, gp);
					g.DrawPath(Pens.Gray, gp);
					return roundedImage;
				}
			}
		}
	}
}
