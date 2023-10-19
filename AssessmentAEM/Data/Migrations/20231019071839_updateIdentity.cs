using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssessmentAEM.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT [Platforms] OFF");
            migrationBuilder.Sql("SET IDENTITY_INSERT [Wells] OFF");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
