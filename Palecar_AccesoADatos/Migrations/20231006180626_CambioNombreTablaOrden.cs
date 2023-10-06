using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Palecar_AccesoADatos.Migrations
{
    /// <inheritdoc />
    public partial class CambioNombreTablaOrden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalles_Ordenes_OrdenId",
                table: "OrdenDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalles_Productos_ProductoId",
                table: "OrdenDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_Ordenes_AspNetUsers_UsuarioAplicacionId",
                table: "Ordenes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ordenes",
                table: "Ordenes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdenDetalles",
                table: "OrdenDetalles");

            migrationBuilder.RenameTable(
                name: "Ordenes",
                newName: "Orden");

            migrationBuilder.RenameTable(
                name: "OrdenDetalles",
                newName: "OrdenDetalle");

            migrationBuilder.RenameIndex(
                name: "IX_Ordenes_UsuarioAplicacionId",
                table: "Orden",
                newName: "IX_Orden_UsuarioAplicacionId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenDetalles_ProductoId",
                table: "OrdenDetalle",
                newName: "IX_OrdenDetalle_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenDetalles_OrdenId",
                table: "OrdenDetalle",
                newName: "IX_OrdenDetalle_OrdenId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orden",
                table: "Orden",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdenDetalle",
                table: "OrdenDetalle",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orden_AspNetUsers_UsuarioAplicacionId",
                table: "Orden",
                column: "UsuarioAplicacionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalle_Orden_OrdenId",
                table: "OrdenDetalle",
                column: "OrdenId",
                principalTable: "Orden",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalle_Productos_ProductoId",
                table: "OrdenDetalle",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orden_AspNetUsers_UsuarioAplicacionId",
                table: "Orden");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalle_Orden_OrdenId",
                table: "OrdenDetalle");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdenDetalle_Productos_ProductoId",
                table: "OrdenDetalle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdenDetalle",
                table: "OrdenDetalle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orden",
                table: "Orden");

            migrationBuilder.RenameTable(
                name: "OrdenDetalle",
                newName: "OrdenDetalles");

            migrationBuilder.RenameTable(
                name: "Orden",
                newName: "Ordenes");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenDetalle_ProductoId",
                table: "OrdenDetalles",
                newName: "IX_OrdenDetalles_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdenDetalle_OrdenId",
                table: "OrdenDetalles",
                newName: "IX_OrdenDetalles_OrdenId");

            migrationBuilder.RenameIndex(
                name: "IX_Orden_UsuarioAplicacionId",
                table: "Ordenes",
                newName: "IX_Ordenes_UsuarioAplicacionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdenDetalles",
                table: "OrdenDetalles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ordenes",
                table: "Ordenes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalles_Ordenes_OrdenId",
                table: "OrdenDetalles",
                column: "OrdenId",
                principalTable: "Ordenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdenDetalles_Productos_ProductoId",
                table: "OrdenDetalles",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ordenes_AspNetUsers_UsuarioAplicacionId",
                table: "Ordenes",
                column: "UsuarioAplicacionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
