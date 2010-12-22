using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model;
using StackTagz.Infrastructure;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;
using StackTagz.Model.Data;
using StackTagz.Model.Comments;

namespace StackTagz.Summarizer {
	public class Summarizer : ISummarizer {
		private readonly IQuestionsRepository m_QuestionRepo;
		private readonly IAnswersRepository m_AnswerRepo;
		private readonly IPersistQuestionsRepository m_questionPersitRepo;
		private readonly IPersistTimeseriesRepository m_timeseriesPersistRepo;
		private readonly ICommentQuestionProcessor m_commentsProcessor;
		private readonly ICommentsRepository m_commentsRepository;

		public Summarizer(
			IQuestionsRepository questionRepo, 
			IAnswersRepository answerRepo, 
			IPersistQuestionsRepository questionPersitRepo, 
			IPersistTimeseriesRepository timeseriesPersistRepo,
			ICommentsRepository commentsRepository,
			ICommentQuestionProcessor commentsProcessor) {
			
			m_QuestionRepo = questionRepo;
			m_AnswerRepo = answerRepo;
			m_questionPersitRepo = questionPersitRepo;
			m_timeseriesPersistRepo = timeseriesPersistRepo;
			m_commentsRepository = commentsRepository;
			m_commentsProcessor = commentsProcessor;
		}

		class InterestPoint {
			public int QuestionId { get; set; }
			public DateTime Date { get; set; }
		}

		#region ITimeseriesRepository Members
		public TimeseriesResult GetTimeSeries(string site, int userId, DateTime? start, DateTime? end, Rollup rollup) {
			
			//See if the DB contains the whole TimeseriesResult
			var result = m_timeseriesPersistRepo.GetTimeSeries(site, userId, start, end, rollup);
			if(result != null)
				return result;
			this.GetLogger().Info(m=>m("Requesting timeseries for site {0}, user {1}".FormatString(site, userId)));
			var tsr = RequestTimeSeries(site, userId, start,end, rollup);
			m_timeseriesPersistRepo.SaveTimeseries(site, userId, tsr);
			return tsr;
		}
		#endregion
		public TimeseriesResult RequestTimeSeries(string site, int userId, DateTime? start, DateTime? end, Rollup rollup) {

			var timeSeriesResult = new TimeseriesResult(rollup);

			var questionInfos = m_QuestionRepo.Get(site, userId);
			m_questionPersitRepo.SaveQuestions(site, questionInfos);
			var questionPoints = from q in questionInfos
								 select q.ToTimeseriesPoint();

			var questionIds = from answer in m_AnswerRepo.Get(site, userId)
							  select new InterestPoint { QuestionId = answer.QuestionId, Date = answer.CreationDate };

			var answerPoints = GetInterestPoints(site, questionPoints, questionIds, PointType.Answer);

			if (answerPoints != null)
				questionPoints = questionPoints.Concat(answerPoints);

			var commentIds = from comment in m_commentsProcessor.ConvertAnswersToQuestions(site, m_commentsRepository.Get(site, userId))
							  select new InterestPoint { QuestionId = comment.Id, Date = comment.CreationDate };

			var commentPoints = GetInterestPoints(site, questionPoints, commentIds, PointType.Comment);
			if(commentPoints != null)
				questionPoints = questionPoints.Concat(commentPoints);

			
			
			foreach(var dataPoint in questionPoints) {
				foreach(var tag in dataPoint.Tags) {

					Timeseries ts;
					if(!timeSeriesResult.Timeseries.TryGetValue(tag, out ts))
						ts = timeSeriesResult.Timeseries[tag] = timeSeriesResult.CreateTimeseries();

					ts.Add(dataPoint.Date, dataPoint);
				}
			}
			
			return Filter(timeSeriesResult);

		}

		public TimeseriesResult Filter(TimeseriesResult timeSeriesResult) {
			//remove all tags with one question
			var ret = timeSeriesResult.Timeseries.OrderBy(kv => kv.Value.Count(), new DescendingOrderComparer()).Take(10).ToDictionary(kv=>kv.Key, kv=>kv.Value);

			return new TimeseriesResult(timeSeriesResult.RollupType,ret);
		}

		private IEnumerable<TimeseriesPoint> GetInterestPoints(string site, IEnumerable<TimeseriesPoint> questionPoints, IEnumerable<InterestPoint> questionIds, PointType type) {
			
			var filteredQuestionIds = questionIds
				.Where(qId => !questionPoints.Any(q => qId.QuestionId == q.QuestionId))
				.Select(qid => qid.QuestionId)
				.ToList();

			//ask persist qeustion repo for unknown questions
			var questionsFromAnswers = from qi in m_questionPersitRepo.Get(site, filteredQuestionIds)
									   select new TimeseriesPoint(qi.CreationDate, qi.Id, qi.Tags, type);

			IEnumerable<TimeseriesPoint> answerPoints = null;
			filteredQuestionIds = filteredQuestionIds.Where(qId => !questionsFromAnswers.Any(q => q.QuestionId == qId)).ToList();
			if(filteredQuestionIds.Count() > 0) {
				var unknownQuestions = m_QuestionRepo.Get(site, filteredQuestionIds);
				m_questionPersitRepo.SaveQuestions(site, unknownQuestions);
				answerPoints = from question in unknownQuestions
							   let answerDate = questionIds.Where(qid => qid.QuestionId == question.Id).FirstOrDefault().Date
							   select new TimeseriesPoint(answerDate, question.Id, question.Tags, type);
				answerPoints = answerPoints.Concat(questionsFromAnswers);
			}
			return answerPoints;
		}
	}
}
