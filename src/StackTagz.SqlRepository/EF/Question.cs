using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Questions;

namespace StackTagz.SqlRepository.EF {
	public class Question {
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string Site { get; set; }
		public int QuestionId { get; set; }
		public string TagsList { get; set; }


		internal QuestionInfo ToQuestionInfo() {
			return new QuestionInfo() { CreationDate = Date, Id = QuestionId, Tags = TagsList != null ? TagsList.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>()};
		}

		internal void SetFromQuestionInfo(string site, QuestionInfo qInfo) {
			this.QuestionId = qInfo.Id;
			this.Date = qInfo.CreationDate;
			this.Site = site;
			this.TagsList = String.Join(",", qInfo.Tags);
		}

		#region Equality
		public override bool Equals(object obj) {

			var other = obj as Question;
			if(other == null) {
				return false;
			}

			return
				Id == other.Id &&
				Date == other.Date &&
				Site == other.Site &&
				QuestionId == other.QuestionId &&
				TagsList == other.TagsList;
		}


		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 23 + Id.GetHashCode();
			hash = hash * 23 + Date.GetHashCode();
			hash = hash * 23 + QuestionId.GetHashCode();
			hash = hash * 23 + ((Site != null) ? Site.GetHashCode() : 0);
			hash = hash * 23 + ((TagsList != null) ? TagsList.GetHashCode() : 0);
			return hash;
		}
    
    
		#endregion
	}
}
