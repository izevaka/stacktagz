using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Infrastructure {
	public class DateTimeBase {
		#region IDateTimeWrapper Members

		public virtual DateTime Now {
			get {
				return DateTime.Now;
			}
		}

		#endregion
	}
}
