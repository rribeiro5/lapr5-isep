using Microsoft.EntityFrameworkCore.Migrations;



namespace DDDNetCore.Migrations

{

    public partial class AddAvatar : Migration

    {

        protected override void Up(MigrationBuilder migrationBuilder)

        {

            migrationBuilder.AddColumn<string>(

                name: "_Avatar__avatarUrl",

                table: "Users",

                type: "nvarchar(max)",

                nullable: true);

        }



        protected override void Down(MigrationBuilder migrationBuilder)

        {

            migrationBuilder.DropColumn(

                name: "_Avatar__avatarUrl",

                table: "Users");

        }

    }

}