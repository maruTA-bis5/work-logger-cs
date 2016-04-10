using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkLog.TimeLogger.Model {
	public class TaskWorkFact {
		public Nullable<long> Id { get; set; }
		public long TaskId { get; set; }
		[ForeignKey("TaskId")]
		public virtual Task refTask { get; set; }
		public DateTime StartAt { get; set; }
		public DateTime EndAt { get; set; }
		public decimal Manhour { get; set; }

		public void updateManhour() {
			TimeSpan span = EndAt.Subtract(StartAt);
			decimal hour = new decimal(span.Hours);
			decimal min = new decimal(span.Minutes);
			min = decimal.Divide(min, 60);
			min = decimal.Round(min, 2);

			Manhour = hour + min;
		}
	}
}
