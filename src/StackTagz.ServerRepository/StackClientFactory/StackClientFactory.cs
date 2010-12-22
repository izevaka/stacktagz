using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacky;

namespace StackTagz.ServerRepository.StackClientFactory {
	public class StackClientFactory : IStackClientFactory {
		
		public StackyClient GetClient(string site) {
			return new StackyClient(ApiSettings.Version, ApiSettings.Key, site, new UrlClient(), new JsonProtocol());
		}

		#region IStackClientFactory Members


		public StackAuthClient GetAuthClient() {
			return new StackAuthClient(new UrlClient(), new JsonProtocol());
		}

		#endregion
	}
}
