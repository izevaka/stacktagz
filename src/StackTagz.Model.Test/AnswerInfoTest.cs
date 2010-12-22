using StackTagz.Model.Answers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StackTagz.Model.Test
{
	[TestClass()]
	public class AnswerInfoTest : EqualityTestBase<AnswerInfo>{
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
	}
}
