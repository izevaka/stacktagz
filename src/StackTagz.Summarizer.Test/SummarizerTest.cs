using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StackTagz.Model;
using Moq;
using System.Collections.Generic;
using StackTagz.Infrastructure;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;
using StackTagz.Model.Data;
using StackTagz.Model.Comments;
using System.Collections;
using System.Linq;

namespace StackTagz.Summarizer.Test
{
    
    
	[TestClass()]
	public class SummarizerTest {
		Mock<IQuestionsRepository> m_questionRepo ; 
		Mock<IAnswersRepository> m_answerRepo ;
		Mock<IPersistQuestionsRepository> m_PersistQuestionsRepo;
		Mock<ICommentQuestionProcessor> m_commentsProcessorMock;
		Mock<ICommentsRepository> m_commentsRepositoryMock;

	
		
		List<QuestionInfo> m_UserQuestionData = new List<QuestionInfo> { 
				new QuestionInfo{Id = 1234, Tags=new List<string>{"tag1", "tag2"}, CreationDate = new DateTime(2009, 06,23)},
				new QuestionInfo{Id = 1235, Tags=new List<string>{"tag2", "tag3"}, CreationDate = new DateTime(2009, 06,24)}
			};

		List<QuestionInfo> m_PersistQuestionData = new List<QuestionInfo> { 
				new QuestionInfo{Id = 3456, Tags=new List<string>{"tag1", "tag2"}, CreationDate = new DateTime(2009, 06,25)}
			};

		List<QuestionInfo> m_QuestionListData = new List<QuestionInfo> { 
				new QuestionInfo{Id = 2345, Tags=new List<string>{"tag1", "tag4"}, CreationDate = new DateTime(2009, 06,25)}
			};
		
		[TestInitialize()]
		public void MyTestInitialize() {
			m_questionRepo = new Mock<IQuestionsRepository>();
			m_answerRepo = new Mock<IAnswersRepository>();
			m_PersistQuestionsRepo = new Mock<IPersistQuestionsRepository>();

			m_questionRepo.Setup(q => q.Get(It.IsAny<string>(), It.IsAny<int>())).Returns(m_UserQuestionData);

			m_answerRepo.Setup(q => q.Get(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<AnswerInfo> { 
				new AnswerInfo{QuestionId = 1234, CreationDate = new DateTime(2009, 06,26)},
				new AnswerInfo{QuestionId = 2345, CreationDate = new DateTime(2009, 06,27)},
				new AnswerInfo{QuestionId = 3456, CreationDate = new DateTime(2009, 06,27)}
			});

			m_questionRepo.Setup(q => q.Get(It.IsAny<string>(), It.IsAny<IEnumerable<int>>())).Returns(m_QuestionListData);

			m_PersistQuestionsRepo.Setup(r => r.Get(It.IsAny<string>(), It.IsAny<IEnumerable<int>>())).Returns(new List<QuestionInfo>(m_PersistQuestionData));

			m_commentsProcessorMock = new Mock<ICommentQuestionProcessor> ();
			m_commentsRepositoryMock = new Mock<ICommentsRepository> ();

		}


		/// <summary>
		///A test for RequestTimeSeries
		///</summary>
		[TestMethod()]
		public void RequestTimeSeriesShouldReturnQuestionAndAnswers() {
			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, null, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object); 
			string site = string.Empty; 
			int userId = 0; 
			Nullable<DateTime> start = new Nullable<DateTime>(); 
			Nullable<DateTime> end = new Nullable<DateTime>(); 
			TimeseriesResult actual;
			actual = target.RequestTimeSeries(site, userId, start, end, Rollup.Weekly);

			CollectionAssert.AreEqual(new []{"tag1", "tag2", "tag3", "tag4"}, actual.Timeseries.Keys);
			Assert.IsTrue(actual.Timeseries["tag1"].IsEqual(new Dictionary<DateTime, TimeseriesPeriod> { { new DateTime(2009, 06, 21), new TimeseriesPeriod(1, 2, 0) } }));
			Assert.IsTrue(actual.Timeseries["tag2"].IsEqual(new Dictionary<DateTime, TimeseriesPeriod> { { new DateTime(2009, 06, 21), new TimeseriesPeriod(2, 1, 0) } }));
			Assert.IsTrue(actual.Timeseries["tag3"].IsEqual(new Dictionary<DateTime, TimeseriesPeriod> { { new DateTime(2009, 06, 21), new TimeseriesPeriod(1, 0, 0) } }));
			Assert.IsTrue(actual.Timeseries["tag4"].IsEqual(new Dictionary<DateTime, TimeseriesPeriod> { { new DateTime(2009, 06, 21), new TimeseriesPeriod(0, 1, 0) } }));
		}

		[TestMethod]
		public void RequestTimeSeriesShouldRequestUnknownQuestionIds() {
			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, null, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			string site = string.Empty;
			int userId = 0;
			Nullable<DateTime> start = new Nullable<DateTime>();
			Nullable<DateTime> end = new Nullable<DateTime>();
			TimeseriesResult actual;
			actual = target.RequestTimeSeries(site, userId, start, end, Rollup.Weekly);
			m_questionRepo.Verify(q => q.Get(It.IsAny<string>(), It.Is<IEnumerable<int>>(v => v.IsEqual(new[] { 2345 }))), Times.Exactly(1));
		}
		[TestMethod]
		public void RequestTimeSeriesShouldPersistUnknownQuestionIds() {
			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, null, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			string site = string.Empty;
			int userId = 0;
			Nullable<DateTime> start = new Nullable<DateTime>();
			Nullable<DateTime> end = new Nullable<DateTime>();
			TimeseriesResult actual;
			actual = target.RequestTimeSeries(site, userId, start, end, Rollup.Weekly);
			m_PersistQuestionsRepo.Verify(q => q.SaveQuestions(site, m_QuestionListData), Times.Exactly(1));
		}

		[TestMethod]
		public void RequestTimeSeriesShouldaskPersistRepoForQuestions() {
			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, null, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			string site = string.Empty;
			int userId = 0;
			Nullable<DateTime> start = new Nullable<DateTime>();
			Nullable<DateTime> end = new Nullable<DateTime>();
			TimeseriesResult actual;
			actual = target.RequestTimeSeries(site, userId, start, end, Rollup.Weekly);
			m_PersistQuestionsRepo.Verify(q => q.Get(It.IsAny<string>(), It.Is<IEnumerable<int>>(v => v.IsEqual(new[] { 2345,3456 }))), Times.Exactly(1));
		}

		[TestMethod]
		public void RequestTimeSeriesShouldNotCallQuestionRepoIfNoAnswers() {
			m_answerRepo = new Mock<IAnswersRepository>();
			m_answerRepo.Setup(q => q.Get(It.IsAny<string>(), It.IsAny<int>())).Returns(new List<AnswerInfo>());

			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, null, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			string site = string.Empty;
			int userId = 0;
			Nullable<DateTime> start = new Nullable<DateTime>();
			Nullable<DateTime> end = new Nullable<DateTime>();
			TimeseriesResult actual;
			actual = target.RequestTimeSeries(site, userId, start, end, Rollup.Weekly);
			m_questionRepo.Verify(q => q.Get(It.IsAny<string>(), It.IsAny<IEnumerable<int>>()), Times.Never());
		}

		[TestMethod]
		public void RequestTimeSeriesShouldSaveQuestions() {
			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, null, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			string site = string.Empty;
			int userId = 0;
			Nullable<DateTime> start = new Nullable<DateTime>();
			Nullable<DateTime> end = new Nullable<DateTime>();
			TimeseriesResult actual;
			actual = target.RequestTimeSeries(site, userId, start, end, Rollup.Weekly);

			m_PersistQuestionsRepo.Verify(r => r.SaveQuestions(site, m_UserQuestionData), Times.Once());
		}


		[TestMethod]
		public void GetTimeSeriesReturnPersistRepoDataIfNotNull() {

			var expected = new TimeseriesResult(Rollup.Daily);
			Mock<IPersistTimeseriesRepository> persistTsRepo = new Mock<IPersistTimeseriesRepository>();
			persistTsRepo.Setup(r => r.GetTimeSeries(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<Rollup>())).Returns(expected);

			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, persistTsRepo.Object, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			string site = string.Empty;
			int userId = 0;
			Nullable<DateTime> start = new Nullable<DateTime>();
			Nullable<DateTime> end = new Nullable<DateTime>();
			TimeseriesResult actual;
			actual = target.GetTimeSeries(site, userId, start, end, Rollup.Weekly);
			persistTsRepo.VerifyAll();
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void GetTimeSeriesShouldCallPersistRepoWithSameParams() {
			string site = string.Empty;
			int userId = 123;
			Nullable<DateTime> start = new Nullable<DateTime>();
			Nullable<DateTime> end = new Nullable<DateTime>();

			var expected = new TimeseriesResult(Rollup.Daily);
			Mock<IPersistTimeseriesRepository> persistTsRepo = new Mock<IPersistTimeseriesRepository>();
			persistTsRepo.Setup(r => r.GetTimeSeries(site, userId, start, end, Rollup.Weekly)).Returns(expected);

			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, persistTsRepo.Object, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			TimeseriesResult actual;
			actual = target.GetTimeSeries(site, userId, start, end, Rollup.Weekly);
			persistTsRepo.VerifyAll();
		}
		[TestMethod]
		public void GetTimeSeriesShouldSaveResultIfRequestingFromServer() {
			string site = string.Empty;
			int userId = 123;
			Nullable<DateTime> start = new Nullable<DateTime>();
			Nullable<DateTime> end = new Nullable<DateTime>();

			
			Mock<IPersistTimeseriesRepository> persistTsRepo = new Mock<IPersistTimeseriesRepository>();
			
			persistTsRepo.Setup(r => r.GetTimeSeries(site, userId, start, end, Rollup.Weekly)).Returns((TimeseriesResult)null);
			persistTsRepo.Setup(r => r.SaveTimeseries(site, userId, It.IsAny<TimeseriesResult>()));

			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, persistTsRepo.Object, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			TimeseriesResult actual;
			actual = target.GetTimeSeries(site, userId, start, end, Rollup.Weekly);
			persistTsRepo.VerifyAll();
		}

		[TestMethod]
		public void GetTimeSeriesShouldProcessComments() {
			Summarizer target = new Summarizer(m_questionRepo.Object, m_answerRepo.Object, m_PersistQuestionsRepo.Object, null, m_commentsRepositoryMock.Object, m_commentsProcessorMock.Object);
			int userId = 0;

			m_commentsProcessorMock.Setup(cp => cp.ConvertAnswersToQuestions(It.IsAny<string>(), It.IsAny<IEnumerable<CommentInfo>>()))
				.Returns(new[] { 
					new CommentInfo { CreationDate = new DateTime(2010, 07, 01), Id = 9876, IsQuestion = true } ,
					new CommentInfo { CreationDate = new DateTime(2010, 07, 02), Id = 9875, IsQuestion = true } 
				});

			m_questionRepo.Setup(ar => ar.Get(It.IsAny<string>(), It.Is<IEnumerable<int>>(arr => arr.IsEqual(new[] { 9876, 9875 }))))
				.Returns(new List<QuestionInfo> { 
					new QuestionInfo { CreationDate = new DateTime(2010, 06, 30), Id = 9876, Tags = new List<string> { "tag1", "tag3" } } ,
					new QuestionInfo { CreationDate = new DateTime(2010, 06, 29), Id = 9875, Tags = new List<string> { "tag3", "tag4" } } 
				});

			m_PersistQuestionsRepo.Setup(qpr => qpr.Get(It.IsAny<string>(), It.Is<IEnumerable<int>>(arr => arr.IsEqual(new[] { 9876, 9875 })))).Returns(new List<QuestionInfo> { });
			
			var actual = target.RequestTimeSeries("", userId, null,null, Rollup.Weekly);

			CollectionAssert.AreEquivalent(new[] { "tag1", "tag2", "tag3", "tag4" }, actual.Timeseries.Keys);
			Assert.IsTrue(actual.Timeseries["tag1"].IsEqual(new Dictionary<DateTime, TimeseriesPeriod> { { new DateTime(2009, 06, 21), new TimeseriesPeriod(1, 2, 0) }, { new DateTime(2010, 06, 27), new TimeseriesPeriod(0, 0, 1) } }));
			Assert.IsTrue(actual.Timeseries["tag2"].IsEqual(new Dictionary<DateTime, TimeseriesPeriod> { { new DateTime(2009, 06, 21), new TimeseriesPeriod(2, 1, 0) }}));
			Assert.IsTrue(actual.Timeseries["tag3"].IsEqual(new Dictionary<DateTime, TimeseriesPeriod> { { new DateTime(2009, 06, 21), new TimeseriesPeriod(1, 0, 0) }, { new DateTime(2010, 06, 27), new TimeseriesPeriod(0, 0, 2) } }));
			Assert.IsTrue(actual.Timeseries["tag4"].IsEqual(new Dictionary<DateTime, TimeseriesPeriod> { { new DateTime(2009, 06, 21), new TimeseriesPeriod(0, 1, 0) }, { new DateTime(2010, 06, 27), new TimeseriesPeriod(0, 0, 1) } }));

		}
		
	}
}
