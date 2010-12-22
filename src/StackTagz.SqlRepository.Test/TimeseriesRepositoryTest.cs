using StackTagz.SqlRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StackTagz.Model.Data;
using Moq;
using System.Data.Objects;
using StackTagz.SqlRepository.EF;
using System.Collections.Generic;
using StackTagz.Infrastructure;
using System.Linq;

namespace StackTagz.SqlRepository.Test
{
    
    
    /// <summary>
    ///This is a test class for TimeseriesRepositoryTest and is intended
    ///to contain all TimeseriesRepositoryTest Unit Tests
    ///</summary>
	[TestClass()]
	public class TimeseriesRepositoryTest {
		[TestInitialize()]
		public void MyTestInitialize() {
			m_EntitiesMock = new Mock<IStackTagzContext>();
		}


		Mock<IStackTagzContext> m_EntitiesMock;
		/// <summary>
		///A test for GetTimeSeries
		///</summary>
		[TestMethod()]
		public void GetTimeSeriesShouldRollUpPeriods() {
			
			m_EntitiesMock.Setup(e => e.TSResults).Returns(new MockObjectSet<TSResult>(new List<TSResult> { 
				new TSResult(){TSPeriods = new List<TSPeriod>{
					new TSPeriod{QuestionCount = 6, AnswerCount = 1, Date = new DateTime(2010,6,23), tag = "tag1"},
					new TSPeriod{QuestionCount = 7, AnswerCount = 2, Date = new DateTime(2010,6,23), tag = "tag2"},
					new TSPeriod{QuestionCount = 8, AnswerCount = 3, Date = new DateTime(2010,6,23), tag = "tag3"},
					new TSPeriod{QuestionCount = 9, AnswerCount = 4, Date = new DateTime(2010,6,24), tag = "tag1"},
				}, Rollup = (int)Rollup.Weekly, UserId = 123, Site = "site"}
			}));
			TimeseriesRepository target = new TimeseriesRepository(m_EntitiesMock.Object);
			
			var expectedTS = new Dictionary<string, Timeseries> {
				{"tag1",new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod>{
					{new DateTime(2010,6,23), new TimeseriesPeriod(6,1,0)},
					{new DateTime(2010,6,24), new TimeseriesPeriod(9,4,0)}
				})},
				{"tag2",new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod>{
					{new DateTime(2010,6,23), new TimeseriesPeriod(7,2,0)},
				})},
				{"tag3",new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod>{
				    {new DateTime(2010,6,23), new TimeseriesPeriod(8,3,0)},
				})},
			};
			TimeseriesResult actual = target.GetTimeSeries("site", 123, null, null, Rollup.Weekly);

			CollectionAssert.AreEqual(expectedTS, actual.Timeseries);
		}

		[TestMethod()]
		public void GetTimeSeriesShouldReturnNullIfNoData() {

			m_EntitiesMock.Setup(e => e.TSResults).Returns(new MockObjectSet<TSResult>(new [] { 
				new TSResult(){TSPeriods = new List<TSPeriod>{
					new TSPeriod{QuestionCount = 6, AnswerCount = 1, Date = new DateTime(2010,6,23), tag = "tag1"},
					new TSPeriod{QuestionCount = 7, AnswerCount = 2, Date = new DateTime(2010,6,23), tag = "tag2"},
					new TSPeriod{QuestionCount = 8, AnswerCount = 3, Date = new DateTime(2010,6,23), tag = "tag3"},
					new TSPeriod{QuestionCount = 9, AnswerCount = 4, Date = new DateTime(2010,6,24), tag = "tag1"},
				}, Rollup = (int)Rollup.Weekly, UserId = 123, Site = "site"}
			}));
			TimeseriesRepository target = new TimeseriesRepository(m_EntitiesMock.Object);

			var expectedTS = new Dictionary<string, Timeseries> {
				{"tag1",new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod>{
					{new DateTime(2010,6,23), new TimeseriesPeriod(6,1,0)},
					{new DateTime(2010,6,24), new TimeseriesPeriod(9,4,0)}
				})},
				{"tag2",new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod>{
					{new DateTime(2010,6,23), new TimeseriesPeriod(7,2,0)},
				})},
				{"tag3",new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod>{
				    {new DateTime(2010,6,23), new TimeseriesPeriod(8,3,0)},
				})},
			};
			TimeseriesResult actual = target.GetTimeSeries("othersite", 123, null, null, Rollup.Weekly);

			Assert.IsNull(actual);
		}

		[TestMethod]
		public void SaveTimeseriesShouldCallAddTSResult() {
			TimeseriesRepository target = new TimeseriesRepository(m_EntitiesMock.Object);

			var result = new TSResult();
			m_EntitiesMock.Setup(e => e.CreateTSResult()).Returns(result);
			m_EntitiesMock.Setup(e=>e.AddTSResult(result));
			m_EntitiesMock.Setup(e=>e.SaveChanges());

			target.SaveTimeseries("site", 123, new TimeseriesResult(Rollup.Weekly) { });

			m_EntitiesMock.VerifyAll();
		}

		[TestMethod]
		public void SavePeriodsShouldAddToResult() {
			TimeseriesRepository target = new TimeseriesRepository(m_EntitiesMock.Object);
			var result = new TSResult();
			var period = new TSPeriod();
			m_EntitiesMock.Setup(e=>e.CreateTSPeriod(It.IsAny<TSResult>())).Returns(period);

			target.SavePeriods(result, new Dictionary<string, Timeseries> {
				{"tag1",new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod>{
					{new DateTime(2010,6,23), new TimeseriesPeriod(6,1,0)},
				})}});

			Assert.AreEqual(result.TSPeriods[0], period);
			m_EntitiesMock.VerifyAll();
		}



		//[TestMethod]
		//public void ActualDBTest() {
		//    var m_Entities = new StackTagzContext();
		//    var site = "api.stackoverflow.com";
		//    var itemIds = new[] { 1218262, 1689594 };
		//    (from q in m_Entities.Questions
		//     where q.Site == site && itemIds.Contains(q.QuestionId)
		//     select q).ToList().Select(q => q.ToQuestionInfo()).ToList();
		//}
	}
}
