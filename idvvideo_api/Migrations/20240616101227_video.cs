using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace idvvideo_api.Migrations
{
    /// <inheritdoc />
    public partial class video : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_video",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Video_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Video_msg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Video_url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_video", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_video");
        }
    }
}
