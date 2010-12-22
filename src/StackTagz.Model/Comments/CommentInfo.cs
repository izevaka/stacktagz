using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Comments {
	public class CommentInfo {
		public DateTime CreationDate { get; set; }
		public int Id { get; set; }
		public bool IsQuestion { get; set; }

		public override bool Equals(object obj) {

			var other = obj as CommentInfo;
			if(other == null) {
				return false;
			}

			return Id == other.Id &&
				CreationDate == other.CreationDate &&
				IsQuestion == other.IsQuestion;
		}


		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 23 +  CreationDate.GetHashCode();
			hash = hash * 23 + Id.GetHashCode();
			hash = hash * 23 + IsQuestion.GetHashCode();
			return hash;
		}
    
    
	}
}
