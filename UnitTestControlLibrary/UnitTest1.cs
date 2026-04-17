using ControlLibrary.Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnitTestControlLibrary
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{			
			List<object> testList = new List<object>
			{
				Period.MaxValue,
				Period.MinValue,
				null,
				"MaxValue"
			};
			testList.Sort(new Period.PeriodComparer());

			Assert.AreEqual(testList.Count, 4, "Значение должно быть " + 4);

			Assert.AreEqual(testList[0], null, "Значение должно быть Null");

			Assert.AreNotEqual(testList[1], null, "Значение не должно быть Null");

			Assert.AreEqual(testList[2], Period.MinValue, "Значение должно быть Period.MinValue");

			Assert.AreEqual(testList[3], Period.MaxValue, "Значение должно быть Period.MaxValue");

		}
	}
}
