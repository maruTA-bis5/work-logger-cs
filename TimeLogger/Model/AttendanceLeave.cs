using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkLog.TimeLogger.Model {
	public class AttendanceLeave {
		public long? Id { get; set; }
		public DateTime TargetDate { get; set; }
		public DateTime Attendance { get; set; }
		public DateTime Leave { get; set; }
	}
}
