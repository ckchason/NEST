﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nest;
using Nest.Tests.MockData;
using Nest.Tests.MockData.Domain;
using NUnit.Framework;
using Nest.Resolvers;

namespace Nest.Tests.Integration
{
	public class IntegrationTests
	{
		protected readonly ElasticClient _client = ElasticsearchConfiguration.Client;
		protected readonly ElasticClient _thriftClient = ElasticsearchConfiguration.ThriftClient;
		protected readonly IConnectionSettings _settings = ElasticsearchConfiguration.Settings();
	
		protected virtual void ResetIndexes()
		{
			
		}

		protected IQueryResponse<T> SearchRaw<T>(string query) where T : class
		{
			var index = this._client.Infer.IndexName<T>();
			var typeName = this._client.GetTypeNameFor<T>();
			var connectionStatus = this._client.Raw.SearchPost(index, typeName, query);
			return connectionStatus.Deserialize<QueryResponse<T>>();
		} 

		public void DoFilterTest(Func<FilterDescriptor<ElasticSearchProject>, Nest.BaseFilter> filter, ElasticSearchProject project, bool queryMustHaveResults)
		{
			var filterId = Filter<ElasticSearchProject>.Term(e => e.Id, project.Id);

			var results = this._client.Search<ElasticSearchProject>(
				s => s.Filter(ff => ff.And(
						f => f.Term(e => e.Id, project.Id),
						filter
					))
				);

			Assert.True(results.IsValid, results.ConnectionStatus.Result);
			Assert.True(results.ConnectionStatus.Success, results.ConnectionStatus.Result);
			Assert.AreEqual(queryMustHaveResults ? 1 : 0, results.Total);
		}
	
	}
}
