using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StackTagz.Model.Test {
	
	public abstract class EqualityTestBase<T> where T : class, new(){
		
		public void DoEqualsShouldBeTrueForDefault() {
			T target = new T();
			Assert.IsTrue(target.Equals(new T()));
		}
		
		
		public void DoEqualsShouldBeFalseForOtherNull() {
			T target = new T();
			Assert.IsFalse(target.Equals(null));
		}
		
		
		public void DoEqualsShouldBeFalseForOtherType() {
			T target = new T();
			Assert.IsFalse(target.Equals(new Object()));
		}

		public abstract void EqualsShouldBeTrueForDefault();
		public abstract void EqualsShouldBeFalseForOtherNull();
		public abstract void EqualsShouldBeFalseForOtherType();
	}
}
