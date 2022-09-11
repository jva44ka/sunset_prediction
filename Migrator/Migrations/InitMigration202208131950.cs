using FluentMigrator;

namespace Migrator.Migrations;

[Migration(202209041900)]
public class InitMigration202209041900 : Migration
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

        Execute.Sql("CREATE INDEX IX_cities_name_pattern ON cities (name text_pattern_ops)");

        Create.Table("updates")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("external_id").AsInt64().Unique()
            .WithColumn("handled_at").AsDateTime2().Indexed();

        Create.Table("chats")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("external_id").AsInt64().Unique()
            .WithColumn("previous_state").AsByte().Nullable()
            .WithColumn("current_state").AsByte()
            .WithColumn("state_changed_at").AsDateTime2();

        Create.Table("users")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("external_id").AsInt64().Unique()
            .WithColumn("first_name").AsString().Nullable()
            .WithColumn("last_name").AsString().Nullable()
            .WithColumn("user_name").AsString().NotNullable()
            .WithColumn("subscribe_type").AsByte().Nullable()
            .WithColumn("city_id").AsInt32().Nullable().ForeignKey("fk_users_cities", "cities", "id")
            .WithColumn("chat_id").AsInt32().NotNullable().ForeignKey("fk_users_chats", "chats", "id");
    }

    public override void Down()
    {
        Delete.Table("cities");
        Delete.Table("updates");
        Delete.Table("users");
        Delete.Table("chats");
    }
}