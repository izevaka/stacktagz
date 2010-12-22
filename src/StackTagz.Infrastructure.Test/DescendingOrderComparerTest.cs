using StackTagz.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Moq;

namespace StackTagz.Infrastructure.Test
{

	[TestClass()]
	public class DescendingOrderComparerTest {


		private TestContext testContextInstance;

		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

	
		/// <summary>
		///A test for Compare
		///</summary>
		[TestMethod()]
		public void CompareShouldReturnYMinusX() {
			DescendingOrderComparer target = new DescendingOrderComparer();

			var actual = target.Compare(3, 5);
			Assert.AreEqual(2, actual);
			
		}
		[TestMethod()]
		public void OrderByShouldBeDescending() {
			DescendingOrderComparer target = new DescendingOrderComparer();

			var arr = new[] { 3, 1, 5, 2, 4 };
			var sorted = arr.OrderBy(item => item, target).ToArray();

			CollectionAssert.AreEqual(new[] { 5, 4, 3, 2, 1 }, sorted);
		}
	}
}
