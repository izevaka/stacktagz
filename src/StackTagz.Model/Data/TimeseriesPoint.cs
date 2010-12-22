using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Data {

	public struct TimeseriesPoint {
		public TimeseriesPoint(DateTime date, long questionId, List<string> tags, PointType type) {
			m_Date = date;
			m_QuestionId = questionId;
			m_Type = type;
			m_Tags = tags;
		}

		private List<string> m_Tags;
		public List<string> Tags { get {return m_Tags;}}

		private DateTime m_Date;
		public DateTime Date {
			get { return m_Date; }
		}


		private long m_QuestionId;
		public long QuestionId {
			get { return m_QuestionId; }
		}

		private PointType m_Type;
		public PointType PointType {
			get { return m_Type; }
			set { m_Type = value; }
		}
	}
}
