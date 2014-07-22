using FluentMigrator;

namespace KitchIn.Tools.Migrator.Migrations
{
    [Migration(6)]
    public class M0006_CreateFavoritesRecipesTable : Migration
    {
        public override void Up()
        {
            this.Create.Table("FavoritesRecipes")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("Pk_FavRecipeTab_ID").Identity()
                .WithColumn("Name").AsString(255).Nullable()
                .WithColumn("RecipeBigOven").AsString(255).Nullable();

            this.Create.Table("FavoritesRecipes_Users")
                .WithColumn("FavoriteRecipeId").AsInt64().Nullable()
                .WithColumn("UserId").AsInt64().Nullable();


            this.Create.ForeignKey("fk_FavRec_UsTab_UserId_UsersTab_Id")
           .FromTable("FavoritesRecipes_Users").ForeignColumn("UserId")
           .ToTable("Users").PrimaryColumn("Id");

            this.Create.ForeignKey("fk_FavRec_UsTab_FavRecId_FavRecTab_Id")
                .FromTable("FavoritesRecipes_Users").ForeignColumn("FavoriteRecipeId")
                .ToTable("FavoritesRecipes").PrimaryColumn("Id");
        }

        public override void Down()
        {
            this.Delete.Table("FavoritesRecipes_Users");
            this.Delete.Table("FavoritesRecipes");
        }
    }
}