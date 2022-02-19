using FluentMigrator;

namespace Migrator.Migrations
{
    [Migration(202202192346)]
    public class CreateDialogStateTableMigration_202202192346 : Migration
    {
        public override void Up()
        {
            Create.Table("dialog_states")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt32().Indexed()
                .WithColumn("previous_state").AsByte().Nullable()
                .WithColumn("state").AsByte()
                .WithColumn("proposed_city_id").AsInt32().Nullable()
                .WithColumn("state_change_date").AsDateTime2();
        }

        public override void Down()
        {
            Delete.Table("dialog_state");
        }
    }
}