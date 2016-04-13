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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkLog.TimeLogger.Model;

namespace WorkLog.TimeLogger.Widget {
	/// <summary>
	/// TaskRow.xaml の相互作用ロジック
	/// </summary>
	public partial class TaskRow : UserControl {
		public Task Task { get; private set; }
		public TaskRow(Task task) {
			InitializeComponent();
			Task = task;
            TaskLabel.Content = Task.ToString();
            TaskLabel.ToolTip = string.IsNullOrEmpty(Task.Description) ? Task.ToString() : Task.Description;
		}

		public event TaskStartEventHandler OnTaskStart;
		public event TaskStopEventHandler OnTaskStop;


		private void onTaskStart(Object sender, RoutedEventArgs e) {
			OnTaskStart(Task);
		}

		private void onTaskStop(Object sender, RoutedEventArgs e) {
			OnTaskStop(Task);
		}

	}

	public delegate void TaskStartEventHandler(Task task);
	public delegate void TaskStopEventHandler(Task task);
}
