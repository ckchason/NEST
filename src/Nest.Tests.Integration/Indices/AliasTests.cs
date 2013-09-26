﻿using NUnit.Framework;

namespace Nest.Tests.Integration.Indices
{
	[TestFixture]
	public class AliasTest : IntegrationTests
	{
		[Test]
		public void SimpleAddRemoveAlias()
		{
			var index = ElasticsearchConfiguration.DefaultIndex;
			var alias = ElasticsearchConfiguration.DefaultIndex + "-2";

			var r = this._client.Alias(index, alias);
			Assert.True(r.IsValid);
			Assert.True(r.OK);
			Assert.True(r.Acknowledged);
			var count1 = this._client.Count<dynamic>(c => c.OnIndex(index));
      var count2 = this._client.Count<dynamic>(c => c.OnIndex(alias));
			Assert.AreEqual(count1.Count, count2.Count);
			r = this._client.RemoveAlias(index, alias);
      count1 = this._client.Count<dynamic>(c => c.OnIndex(index));
      count2 = this._client.Count<dynamic>(c => c.OnIndex(index));
			Assert.AreNotEqual(count1.Count, count2.Count);
			Assert.False(count2.IsValid);
		}
		[Test]
		public void SimpleRenameAlias()
		{
			var index = ElasticsearchConfiguration.DefaultIndex;
			var alias = ElasticsearchConfiguration.DefaultIndex + "-2";

			var r = this._client.Alias(index, alias);
			Assert.True(r.IsValid);
			Assert.True(r.OK);
			Assert.True(r.Acknowledged);
      var count1 = this._client.Count<dynamic>(c => c.OnIndex(index));
      var count2 = this._client.Count<dynamic>(c => c.OnIndex(alias));
			Assert.AreEqual(count1.Count, count2.Count);

			var renamed = index + "-3";

			r = this._client.Rename(index, alias, renamed);
      count1 = this._client.Count<dynamic>(c => c.OnIndex(index));
      count2 = this._client.Count<dynamic>(c => c.OnIndex(alias));
      var count3 = this._client.Count<dynamic>(c => c.OnIndex(renamed));
			Assert.AreEqual(count1.Count, count3.Count);
			Assert.AreNotEqual(count1.Count, count2.Count);
			Assert.False(count2.IsValid);

			r = this._client.RemoveAlias(index, renamed);
      count1 = this._client.Count<dynamic>(c => c.OnIndex(index));
      count3 = this._client.Count<dynamic>(c => c.OnIndex(renamed));
			Assert.AreNotEqual(count1.Count, count3.Count);
			Assert.False(count3.IsValid);
		}
	}
}