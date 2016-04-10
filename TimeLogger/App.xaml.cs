using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using WorkLog.TimeLogger.Migrations;
using WorkLog.TimeLogger.Model;
namespace TimeLogger {
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application {

		public App() {
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<TimeLoggerContext, Configuration>());
		}
	}
}
