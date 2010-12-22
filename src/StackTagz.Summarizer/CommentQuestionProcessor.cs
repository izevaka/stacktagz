using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Comments;
using StackTagz.Model.Answers;

namespace StackTagz.Summarizer {
	public class CommentQuestionProcessor : ICommentQuestionProcessor {
		private IAnswersRepository m_answerRepo;

		public CommentQuestionProcessor(IAnswersRepository answerRepo) {
			m_answerRepo = answerRepo;
		}

		#region ICommentQuestionProcessor Members

		public IEnumerable<CommentInfo> ConvertAnswersToQuestions(string site, IEnumerable<CommentInfo> infos) {
			var answerComments = infos.Where(i => !i.IsQuestion);


			//discard items that we don't know about
			var existingItems = m_answerRepo.Get(site, answerComments.Select(i => i.Id))
				.Where((a => answerComments.Any(c => c.Id == a.AnswerId)));

#if DEBUG
			var missingItems = m_answerRepo.Get(site, answerComments.Select(i => i.Id))
				.Where((a => !answerComments.Any(c => c.Id == a.AnswerId)));

			System.Diagnostics.Debug.WriteLine("Missing ids: " + string.Join(",", missingItems.Select(i => i.AnswerId)));
#endif


			var answerInfos = existingItems
				.Select(a => new CommentInfo { Id = a.QuestionId, IsQuestion = true, CreationDate = answerComments.First(c=>c.Id == a.AnswerId).CreationDate});

			return infos.Where(i => i.IsQuestion).Concat(answerInfos);
		}

		#endregion
	}
}
