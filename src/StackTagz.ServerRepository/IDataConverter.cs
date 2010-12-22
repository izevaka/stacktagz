using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Answers;

namespace StackTagz.ServerRepository {
	public interface IDataConverter<TInput, TOutput> {
		TOutput Convert(TInput item);
	}
}
