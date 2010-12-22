using StackTagz.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using StackTagz.Model.Data;

namespace StackTagz.Model.Test
{
    
	[TestClass()]
	public class TimeseriesTest : EqualityTestBase<Timeseries> {


		private TestContext testContextInstance;

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


		///// <summary>
		/////A test for Add
		/////</summary>
		//[TestMethod()]
		//public void AddTest() {
		//    Timeseries target = new Timeseries(); // TODO: Initialize to an appropriate value
		//    DateTime date = new DateTime(); // TODO: Initialize to an appropriate value
		//    int value = 0; // TODO: Initialize to an appropriate value
		//    Rollup rollup = new Rollup(); // TODO: Initialize to an appropriate value
		//    target.Add(date, value, rollup);
		//    Assert.Inconclusive("A method that does not return a value cannot be verified.");
		//}

		/// <summary>
		///Daily rollup
		///</summary>
		[TestMethod()]
		public void GetRolledUpShouldReturnDateForDaily() {
			Timeseries timeseries = new Timeseries(Rollup.Daily); // TODO: Initialize to an appropriate value
			DateTime date = new DateTime(2009, 06, 24, 15, 14, 13); // TODO: Initialize to an appropriate value

			DateTime actual;
			actual = timeseries.GetRolledUpDate(date);
			Assert.AreEqual(new DateTime(2009, 06, 24), actual);
		}
		/// <summary>
		///Monthly rollup
		///</summary>
		[TestMethod()]
		public void GetRolledUpShouldReturnFirstOfNextonthForMonthly() {
			Timeseries timeseries = new Timeseries(Rollup.Monthly); // TODO: Initialize to an appropriate value
			DateTime date = new DateTime(2009, 06, 24, 15, 14, 13); // TODO: Initialize to an appropriate value

			DateTime actual;
			actual = timeseries.GetRolledUpDate(date);
			Assert.AreEqual(new DateTime(2009, 07, 1), actual);
		}
		/// <summary>
		///Monthly rollup
		///</summary>
		[TestMethod()]
		public void GetRolledUpShouldReturnDateForFirstForMonthly() {
			Timeseries timeseries = new Timeseries(Rollup.Monthly); // TODO: Initialize to an appropriate value
			DateTime date = new DateTime(2009, 06, 1, 15, 14, 13); // TODO: Initialize to an appropriate value

			DateTime actual;
			actual = timeseries.GetRolledUpDate(date);
			Assert.AreEqual(new DateTime(2009, 06, 1), actual);
		}

		/// <summary>
		///Weekly rollup
		///</summary>
		[TestMethod()]
		public void GetRolledUpShouldReturnSundayForWeekly() {
			Timeseries timeseries = new Timeseries(Rollup.Weekly); // TODO: Initialize to an appropriate value
			DateTime date = new DateTime(2009, 06, 24, 15, 14, 13); // TODO: Initialize to an appropriate value

			DateTime actual;
			actual = timeseries.GetRolledUpDate(date);
			Assert.AreEqual(new DateTime(2009, 06, 21), actual);
		}
		/// <summary>
		///Weekly rollup
		///</summary>
		[TestMethod()]
		public void GetRolledUpShouldReturnSameDateIfSundayForWeekly() {
			Timeseries timeseries = new Timeseries(Rollup.Weekly); // TODO: Initialize to an appropriate value
			DateTime date = new DateTime(2009, 06, 28, 15, 14, 13); // TODO: Initialize to an appropriate value

			DateTime actual;
			actual = timeseries.GetRolledUpDate(date);
			Assert.AreEqual(new DateTime(2009, 06, 28), actual);
		}

		[TestMethod()]
		public override void EqualsShouldBeTrueForDefault() {
			DoEqualsShouldBeTrueForDefault();
		}
		[TestMethod()]
		public override void EqualsShouldBeFalseForOtherNull() {
			DoEqualsShouldBeFalseForOtherNull();
		}
		[TestMethod()]
		public override void EqualsShouldBeFalseForOtherType() {
			DoEqualsShouldBeFalseForOtherType();
		}
	}
}
