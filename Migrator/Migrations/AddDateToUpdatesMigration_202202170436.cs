using FluentMigrator;

namespace Migrator.Migrations
{
    [Migration(202202170436)]
    public class AddDateToUpdatesMigration_202202170436 : Migration
    {
        public override void Up()
        {
            Alter.Table("updates")
                .AddColumn("handle_date").AsDateTime2();
        }

        public override void Down()
        {
            Delete.Column("handle_date").FromTable("updates");
        }
    }
}