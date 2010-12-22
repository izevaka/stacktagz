using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacky;
using StackTagz.Model.Questions;

namespace StackTagz.ServerRepository.Questions {
	public class QuestionToQuestionInfoConverter : IDataConverter<Question, QuestionInfo> {
		#region IDataConverter<Question,QuestionInfo> Members

		public QuestionInfo Convert(Question item) {
			return item.ToQuestionInfo();
		}

		#endregion
	}
}
