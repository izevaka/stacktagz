using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Answers{
	public class AnswerInfo {
		public DateTime CreationDate { get; set; }
		public int QuestionId { get; set; }
		public int AnswerId { get; set; }

		#region Equality Comparison
		public override bool Equals(object obj) {

			var other = obj as AnswerInfo;
			if(other == null) {
				return false;
			}
			return CreationDate == other.CreationDate &&
					QuestionId == other.QuestionId && 
					AnswerId == other.AnswerId;
		}


		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 23 + CreationDate.GetHashCode();
			hash = hash * 23 + QuestionId.GetHashCode();
			hash = hash * 23 + AnswerId.GetHashCode();
			return hash;
		} 
		#endregion
		
	}
}
