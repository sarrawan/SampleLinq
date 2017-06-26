using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;

namespace SampleLINQ
{
	public class AssetFlowsByYearMonth : SortedDictionary<int, SortedDictionary<int, List<AssetFlow>>>
	{
		
		// need to surround all the methods with the method logger in order to be able to check the performance
		// need to create a test that calls each of the methods and make sure that the code you wrote
		// is actually doing what it is supposed to do
		// need to make sure that the logger is actually working

		public AssetFlowsByYearMonth(SortedDictionary<int, SortedDictionary<int, List<AssetFlow>>> data)
		{
			foreach (var keyValuePair in data)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

		public bool HaveSecurityAssetFlowsInNonInitialMonth(int yearMonth, out bool hasPseudo)
		{
			bool haveSecurityAssetFlows = false;
			using(new MethodLog(Logger, "HaveSecurityAssetFlowsInNonInitialMonth"))
			{
				hasPseudo = false;
				SortedDictionary<int, List<AssetFlow>> purchaseYearMonthAssetFlowSets = null;
				Logger.Debug("this is a debug message");
				Logger.Error("this is an error message");

				if (TryGetValue(yearMonth, out purchaseYearMonthAssetFlowSets))
				{
					foreach (var cpymaf in purchaseYearMonthAssetFlowSets)
					{
						foreach (var caaf in cpymaf.Value.Where(af => af.IsInFlow))
						{
							hasPseudo = IsPseudoTicker(caaf.Ticker);

							if (!caaf.IsCash)
							{
								haveSecurityAssetFlows = true;
								break;
							}
						}
						if (haveSecurityAssetFlows)
						{
							break;
						}
					}
				}
			}

			return haveSecurityAssetFlows;
		}

		public bool HaveSecurityLinqAny(int yearMonth, out bool hasPseudo)
		{
			using (new MethodLog(Logger, "HaveSecurityLinqAny"))
			{
				var purchaseYearMonth = this.Where(x => x.Key == yearMonth);

				var keyValuePairs = purchaseYearMonth as IList<KeyValuePair<int, SortedDictionary<int, List<AssetFlow>>>> ??
				                    purchaseYearMonth.ToList();
				var haveSecurityAssetFlows = keyValuePairs.Any(x => x.Value.Any(a => a.Value.Any(f => f.IsInFlow && !f.IsCash)));
				hasPseudo = keyValuePairs.Any(
					x => x.Value.Any(a => a.Value.Any(f => f.IsInFlow && !f.IsCash && IsPseudoTicker(f.Ticker))));

				return haveSecurityAssetFlows;
			}
		}

		public bool HaveSecurityLinqSelectMany(int yearMonth, out bool hasPseudo)
		{
			using (new MethodLog(Logger, "HaveSecurityLinqSelectMany"))
			{
				var purchaseYearMonth = this.Where(x => x.Key == yearMonth);

				var keyValuePairs = purchaseYearMonth as IList<KeyValuePair<int, SortedDictionary<int, List<AssetFlow>>>> ??
				                    purchaseYearMonth.ToList();
				var haveSecurityAssetFlowsResult = keyValuePairs
					.SelectMany(x => x.Value.SelectMany(a => a.Value.Where(f => f.IsInFlow && !f.IsCash))).ToList();

				hasPseudo = haveSecurityAssetFlowsResult.Any(a => IsPseudoTicker(a.Ticker));

				return haveSecurityAssetFlowsResult.Any();
			}
		}

		private bool IsPseudoTicker(string caafTicker)
		{
			return caafTicker == "pseudo";
		}
	}
}