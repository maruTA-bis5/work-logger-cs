namespace WorkLog.TimeLogger.Model {
	using System;
	using System.Data.Entity;
	using System.Linq;

	public class TimeLoggerContext : DbContext {
		// コンテキストは、アプリケーションの構成ファイル (App.config または Web.config) から 'TimeLoggerModel' 
		// 接続文字列を使用するように構成されています。既定では、この接続文字列は LocalDb インスタンス上
		// の 'WorkLog.TimeLogger.Model.TimeLoggerModel' データベースを対象としています。 
		// 
		// 別のデータベースとデータベース プロバイダーまたはそのいずれかを対象とする場合は、
		// アプリケーション構成ファイルで 'TimeLoggerModel' 接続文字列を変更してください。
		public TimeLoggerContext()
			: base("name=TimeLoggerModel") {
		}

		// モデルに含めるエンティティ型ごとに DbSet を追加します。Code First モデルの構成および使用の
		// 詳細については、http://go.microsoft.com/fwlink/?LinkId=390109 を参照してください。

		public virtual DbSet<Task> Tasks { get; set; }
		public virtual DbSet<TaskWorkFact> TaskWorkFacts { get; set; }
		public virtual DbSet<AttendanceLeave> AttendanceLeaves { get; set; }
	}

	//public class MyEntity
	//{
	//    public int Id { get; set; }
	//    public string Name { get; set; }
	//}
}