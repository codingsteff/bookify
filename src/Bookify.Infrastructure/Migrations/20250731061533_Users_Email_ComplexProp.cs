using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Users_Email_ComplexProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // see UserConfiguration.cs for the complex property configuration
            // Index are currently not supported for complex properties
            // migrationBuilder.DropIndex(
            //     name: "ix_users_email",
            //     table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateIndex(
            //     name: "ix_users_email",
            //     table: "users",
            //     column: "email",
            //     unique: true);
        }
    }
}
