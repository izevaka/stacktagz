using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Stacky;
using StackTagz.Repository;
using StackTagz.ServerRepository.StackClientFactory;
using StackTagz.ServerRepository;

namespace StackTagz.ServerRepository.Test {
	public class ServerRepositoryTestBase {

		Mock<IStackClientFactory> m_StackClientFactoryMock;

		public Mock<IStackClientFactory> StackClientFactoryMock {
			get { return m_StackClientFactoryMock; }
		}
		Mock<StackyClient> m_StackClient;
		Mock<StackAuthClient> m_StackAuthClient;

		public Mock<StackAuthClient> StackAuthClient {
			get { return m_StackAuthClient; }
		}

		public Mock<StackyClient> StackClient {
			get { return m_StackClient; }
		}

		public void InitFactory() {

			var protocolMock = new Mock<IProtocol>();
			var clientMock = new Mock<IUrlClient>();
			m_StackClient = new Mock<StackyClient>(ApiSettings.Version, null, "testsite", clientMock.Object, protocolMock.Object);
			m_StackAuthClient = new Mock<StackAuthClient>(clientMock.Object, protocolMock.Object);
			
			m_StackClientFactoryMock = new Mock<IStackClientFactory>();
			m_StackClientFactoryMock.Setup(m => m.GetClient(It.IsAny<string>())).Returns(m_StackClient.Object);
			m_StackClientFactoryMock.Setup(m => m.GetAuthClient()).Returns(m_StackAuthClient.Object);
		}

	}
}
