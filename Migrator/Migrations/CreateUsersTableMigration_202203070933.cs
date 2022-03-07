using FluentMigrator;

namespace Migrator.Migrations
{
    [Migration(202203070933)]
    public class CreateUsersTableMigration_202203070933 : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("previous_dialog_state").AsByte().Nullable()
                .WithColumn("dialog_state").AsByte()
                .WithColumn("city_id").AsInt32().Nullable().ForeignKey("fk_users_cities", "cities", "id")
                .WithColumn("state_change_date").AsDateTime2();
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}