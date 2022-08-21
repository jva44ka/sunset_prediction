using FluentMigrator;

namespace Migrator.Migrations
{
    [Migration(202208131950)]
    public class InitMigration202208131950 : Migration
    {
        public override void Up()
        {
            Create.Table("cities")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().Unique()
                .WithColumn("name_for_url").AsString()
                .WithColumn("address").AsString()
                .WithColumn("country_code").AsString()
                .WithColumn("latitude").AsDouble().Nullable()
                .WithColumn("longitude").AsDouble().Nullable();

            Create.Table("updates")
                .WithColumn("update_id").AsInt32().PrimaryKey()
                .WithColumn("handle_date").AsDateTime();

            Create.Table("users")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("external_id").AsInt64().Unique()
                .WithColumn("first_name").AsString().Nullable()
                .WithColumn("last_name").AsString().Nullable()
                .WithColumn("user_name").AsString().NotNullable()
                .WithColumn("previous_dialog_state").AsByte().Nullable()
                .WithColumn("current_dialog_state").AsByte()
                .WithColumn("city_id").AsInt32().Nullable().ForeignKey("fk_users_cities", "cities", "id")
                .WithColumn("state_change_date").AsDateTime();
        }

        public override void Down()
        {
            Delete.Table("cities");
            Delete.Table("updates");
            Delete.Table("users");
        }
    }
}