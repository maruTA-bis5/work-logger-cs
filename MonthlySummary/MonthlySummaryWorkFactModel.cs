using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkLog.TimeLogger.Model;

namespace WorkLog.MonthlySummary {
	public class MonthlySummaryWorkFactModel {
		public static MonthlySummaryWorkFactModel of(Dictionary<string, TaskWorkFact> data) {
			return new MonthlySummaryWorkFactModel(data);
		}
		public Dictionary<string, TaskWorkFact> Data { get; private set; }
		private MonthlySummaryWorkFactModel(Dictionary<string, TaskWorkFact> data) {
			Data = data;
		}
	}
}
