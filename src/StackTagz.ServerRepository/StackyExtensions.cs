using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model;
using Stacky;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;

namespace StackTagz.ServerRepository {
	public static class StackyExtensions {
		public static QuestionInfo ToQuestionInfo(this Question question) {
			return new QuestionInfo { CreationDate = question.CreationDate, Id = (int)question.Id, Tags = question.Tags };
		}
		public static AnswerInfo ToAnswerInfo(this Answer answer) {
			return new AnswerInfo { CreationDate = answer.CreationDate, QuestionId = answer.QuestionId, AnswerId = answer.Id };
		}
		public static StackTagz.Model.Users.UserInfo ToUserInfo(this User user) {
			return new StackTagz.Model.Users.UserInfo {
				Name = user.DisplayName,
				Rep = user.Reputation,
				BronzeBadges = user.BadgeCounts.Bronze,
				SilverBadges = user.BadgeCounts.Silver,
				GoldBadges = user.BadgeCounts.Gold,
				GravatarUrl = user.GravatarUrl,
				UserId = user.Id
			};
		}
	}
}
