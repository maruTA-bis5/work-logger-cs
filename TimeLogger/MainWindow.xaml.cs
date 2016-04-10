﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

using WorkLog.TimeLogger.Model;
using WorkLog.TimeLogger.Widget;
using WorkLog.TimeLogger;
namespace TimeLogger {
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			OnReloadButtonClick(null, null);
		}
		private Task runningTask { get; set; }
		private TaskWorkFact runningFact { get; set; }
		private void onTaskStart(Task task) {
			if (runningTask != null) {
				onTaskStop(runningTask);
			}
			runningTask = task;
			runningFact = new TaskWorkFact();
			runningFact.TaskId = task.Id.Value;
			runningFact.StartAt = DateTime.Now;
		}
		private void OnTaskStop(object sender, RoutedEventArgs e) {
			onTaskStop(runningTask);
		}
		private void onTaskStop(Task task) {
			if (runningTask == null) {
				return;
			}
			runningFact.EndAt = DateTime.Now;
			runningFact.updateManhour();
			using (var db = new TimeLoggerContext()) {
				db.TaskWorkFacts.Add(runningFact);
				db.SaveChanges();
			}
			runningFact = null;
			runningTask = null;
		}

		private void OnReloadButtonClick(object sender, RoutedEventArgs e) {
			var latestTaskRows = new List<TaskRow>();
			using (var db = new TimeLoggerContext()) {
				foreach (var task in db.Tasks.OrderBy(t => t.TaskCode)) {
					var row = new TaskRow(task);
					row.OnTaskStart += onTaskStart;
					row.OnTaskStop += onTaskStop;
					latestTaskRows.Add(row);
				}
			}
			TaskList.Children.Clear();
			latestTaskRows.ForEach(r => TaskList.Children.Add(r));
		}

		private void OnManageButtonClick(object sender, RoutedEventArgs e) {
			new TaskManageWindow().ShowDialog();
			OnReloadButtonClick(sender, e);
		}

		private List<TaskWorkFact> loadDailyWorkFacts(DateTime targetDate) {
			List<TaskWorkFact> dailyWorkFact;
			using (var db = new TimeLoggerContext()) {
				var from = targetDate.Date;
				var to = from.AddDays(1).AddMilliseconds(-1);
				dailyWorkFact = db.TaskWorkFacts.Where(f => from <= f.StartAt && f.EndAt <= to) //
					.OrderBy(f => f.TaskId).ToList();
			}
			return dailyWorkFact;
		}

		private void OnReportButtonClick(object sender, RoutedEventArgs e) {
			var todayWorkFact = loadDailyWorkFacts(DateTime.Today);

			if (todayWorkFact.Count == 0) {
				MessageBox.Show("実績データがありません");
				return;
			}

			var factByTask = todayWorkFact.GroupBy(f => f.TaskId, (t, fs) => new KeyValuePair<long, decimal>(t, fs.Sum(f => f.Manhour)));
			Dictionary<long?, Task> tasks;
			using (var db = new TimeLoggerContext()) {
				tasks = db.Tasks.ToDictionary(t => t.Id);
			}

			StringBuilder builder = new StringBuilder();
			foreach (var fact in factByTask) {
				var task = tasks[fact.Key];
				builder.AppendLine(String.Format("{0}:{1} \t{2}h", task.TaskCode, task.TaskName, fact.Value));
			}

			MessageBox.Show(builder.ToString());
		}

		private void onLeaveClick(object sender, RoutedEventArgs e) {
			onTaskStop(runningTask);
			using (var db = new TimeLoggerContext()) {
				var todayData = db.AttendanceLeaves.Where(a => a.TargetDate == DateTime.Today).First();
				todayData.Leave = DateTime.Now;
				db.SaveChanges();
			}
			Close();
		}
	}
}
