using FluentMigrator;

namespace Migrator.Migrations
{
    [Migration(202202202241)]
    public class CreateCitiesTableMigration_202202202241 : Migration
    {
        public override void Up()
        {
            Create.Table("cities")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString()
                .WithColumn("url_name").AsString()
                .WithColumn("address").AsString()
                .WithColumn("country_code").AsString()
                .WithColumn("latitude").AsString()
                .WithColumn("longitude").AsString();
        }

        public override void Down()
        {
            Delete.Table("cities");
        }
    }
}