using StackTagz.SqlRepository.EF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StackTagz.Model.Questions;

namespace StackTagz.SqlRepository.Test
{
	[TestClass()]
	public class QuestionTest {

		[TestMethod()]
		public void SetFromQuestionInfoTest() {
			Question expected = new Question { Date = new DateTime(2010, 06, 23),QuestionId = 45, Site="site1", TagsList = "tag1,tag2" };
			QuestionInfo qInfo = new QuestionInfo { CreationDate = new DateTime(2010, 06, 23), Id = 45, Tags = new System.Collections.Generic.List<string> { "tag1", "tag2" } }; 
			var target = new Question();
			target.SetFromQuestionInfo("site1", qInfo);

			Assert.AreEqual(expected, target);
			
		}

		[TestMethod()]
		public void ToQuestionInfoShouldSplitTags() {
			Question target = new Question { Date = new DateTime(2010, 06,23), Id = 1, QuestionId = 45, Site = "site", TagsList="tag1,tag2"};
			QuestionInfo expected = new QuestionInfo { CreationDate = new DateTime(2010, 06, 23), Id = 45, Tags = new System.Collections.Generic.List<string> { "tag1", "tag2"} }; 
			
			var actual = target.ToQuestionInfo();
			Assert.AreEqual(expected, actual);
		}
		[TestMethod()]
		public void ToQuestionInfoShouldReturnEmptyTagsForNullTagsList() {
			Question target = new Question { Date = new DateTime(2010, 06, 23), Id = 1, QuestionId = 45, Site = "site", TagsList = null };
			QuestionInfo expected = new QuestionInfo { CreationDate = new DateTime(2010, 06, 23), Id = 45, Tags = new System.Collections.Generic.List<string> { } };

			var actual = target.ToQuestionInfo();
			Assert.AreEqual(expected, actual);
		}
		[TestMethod()]
		public void ToQuestionInfoShouldReturnEmptyTagsForEmptyTagsList() {
			Question target = new Question { Date = new DateTime(2010, 06, 23), Id = 1, QuestionId = 45, Site = "site", TagsList = String.Empty };
			QuestionInfo expected = new QuestionInfo { CreationDate = new DateTime(2010, 06, 23), Id = 45, Tags = new System.Collections.Generic.List<string> { } };

			var actual = target.ToQuestionInfo();
			Assert.AreEqual(expected, actual);
		}
	}
}
