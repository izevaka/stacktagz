using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.SqlRepository.EF {
	public class TSPeriod {
		public int Id { get; set; }
		public int TSResultId { get; set; }
		public DateTime Date { get; set; }
		public string tag { get; set; }
		public int AnswerCount { get; set; }
		public int QuestionCount { get; set; }
		public int CommentCount { get; set; }
		public int VoteCount { get; set; } 

		//navigation
		public virtual TSResult TSResult { get; set; }

		internal Model.Data.TimeseriesPeriod ToTimeseriesPeriod() {
			return new Model.Data.TimeseriesPeriod(QuestionCount, AnswerCount, CommentCount);
		}

		internal void SetFromTimeseriesPeriod(string tag, DateTime date, Model.Data.TimeseriesPeriod timeseriesPeriod) {
			this.tag = tag;
			this.Date = date;
			this.QuestionCount = timeseriesPeriod.QuestionCount;
			this.AnswerCount = timeseriesPeriod.AnswerCount;
			this.CommentCount = timeseriesPeriod.CommentCount;
		}

		#region Value Equality
		public override bool Equals(object obj) {

			var other = obj as TSPeriod;
			if(other == null) {
				return false;
			}

			return
				Id == other.Id &&
				TSResultId == other.TSResultId &&
				Date == other.Date &&
				tag == other.tag &&
				AnswerCount == other.AnswerCount &&
				QuestionCount == other.QuestionCount &&
				CommentCount == other.CommentCount &&
				TSResult == other.TSResult;
		}


		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 23 + Id.GetHashCode();
			hash = hash * 23 + TSResultId.GetHashCode();
			hash = hash * 23 + Date.GetHashCode();
			hash = hash * 23 + ((tag != null) ? tag.GetHashCode() : 0);
			hash = hash * 23 + AnswerCount.GetHashCode();
			hash = hash * 23 + QuestionCount.GetHashCode();
			hash = hash * 23 + CommentCount.GetHashCode();
			hash = hash * 23 + ((TSResult != null) ? TSResult.GetHashCode() : 0);

			return hash;
		}
    
    
		#endregion
	}
}
