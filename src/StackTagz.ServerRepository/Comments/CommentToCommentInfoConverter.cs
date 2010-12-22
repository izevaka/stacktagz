using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacky;
using StackTagz.Model.Answers;
using StackTagz.Model.Comments;
using StackTagz.Infrastructure;

namespace StackTagz.ServerRepository.Comments {
	public class CommentToCommentInfoConverter : IDataConverter<Comment, CommentInfo> {
		#region IDataConverter<Comment, CommentInfo> Members

		public CommentInfo Convert(Comment item) {
			if (item.PostType != PostType.Question && item.PostType != PostType.Answer) {
				this.GetLogger().Warn(m=>m("Unexpected comment type, only Question and Answer types are supported"));
				return null;
			}
			return new CommentInfo { CreationDate = item.CreationDate, Id = item.PostId, IsQuestion = item.PostType == PostType.Question };
		}

		#endregion

		public virtual void TestMethod(){
		}
	}
}
