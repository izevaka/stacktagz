using StackTagz.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stacky;
using System.Collections.Generic;
using Moq;
using StackTagz.Infrastructure;
using StackTagz.ServerRepository;
using StackTagz.Model;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;
using StackTagz.ServerRepository.Questions;

namespace StackTagz.ServerRepository.Test
{
    [TestClass()]
	public class QuestionsRepositoryTest : ServerRepositoryTestBase{
		private const int c_TestId = 123;

		#region Additional test attributes

		[TestInitialize]
		public void TestInitialize()
		{
			InitFactory();
		}
		
		[TestCleanup]
		public void TestCleanup()
		{
		}
		#endregion


		/// <summary>
		///A test for Get
		///</summary>
		[TestMethod()]
		public void GetShouldReturnListOfQuestionsOnePage() {
			QuestionsRepository target = new QuestionsRepository(StackClientFactoryMock.Object);

			StackClient.Setup(c =>
				c.GetQuestionsByUser(c_TestId, QuestionsByUserSort.Activity, SortDirection.Descending, 1, ApiSettings.PageSize, false, false, null, null, null, null, null))
				.Returns(new PagedList<Question>(new List<Question>() {
						new Question {Id = 1},
						new Question {Id = 2}}
				) { PageSize = 2, TotalItems = 2 });

			List<QuestionInfo> actual;
			actual = target.Get("siteaddr", c_TestId);

			StackClient.VerifyAll();
			var expected = new List<QuestionInfo>() {
						new QuestionInfo {Id = 1},
						new QuestionInfo {Id = 2}
					};

			CollectionAssert.AreEqual(expected, actual);
		}
		[TestMethod()]
		public void GetShouldReturnListOfQuestionsThreePages() {
			QuestionsRepository target = new QuestionsRepository(StackClientFactoryMock.Object);

			StackClient.Setup(c =>
				c.GetQuestionsByUser(c_TestId, QuestionsByUserSort.Activity, SortDirection.Descending, 1, ApiSettings.PageSize, false, false, null, null, null, null, null))
				.Returns(new PagedList<Question>(new List<Question>() {
						new Question {Id = 1},
						new Question {Id = 2}}
				) { PageSize = 2, TotalItems = 5 });

			StackClient.Setup(c =>
				c.GetQuestionsByUser(c_TestId, QuestionsByUserSort.Activity, SortDirection.Descending, 2, ApiSettings.PageSize, false, false, null, null, null, null, null))
				.Returns(new PagedList<Question>(new List<Question>() {
						new Question {Id = 3},
						new Question {Id = 4}}
				) { PageSize = 2, TotalItems = 5 });
			
			StackClient.Setup(c =>
				c.GetQuestionsByUser(c_TestId, QuestionsByUserSort.Activity, SortDirection.Descending, 3, ApiSettings.PageSize, false, false, null, null, null, null, null))
				.Returns(new PagedList<Question>(new List<Question>() {
						new Question {Id = 5}}
				) { PageSize = 2, TotalItems = 5 });

			List<QuestionInfo> actual;
			actual = target.Get("siteaddr", c_TestId);

			StackClient.VerifyAll();
			var expected = new List<QuestionInfo>() {
						new QuestionInfo {Id = 1},
						new QuestionInfo {Id = 2},
						new QuestionInfo {Id = 3},
						new QuestionInfo {Id = 4},
						new QuestionInfo {Id = 5}
					};

			CollectionAssert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetbyIdShouldRequestListIds() {
			QuestionsRepository target = new QuestionsRepository(StackClientFactoryMock.Object);

			StackClient.Setup(c =>
				c.GetQuestions(It.Is<IEnumerable<int>>(arr => arr.IsEqual(new[] { 0, 1, 2, 3, 4, 5 })), It.IsAny<int?>(), It.IsAny<int?>(), false, false, null, null, null, null, null))
				.Returns(new PagedList<Question>(new List<Question>() {
		                new Question {Id = 1},
		                new Question {Id = 2},
		                new Question {Id = 3},
						new Question {Id = 4},
		                new Question {Id = 5},
		                new Question {Id = 6}}

				) { PageSize = 6, TotalItems = 6 });


			int[] ids = new[] { 0, 1, 2, 3, 4, 5 };

			List<QuestionInfo> actual;
			actual = target.Get("siteaddr", ids);

			StackClient.VerifyAll();
			var expected = new List<QuestionInfo>() {
		                new QuestionInfo {Id = 1},
		                new QuestionInfo {Id = 2},
		                new QuestionInfo {Id = 3},
		                new QuestionInfo {Id = 4},
		                new QuestionInfo {Id = 5},
		                new QuestionInfo {Id = 6}
		            };

			CollectionAssert.AreEqual(expected, actual);
		}

	}
}
