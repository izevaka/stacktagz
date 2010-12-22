using StackTagz.SqlRepository.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StackTagz.Model.Data;

namespace StackTagz.SqlRepository.Test.EF
{
	[TestClass()]
	public class TSPeriodTest {

		[TestMethod()]
		public void ToTimeseriesPeriodTest() {
			TSPeriod target = new TSPeriod { QuestionCount = 1, AnswerCount = 2, CommentCount = 3};
			TimeseriesPeriod expected = new TimeseriesPeriod(1,2,3); 
			TimeseriesPeriod actual;
			actual = target.ToTimeseriesPeriod();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void SetFromTimeseriesPeriodTest() {
			TSPeriod target = new TSPeriod(); 
			string tag = "tag1"; 
			DateTime date = new DateTime(2010, 06, 23); 
			TimeseriesPeriod timeseriesPeriod = new TimeseriesPeriod(1,2,3); 
			target.SetFromTimeseriesPeriod(tag, date, timeseriesPeriod);
			Assert.AreEqual(target, new TSPeriod { Date = new DateTime(2010, 06, 23), tag = "tag1", QuestionCount = 1, AnswerCount = 2, CommentCount = 3});
		}
	}
}
