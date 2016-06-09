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

namespace WorkLog.TimeLogger {

    class TaskModel {
        public Task task { get; set; }
        public override string ToString() {
            return task.ToString();
        }
        public static TaskModel of(Task task) {
            TaskModel model = new TaskModel();
            model.task = task;
            return model;
        }
    }
    class EmptyTaskModel : TaskModel {
        public override string ToString() {
            return "未割当";
        }
    }
    /// <summary>
    /// AttendanceWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AttendanceWindow : Window {
        public AttendanceWindow() {
            InitializeComponent();
            using (var db = new TimeLoggerContext()) {
                var tasks = db.Tasks.Where(t => t.IsEnabled).OrderBy(t => t.TaskCode).ToList().ConvertAll(t => TaskModel.of(t));
                tasks.Insert(0, new EmptyTaskModel());
                TaskSelector.ItemsSource = tasks;
            }
        }

        public Task doAttendance() {
            ShowDialog();
            return startupTask;
        }

        private Task startupTask { get; set; }

		private void onAttendanceButtonClick(object sender, RoutedEventArgs e) {
			var taskModel = (TaskModel)TaskSelector.SelectedItem;
			startupTask = taskModel != null ? taskModel.task : null;
			this.Close();
		}
    }
}
