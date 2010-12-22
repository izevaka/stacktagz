using StackTagz.Model.Questions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StackTagz.Model.Test
{
	[TestClass()]
	public class QuestionInfoTest : EqualityTestBase<QuestionInfo>{
		#region EqualityTestBase overrides
		[TestMethod]
		public override void EqualsShouldBeTrueForDefault() {
			DoEqualsShouldBeTrueForDefault();
		}
		[TestMethod]
		public override void EqualsShouldBeFalseForOtherNull() {
			DoEqualsShouldBeFalseForOtherNull();
		}
		[TestMethod]
		public override void EqualsShouldBeFalseForOtherType() {
			DoEqualsShouldBeFalseForOtherType();
		}
		#endregion

		[TestMethod()]
		public void CtorShouldInitTags() {
			QuestionInfo target = new QuestionInfo();
			Assert.IsNotNull(target.Tags);
		}

		[TestMethod()]
		public void EqualsShouldBeTrueForBothTagsNull() {
			QuestionInfo target = new QuestionInfo { Tags = null };
			Assert.IsTrue(target.Equals(new QuestionInfo { Tags = null }));
		}
		[TestMethod()]
		public void EqualsShouldBeFalseForDifferentTags() {
			QuestionInfo target = new QuestionInfo();
			Assert.IsFalse(target.Equals(new QuestionInfo() { Tags = new System.Collections.Generic.List<string> { "tag1", "tag2"} }));
		}
		[TestMethod()]
		public void EqualsShouldBeFalseForOtherTagsNull() {
			QuestionInfo target = new QuestionInfo();
			Assert.IsFalse(target.Equals(new QuestionInfo() { Tags = null}));
		}
		[TestMethod()]
		public void EqualsShouldBeFalseForThisTagsNull() {
			QuestionInfo target = new QuestionInfo() { Tags = null };
			Assert.IsFalse(target.Equals(new QuestionInfo()));
		}


		[TestMethod()]
		public void GetHashCodeShouldntCrashOnNullTags() {
			QuestionInfo target = new QuestionInfo { Tags = null}; 
			int actual;
			actual = target.GetHashCode();
			
		}
	}
}
