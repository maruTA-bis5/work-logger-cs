namespace WorkLog.TimeLogger.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WorkLog.TimeLogger.Model.TimeLoggerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WorkLog.TimeLogger.Model.TimeLoggerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

			// bug: 全てのtask.isEnabled == falseの場合、次回マイグレーション時にすべてtrueになる
			bool isMigrate = context.Tasks.Where(t => t.IsEnabled == true).Count() == 0;
			if (isMigrate) {
				context.Tasks.ToList().ForEach(t => t.IsEnabled = true);
				context.SaveChanges();
			}
        }
    }
}
