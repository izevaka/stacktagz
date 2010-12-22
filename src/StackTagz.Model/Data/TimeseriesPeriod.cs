using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Data {
	public struct TimeseriesPeriod {

		public TimeseriesPeriod(int questionCount, int answerCount, int commentCount) {
			m_QuestionCount = questionCount;
			m_AnswerCount = answerCount;
			m_CommentCount = commentCount;
			m_Count = m_CommentCount + m_QuestionCount + m_AnswerCount;
		}


		private readonly int m_Count;
		public int Count { get { return m_Count; } }

		private readonly int m_QuestionCount;
		public int QuestionCount { get { return m_QuestionCount; } }

		private readonly int m_AnswerCount;
		public int AnswerCount { get { return m_AnswerCount; } }

		private readonly int m_CommentCount;
		public int CommentCount { get { return m_CommentCount; } }


		public static TimeseriesPeriod operator +(TimeseriesPeriod period, TimeseriesPoint point) {

			var questionCount = period.QuestionCount;
			var answerCount = period.AnswerCount;
			var commentCount = period.CommentCount;
			switch(point.PointType) {
				case PointType.Answer:
					answerCount++;
					break;
				case PointType.Question:
					questionCount++;
					break;
				case PointType.Comment:
					commentCount++;
					break;
			}

			return new TimeseriesPeriod(questionCount, answerCount, commentCount);
		}
	}
}
