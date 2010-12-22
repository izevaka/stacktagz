using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacky;
using StackTagz.Model.Answers;
using StackTagz.ServerRepository.StackClientFactory;
using StackTagz.Model.Comments;

namespace StackTagz.ServerRepository.Comments {
	public class CommentsRepository : ServerListRepositoryBase<Comment, CommentInfo>, ICommentsRepository {

		public CommentsRepository(IStackClientFactory factory)
			: base(factory) {
		}

		private IDataConverter<Comment, CommentInfo> m_Converter = new CommentToCommentInfoConverter();
		
		protected override IPagedList<Comment> RequestData(StackyClient client, int userId, int page) {
			return client.GetComments(userId, page: page);
		}

		protected override IDataConverter<Comment, CommentInfo> Converter {
			get { return m_Converter; }
		}

		protected override IPagedList<Comment> RequestData(StackyClient client, IEnumerable<int> idList) {
			throw new NotImplementedException();
		}
	}
}
