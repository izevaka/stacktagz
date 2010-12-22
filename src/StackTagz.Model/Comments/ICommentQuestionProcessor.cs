using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Comments {
	public interface ICommentQuestionProcessor {
		IEnumerable<CommentInfo> ConvertAnswersToQuestions(string site, IEnumerable<CommentInfo> infos);
	}
}
