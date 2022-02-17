using FluentMigrator;

namespace Migrator.Migrations
{
    [Migration(202202170403)]
    public class CreateUpdatesTableMigration_202202170403 : Migration
    {
        public override void Up()
        {
            Create.Table("updates")
                .WithColumn("update_id").AsInt32().PrimaryKey();
        }

        public override void Down()
        {
            Delete.Table("updates");
        }
    }
}