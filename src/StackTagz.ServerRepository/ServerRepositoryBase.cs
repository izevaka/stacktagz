using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacky;
using StackTagz.ServerRepository.StackClientFactory;

namespace StackTagz.ServerRepository {
	public class ServerRepositoryBase {
				private readonly IStackClientFactory m_Factory;

		public IStackClientFactory ClientFactory {
			get { return m_Factory; }
		} 

		public ServerRepositoryBase(IStackClientFactory factory) {
			m_Factory = factory;
		}

	}

	
	public abstract class ServerListRepositoryBase<TInput, TOutput> : ServerRepositoryBase {
		public ServerListRepositoryBase(IStackClientFactory factory) :base(factory){
		}

		#region Abstract Members
		/// <summary>
		/// Inheriting classes must override this method
		/// </summary>
		/// <param name="client"></param>
		/// <param name="userId"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		protected abstract IPagedList<TInput> RequestData(StackyClient client, int userId, int page);
		/// <summary>
		/// Inheriting classes can chose override this method
		/// </summary>
		/// <param name="client"></param>
		/// <param name="idList"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		protected virtual IPagedList<TInput> RequestData(StackyClient client, IEnumerable<int> idList) {
			throw new InvalidOperationException("This method must be overriden.");
		}
		protected abstract IDataConverter<TInput, TOutput> Converter { get; }
		#endregion


		#region IDataRepository<T> Members
		public List<TOutput> Get(string site, int userId) {
			StackyClient client = ClientFactory.GetClient(site);

			var result = new List<TOutput>();
			var page = 1;
			while(true) {
				var anwers = RequestData(client, userId, page);
				page++;
				if(anwers != null) {
					result.AddRange(anwers.Select(a => Converter.Convert(a)));
					if(result.Count >= anwers.TotalItems)
						break;
				} else {
					//Something's gone wrong (likely the test), the return should never be null, bail out
					break;
				}
			}

			return result;

		}

		public List<TOutput> Get(string site, IEnumerable<int> itemIds) {
			StackyClient client = ClientFactory.GetClient(site);

			var result = new List<TOutput>();

			var processedIds = 0;

			while(true) {
				var batch = itemIds.Skip(processedIds).Take(ApiSettings.IdListRequestPageSize);
				processedIds += batch.Count();
				if(batch.Count() == 0)
					break;

				
				var items = RequestData(client, batch);
				
				if(items != null) {
					result.AddRange(items.Select(q => Converter.Convert(q)));

				} else {
					//Something is wrong, bail out
					break;
				}
				

				if(processedIds >= itemIds.Count())
					break;
			}

			return result;

		}


		#endregion

		
	}

}
