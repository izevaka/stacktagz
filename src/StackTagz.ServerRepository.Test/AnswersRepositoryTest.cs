using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stacky;
using System.Collections.Generic;
using Moq;
using StackTagz.ServerRepository;
using StackTagz.Model;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;
using StackTagz.ServerRepository.Answers;

namespace StackTagz.ServerRepository.Test
{
	[TestClass()]
	public class AnswersRepositoryTest : ServerRepositoryTestBase{

		private const int c_TestId = 123;

		#region Additional test attributes
		
		[TestInitialize]
		public void TestInitialize() {
			InitFactory();
		}

		#endregion


		[TestMethod()]
		public void GetShouldReturnOnePage() {
			AnswersRepository target = new AnswersRepository(StackClientFactoryMock.Object);

			StackClient.Setup(c =>
				c.GetUsersAnswers(c_TestId, QuestionsByUserSort.Activity, SortDirection.Descending, 1, ApiSettings.PageSize, false, false, null, null, null, null))
				.Returns(new PagedList<Answer>(new List<Answer>() {
						new Answer {QuestionId = 1},
						new Answer {QuestionId = 2}}
				) { PageSize = 2, TotalItems = 2});

			List<AnswerInfo> actual;
			actual = target.Get("siteaddr", c_TestId);

			StackClient.VerifyAll();
			var expected = new List<AnswerInfo>() {
						new AnswerInfo {QuestionId = 1},
						new AnswerInfo {QuestionId = 2}
					};

			CollectionAssert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void GetShouldReturnTwoPages() {
			AnswersRepository target = new AnswersRepository(StackClientFactoryMock.Object);

			StackClient.Setup(c =>
				c.GetUsersAnswers(c_TestId, QuestionsByUserSort.Activity, SortDirection.Descending, 1, ApiSettings.PageSize, false, false, null, null, null, null))
				.Returns(new PagedList<Answer>(new List<Answer>() {
						new Answer {QuestionId = 1},
						new Answer {QuestionId = 2}}
				) { PageSize = 2, TotalItems = 5 });

			StackClient.Setup(c =>
				c.GetUsersAnswers(c_TestId, QuestionsByUserSort.Activity, SortDirection.Descending, 2, ApiSettings.PageSize, false, false, null, null, null, null))
				.Returns(new PagedList<Answer>(new List<Answer>() {
						new Answer {QuestionId = 3},
						new Answer {QuestionId = 4}}
				) { PageSize = 2, TotalItems = 5 });

			StackClient.Setup(c =>
				c.GetUsersAnswers(c_TestId, QuestionsByUserSort.Activity, SortDirection.Descending, 3, ApiSettings.PageSize, false, false, null, null, null, null))
				.Returns(new PagedList<Answer>(new List<Answer>() {
						new Answer {QuestionId = 5}}
				) { PageSize = 2, TotalItems = 5 });

			List<AnswerInfo> actual;
			actual = target.Get("siteaddr", c_TestId);

			StackClient.VerifyAll();
			var expected = new List<AnswerInfo>() {
						new AnswerInfo {QuestionId = 1},
						new AnswerInfo {QuestionId = 2},
						new AnswerInfo {QuestionId = 3},
						new AnswerInfo {QuestionId = 4},
						new AnswerInfo {QuestionId = 5}
					};

			CollectionAssert.AreEqual(expected, actual);
		}
	}
}
