using System;
using Raylib_cs;


namespace Shadowcasting
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Raylib.InitWindow(800, 800, "Shadowcasting comparison");
            Tile[,] tiles = new Tile[10, 10];
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    Tile tile = new Tile();
                    tile.Wall = (random.Next(0, 100) > 50);
                    tile.Revealed = (random.Next(0, 100) > 50);
                    tiles[x, y] = tile;
                }
            }
            while (!Raylib.WindowShouldClose())
            {
                Raylib.ClearBackground(Color.WHITE);
                Renderer.RenderTiles(100, 100, 200, 200, tiles);
            }
            Raylib.CloseWindow();
        }
    }
}
