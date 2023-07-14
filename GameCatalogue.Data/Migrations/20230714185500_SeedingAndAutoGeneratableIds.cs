using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCatalogue.Data.Migrations
{
    public partial class SeedingAndAutoGeneratableIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Shooter" },
                    { 2, "Roguelike" },
                    { 3, "Platformer" },
                    { 4, "Battle royale" },
                    { 5, "Puzzle" },
                    { 6, "Adventure" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "DeveloperId", "GenreId", "ImageURL", "Name" },
                values: new object[] { 1, "Neon Blast: Enter a cybernetic world of neon-lit chaos. Unleash your skills, wield futuristic weapons, and fight against a corrupt regime. Fast-paced action, stunning visuals, and intense multiplayer battles await!", new Guid("9c9e3599-5d8c-4e84-9fe9-97528e3b3025"), 1, "https://media.istockphoto.com/id/993696960/photo/emoticon-smile-led.jpg?s=2048x2048&w=is&k=20&c=WzuM-npePurbItkdrOsVxy6PlWUaUu37MdGJDzUkVxQ=", "Neon blast" });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "DeveloperId", "GenreId", "ImageURL", "Name" },
                values: new object[] { 2, "\"Dungeoneer\": Deadly dungeons, treasures await. Procedural, perilous. Battle, survive. Unravel secrets, embrace challenge. Permadeath, endless exploration. Conquer or fall. Good luck!", new Guid("9c9e3599-5d8c-4e84-9fe9-97528e3b3025"), 2, "https://media.istockphoto.com/id/1386931686/vector/dungeon-long-medieval-castle-corridor-with-torches-interior-of-ancient-palace-with-stone.jpg?s=2048x2048&w=is&k=20&c=kJEOjgDceG-1HjRWvFTEy7BGYoN0OX8sdygWppl_PYU=", "Dungeoneer" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
