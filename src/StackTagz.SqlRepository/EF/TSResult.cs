using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.SqlRepository.EF {
	public class TSResult {

		public TSResult() {
			TSPeriods = new List<TSPeriod>();
		}

		public int Id { get; set; }
		public string Site { get; set; }
		public int UserId { get; set; }
		public DateTime LastFetched { get; set; }
		public int Rollup { get; set; }
		public int AnswerRequests { get; set; }
		public int QuestionRequests { get; set; }
		public int CommentsRequests { get; set; }
		public int VoteRequests { get; set; }

		public virtual List<TSPeriod> TSPeriods { get; set; }
	}
}
