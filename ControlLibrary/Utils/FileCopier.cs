// Ignore Spelling: Utils

using System;
using System.IO;

namespace ControlLibrary.Utils
{
	internal delegate void ProgressChangedDelegate(double persentage, ref bool cancel);
	internal delegate void ComplateDelegate();

	internal class FileCopier
	{
		public FileCopier(string source, string dest)
		{
			DestFilePath = dest;
			SourceFilePath = source;

			OnProgressChanged += delegate { };
			OnComplate += delegate { };
		}

		public void Copy()
		{
			byte[] buffer = new byte[1024 * 1024];
			bool cancelFlag = false;

			using (FileStream sourse = new FileStream(SourceFilePath, FileMode.Open, FileAccess.Read))
			{
				long fileLength = sourse.Length;
				using (FileStream dest = new FileStream(DestFilePath, FileMode.CreateNew, FileAccess.Write))
				{
					long totalBytes = 0;
					int currentBlockSize = 0;

					while ((currentBlockSize = sourse.Read(buffer, 0, buffer.Length)) > 0)
					{
						totalBytes += currentBlockSize;
						double persenage = totalBytes * 100.0 / fileLength;
						dest.Write(buffer, 0, currentBlockSize);
						cancelFlag = false;
						OnProgressChanged(persenage, ref cancelFlag);

						if (cancelFlag)
						{
							dest.Close();
							try
							{
								File.Delete(DestFilePath);
							}
							catch (Exception) { }
							break;
						}
					}
					OnComplate();
				}
			}
		}

		public string SourceFilePath { get; set; }

		public string DestFilePath { get; set; }

		public event ProgressChangedDelegate OnProgressChanged;

		public event ComplateDelegate OnComplate;
	}
}
