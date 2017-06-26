using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleLINQ;

namespace AssetFlowByYearMonthTests
{
	[TestClass]
	public class UnitTest1
	{
		public SortedDictionary<int, SortedDictionary<int, List<AssetFlow>>> data;

		public void SetUp()
		{
			data = new SortedDictionary<int, SortedDictionary<int, List<AssetFlow>>>();
			var flowsData = new List<AssetFlow>()
			{
				new AssetFlow()
				{
					IsCash = false,
					IsInFlow = true,
					Ticker = "ticker1"
				},
				new AssetFlow()
				{
					IsCash = false,
					IsInFlow = true,
					Ticker = "ticker2"
				},
				new AssetFlow()
				{
					IsCash = true,
					IsInFlow = false,
					Ticker = "ticker3"
				},
			};
		}

		[TestMethod]
		public void OriginalMethod_Test()
		{
			var flow = new AssetFlowsByYearMonth();
			
		}
	}
}
