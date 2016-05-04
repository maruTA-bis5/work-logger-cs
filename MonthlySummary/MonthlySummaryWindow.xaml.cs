using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorkLog.TimeLogger.Model;

namespace WorkLog.MonthlySummary {
	/// <summary>
	/// MonthlySummaryWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MonthlySummaryWindow : Window {
		public MonthlySummaryWindow() {
			InitializeComponent();
		}

		private async void onLoadClick(Object sender, RoutedEventArgs e) {
			DateTime? target = targetYearMonth.SelectedDate;
			if (target.HasValue) {
				await loadMonthlyData(target.Value);
			}
		}

		private async System.Threading.Tasks.Task loadMonthlyData(DateTime targetYearMonth) {
			status.Content = "集計中...";
			await System.Threading.Tasks.Task.Run(() => {
				DateTime monthFrom = targetYearMonth.AddDays(-(targetYearMonth.Day - 1));
				DateTime monthTo = monthFrom.AddMonths(1).AddMilliseconds(-1);

				Dictionary<DateTime, AttendanceLeave> attendanceLeaves;
				List<TaskWorkFact> taskWorkFacts;
				using (var db = new TimeLoggerContext()) {
					attendanceLeaves = db.AttendanceLeaves.Where(a => a.TargetDate >= monthFrom && a.TargetDate <= monthTo).ToDictionary(a => a.TargetDate.Date);
					// ↓DBで集計させるべき
					taskWorkFacts = db.TaskWorkFacts.Where(f => f.StartAt >= monthFrom && f.StartAt <= monthTo).ToList();
				}
				var workFactByDate = new Dictionary<DateTime, List<TaskWorkFact>>();
				foreach (var workfact in taskWorkFacts) {
					var date = workfact.StartAt.Date;
					var workFacts = workFactByDate.Get(date);
					if (workFacts == null) {
						workFacts = new List<TaskWorkFact>();
						workFactByDate[date] = workFacts;
					}
					workFacts.Add(workfact);
				}
				var workFactSummary = new Dictionary<DateTime, Dictionary</*task id*/long, TaskWorkFact>>();
				var taskIds = new HashSet<long>();
				foreach (var workfacts in workFactByDate) {
					var summaries = new Dictionary<long, TaskWorkFact>();
					foreach (var workfact in workfacts.Value) {
						var summary = summaries.Get(workfact.TaskId);
						if (summary == null) {
							summary = new TaskWorkFact();
							summary.TaskId = workfact.TaskId;
							summaries[workfact.TaskId] = summary;
						}
						summary.Manhour += workfact.Manhour;
						if (summary.Manhour > 0) {
							taskIds.Add(summary.TaskId);
						}
					}
					workFactSummary[workfacts.Key] = summaries;
				}

				Dictionary<long,Task> tasks;
				using (var db = new TimeLoggerContext()) {
					tasks = db.Tasks.Where(t => taskIds.Contains(t.Id.Value)).OrderBy(t => t.TaskCode).ToDictionary(t => t.Id.Value);
				}
				Dispatcher.Invoke(() => {
					workFactGrid.Columns.Clear();
					foreach (var task in tasks) {
						var column = new DataGridTextColumn();
						column.Header = task.Value.ToString();
						column.Binding = new Binding("Data[t" + task.Value.Id+"].Manhour");
						workFactGrid.Columns.Add(column);
					}
				});

				var attendanceGridSource = new List<AttendanceLeave>();
				var workFactGridSource = new List<MonthlySummaryWorkFactModel>();
				for (var processDate = monthFrom; processDate <= monthTo; processDate = processDate.AddDays(1)) {
					var attendanceLeave = attendanceLeaves.Get(processDate);
					if (attendanceLeave == null) {
						attendanceLeave = new AttendanceLeave();
						attendanceLeave.TargetDate = processDate;
					}
					attendanceGridSource.Add(attendanceLeave);

					var workFacts = new Dictionary<String, TaskWorkFact>();
					var summaries = workFactSummary.Get(processDate);
					if (summaries != null) {
						foreach (var summary in summaries.Values) {
							workFacts["t" + summary.TaskId] = summary;
						}
					}
					workFactGridSource.Add( MonthlySummaryWorkFactModel.of(workFacts));
				}
				Dispatcher.Invoke(() => {
					attendanceGrid.ItemsSource = attendanceGridSource;
					workFactGrid.ItemsSource = workFactGridSource;
				});


			});
			status.Content = "";
		}
		
	}
}
