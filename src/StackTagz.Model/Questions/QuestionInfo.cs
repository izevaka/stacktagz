using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Infrastructure;
using StackTagz.Model.Data;

namespace StackTagz.Model.Questions {
	public class QuestionInfo {

		public QuestionInfo() {
			Tags = new List<string>();
		}

		public DateTime CreationDate { get; set; }
		public int Id { get; set; }
		public List<string> Tags { get; set; }

		#region Equality Comparison
		public override bool Equals(object obj) {

			var other = obj as QuestionInfo;
			if(other == null) {
				return false;
			}
			return CreationDate == other.CreationDate &&
				Id == other.Id &&
				Tags == other.Tags || (Tags != null && Tags.IsEqual(other.Tags));
		}

		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 23 + CreationDate.GetHashCode();
			hash = hash * 23 + Id.GetHashCode();
			hash = hash * 23 + (Tags != null ? Tags.GetHashCode() : 0);
			return hash;
		}

		#endregion

		public TimeseriesPoint ToTimeseriesPoint() {
			return new TimeseriesPoint(CreationDate, Id, Tags, PointType.Question);
		}
	}
}
