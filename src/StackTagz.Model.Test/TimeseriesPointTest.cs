using StackTagz.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using StackTagz.Model.Data;

namespace StackTagz.Model.Test
{
    
    
    /// <summary>
    ///This is a test class for TimeseriesPointTest and is intended
    ///to contain all TimeseriesPointTest Unit Tests
    ///</summary>
	[TestClass()]
	public class TimeseriesPointTest {


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
		///A test for TimeseriesPoint Constructor
		///</summary>
		[TestMethod()]
		public void TimeseriesPointConstructorTest() {
			DateTime count = new DateTime(2009, 06, 23); // TODO: Initialize to an appropriate value
			long questionId = 123; // TODO: Initialize to an appropriate value
			List<string> tags = new List<string> {"test1","test2" }; // TODO: Initialize to an appropriate value
			PointType type = PointType.Comment; // TODO: Initialize to an appropriate value
			TimeseriesPoint target = new TimeseriesPoint(count, questionId, tags, type);
			TimeseriesPoint actual = target;

			Assert.AreEqual( target, actual);
			Assert.AreEqual(2, actual.Tags.Count);
		}
	}
}
