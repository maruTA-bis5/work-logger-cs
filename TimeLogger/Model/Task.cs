using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WorkLog.TimeLogger.Model {
    public class Task {
        public long? Id { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime EnableFrom { get; set; }
        public DateTime EnableTo { get; set; }
		[DefaultValue(true)]
		public bool IsEnabled { get; set; }

        public override string ToString() {
            return TaskCode + ":" + TaskName;
        }
    }
}
