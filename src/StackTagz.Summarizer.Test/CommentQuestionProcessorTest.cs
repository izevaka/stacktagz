using StackTagz.Summarizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StackTagz.Model.Answers;
using StackTagz.Model.Comments;
using System.Collections.Generic;
using StackTagz.Infrastructure;
using Moq;
using System.Linq;

namespace StackTagz.Summarizer.Test
{
	[TestClass()]
	public class CommentQuestionProcessorTest {
		private Mock<IAnswersRepository> m_AnswerRepo;
		private CommentInfo[] m_infos;

		[TestInitialize]
		public void TestInti() {
			m_AnswerRepo = new Mock<IAnswersRepository>();
			m_AnswerRepo.Setup(ar=>ar.Get(It.IsAny<string>(), It.IsAny<IEnumerable<int>>())).Returns(
				new List<AnswerInfo>{
					new AnswerInfo{AnswerId = 1, QuestionId = 21, CreationDate = new DateTime(2010,06,26)},
					new AnswerInfo{AnswerId = 3, QuestionId = 23, CreationDate = new DateTime(2010,06,27)},
					new AnswerInfo{AnswerId = 5, QuestionId = 25, CreationDate = new DateTime(2010,06,28)},
				}
				);
			m_infos = new[]{
				new CommentInfo{Id = 1, CreationDate = new DateTime(2010, 06,21), IsQuestion = false},
				new CommentInfo{Id = 2, CreationDate = new DateTime(2010, 06,22), IsQuestion = true},
				new CommentInfo{Id = 3, CreationDate = new DateTime(2010, 06,23), IsQuestion = false},
				new CommentInfo{Id = 4, CreationDate = new DateTime(2010, 06,24), IsQuestion = true},
				new CommentInfo{Id = 5, CreationDate = new DateTime(2010, 06,25), IsQuestion = false},
			}; 
		}

		[TestMethod]
		public void ConvertAnswersToQuestionsShouldRequestAnswerIds() {
			CommentQuestionProcessor target = new CommentQuestionProcessor(m_AnswerRepo.Object); 
			target.ConvertAnswersToQuestions("site", m_infos);
			m_AnswerRepo.Verify(r=>r.Get("site", new []{1,3,5}));
		}

		[TestMethod]
		public void ConvertAnswersToQuestionsShouldUseCommentDate() {
			CommentQuestionProcessor target = new CommentQuestionProcessor(m_AnswerRepo.Object);
			var comments = target.ConvertAnswersToQuestions("site", m_infos);
			CollectionAssert.AreEquivalent(new []{
				new CommentInfo{Id = 21, CreationDate = new DateTime(2010, 06,21), IsQuestion = true},
				new CommentInfo{Id = 2, CreationDate = new DateTime(2010, 06,22), IsQuestion = true},
				new CommentInfo{Id = 23, CreationDate = new DateTime(2010, 06,23), IsQuestion = true},
				new CommentInfo{Id = 4, CreationDate = new DateTime(2010, 06,24), IsQuestion = true},
				new CommentInfo{Id = 25, CreationDate = new DateTime(2010, 06,25), IsQuestion = true},
			}, comments.ToList());
		}


		[TestMethod]
		public void ConvertAnswersToQuestionsShouldHandleMissingQuestions() {
			CommentQuestionProcessor target = new CommentQuestionProcessor(m_AnswerRepo.Object);
			var extraInfos = m_infos.Concat(new[] { new CommentInfo { Id = 12345, IsQuestion = false, CreationDate = new DateTime(2010, 06, 29) } });
			target.ConvertAnswersToQuestions("site", extraInfos);
			//No assert, shouldn't crash
		}
	}
}
