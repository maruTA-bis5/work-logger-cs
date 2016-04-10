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
using  WorkLog.TimeLogger.Model;
namespace WorkLog.TimeLogger {
	/// <summary>
	/// TaskManageWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class TaskManageWindow : Window {
		public TaskManageWindow() {
			InitializeComponent();

			using(var db = new TimeLoggerContext()) {
				taskTable.ItemsSource = db.Tasks.OrderBy(t => t.Id).ToList();
			}

		}

		private void OnOkButtonClick(object sender, RoutedEventArgs e) {
			using (var db = new TimeLoggerContext()) {
				var tasks = taskTable.ItemsSource.OfType<Task>().ToList();
				foreach (var task in tasks) {
					if (task.Id == null) {
						db.Tasks.Add(task);
					} else {
						db.Tasks.Attach(task);
						// FIXME すべてのレコードが必ず更新される
						db.Entry(task).State = System.Data.Entity.EntityState.Modified;
					}
				}
				db.SaveChanges();
			}
			Close();
		}

		private void OnCancelButtonClick(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
