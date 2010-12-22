using System;
using Stacky;

namespace StackTagz.ServerRepository.StackClientFactory {
	public interface IStackClientFactory {
		StackyClient GetClient(string site);
		StackAuthClient GetAuthClient();
	}
}
