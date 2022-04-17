using FluentMigrator;

namespace Migrator.Migrations
{
    [Migration(202204172127)]
    public class AddMoreDataToUsersTable_202204172127 : Migration
    {
        public override void Up()
        {
            Alter.Table("users")
                  .AddColumn("first_name").AsString().Nullable()
                  .AddColumn("last_name").AsString().Nullable()
                  .AddColumn("user_name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("first_name").FromTable("users");
            Delete.Column("last_name").FromTable("users");
            Delete.Column("user_name").FromTable("users");
        }
    }
}