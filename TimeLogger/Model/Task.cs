using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLog.TimeLogger.Model {
	public class Task {
		public long? Id { get; set; }
		public string TaskCode { get; set; }
		public string TaskName { get; set; }
		public string Description { get; set; }
	}
}
