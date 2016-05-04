using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLog.MonthlySummary {
	static class Extensions {
		public static V Get<K, V>(this IDictionary<K, V> dictionary, K key) {
			if (dictionary.ContainsKey(key)) {
				return dictionary[key];
			} else {
				return default(V);
			}
		}
	}
}
