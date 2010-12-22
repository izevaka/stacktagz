using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacky;
using StackTagz.Model.Answers;

namespace StackTagz.ServerRepository.Answers {
	public class AnswerToAnswerInfoConverter : IDataConverter<Answer, AnswerInfo> {
		#region IDataConverter<Answer, AnswerInfo> Members

		public AnswerInfo Convert(Answer item) {
			return new AnswerInfo { CreationDate = item.CreationDate, QuestionId = item.QuestionId };
		}

		#endregion
	}
}
