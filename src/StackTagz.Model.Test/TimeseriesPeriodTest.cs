using StackTagz.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using StackTagz.Model.Data;

namespace StackTagz.Model.Test
{
    
    
    /// <summary>
    ///This is a test class for TimeseriesPeriodTest and is intended
    ///to contain all TimeseriesPeriodTest Unit Tests
    ///</summary>
	[TestClass()]
	public class TimeseriesPeriodTest {


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for op_Addition
		///</summary>
		[TestMethod()]
		public void op_AdditionTest() {
			TimeseriesPoint point =		new TimeseriesPoint(DateTime.MinValue, 123, new List<string> { "sdf", "sdfs"}, PointType.Comment); // TODO: Initialize to an appropriate value
			TimeseriesPeriod period =	new TimeseriesPeriod(2, 3, 4); // TODO: Initialize to an appropriate value
			TimeseriesPeriod expected = new TimeseriesPeriod(2, 3, 5); // TODO: Initialize to an appropriate value
			TimeseriesPeriod actual;						 
			actual = (period + point);
			Assert.AreEqual(expected, actual);
		}
	}
}
