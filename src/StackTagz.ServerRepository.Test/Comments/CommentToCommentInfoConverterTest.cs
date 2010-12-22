using StackTagz.ServerRepository.Comments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stacky;
using StackTagz.Model.Comments;
using Moq;
using Common.Logging;
using StackTagz.Infrastructure;
using System.Linq;

namespace StackTagz.Repository.Test.Comments
{

	[TestClass()]
	public class CommentToCommentInfoConverterTest {

		[TestMethod]
		public void ConvertShouldConvertAnswer() {
			Comment item = new Comment { PostId = 123, PostType = PostType.Answer, CreationDate = new DateTime(2010, 06, 23) };
			CommentInfo expected = new CommentInfo { CreationDate = new DateTime(2010, 06, 23), Id = 123, IsQuestion = false };

			DoConvert(item, expected);
		}
		[TestMethod]
		public void ConvertShouldConvertQuestion() {
			Comment item = new Comment { PostId = 123, PostType = PostType.Question, CreationDate = new DateTime(2010, 06, 23) };
			CommentInfo expected = new CommentInfo { CreationDate = new DateTime(2010, 06, 23), Id = 123, IsQuestion = true };

			DoConvert(item, expected);
		}
		[TestMethod]
		public void ConvertShouldReturnNullForNotQuestionOrAnswer() {
			Comment item = new Comment { PostId = 123, PostType = PostType.Comment, CreationDate = new DateTime(2010, 06, 23) };
			CommentInfo expected = null;

			DoConvert(item, expected);
		}
		[TestMethod]
		public void ConvertShouldLogForNotQuestionOrAnswer() {
			Comment item = new Comment { PostId = 123, PostType = PostType.Comment, CreationDate = new DateTime(2010, 06, 23) };
			CommentInfo expected = null;
			var logMock = new Mock<ILog>();
			logMock.Setup(l=>l.Warn(It.IsAny<Action<FormatMessageHandler>>()));
			LoggingExtensions.SetLogger(logMock.Object);

			DoConvert(item, expected);
			logMock.VerifyAll();
		}

		private void DoConvert(Comment item, CommentInfo expected) {
			CommentToCommentInfoConverter target = new CommentToCommentInfoConverter();
			var actual = target.Convert(item);
			Assert.AreEqual(expected, actual);
		}
	}
}
