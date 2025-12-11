using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalogo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PopulandoBancoComDadosIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemByte",
                table: "Produtos");

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativa", "Descricao" },
                values: new object[,]
                {
                    { 1, true, "Lanche" },
                    { 2, true, "Acompanhamento" },
                    { 3, true, "Bebida" },
                    { 4, true, "Sobremesa" }
                });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "CategoriaId", "DataAtualizacao", "DataCriacao", "Descricao", "Nome", "Preco", "Status" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8698), "adicional de bacon", "X-Bacon", 31.99m, true },
                    { 2, 3, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8707), "Zero açucar", "Coca-Cola", 7m, true },
                    { 3, 2, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8709), "300g", "Batata frita", 15m, true },
                    { 4, 4, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8710), "Morango", "Sorvete", 9m, true },
                    { 5, 1, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8711), "saladinha da boa", "X-Salada", 24.99m, true },
                    { 6, 3, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8712), "concorrente", "Pepsi", 7m, true },
                    { 7, 2, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8713), "300g", "Onion rings", 20m, true },
                    { 8, 4, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8714), "Chocolate com morango", "Bolo de pote", 14m, true },
                    { 9, 1, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8716), "tudo do bom e do melhor", "X-Tudo", 40m, true },
                    { 10, 3, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8717), "suquinho", "Suco de maracuja", 10m, true },
                    { 11, 2, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8718), "400g", "Batata + Onion rings P", 27.5m, true },
                    { 12, 4, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8719), "Melhor de todos", "Pudim", 99m, true },
                    { 13, 1, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8720), "fitness", "X-Frango", 22.99m, true },
                    { 14, 1, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8721), "pouca gordura graças a Deus", "X-Calabresa", 26.99m, true },
                    { 15, 1, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8723), "suculência ao máximo", "X-Picanha", 36.99m, true },
                    { 16, 3, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8724), "suquinho 2", "Suco de limão", 7m, true },
                    { 17, 3, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8725), "água de torneira", "H2O", 5m, true },
                    { 18, 2, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8726), "700g", "Batata + Onion rings M", 33m, true },
                    { 19, 2, null, new DateTime(2025, 12, 11, 20, 10, 17, 956, DateTimeKind.Local).AddTicks(8727), "1Kg", "Batata + Onion rings G", 41m, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImagemByte",
                table: "Produtos",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
