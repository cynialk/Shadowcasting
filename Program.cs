using System;
using Raylib_cs;
using System.Threading;


namespace Shadowcasting
{
    public struct Tile
    {
        public bool Wall;
        public bool Revealed;
    }
    class Program
    {
        public static int PosX;
        public static int PosY;
        public static Tile[,] TileMap;
        static void Main(string[] args)
        {
            FOVRecurse fov = new FOVRecurse();
            
            Raylib.InitWindow(600, 600, "Shadowcasting");
            int currentAlgorithm = 0;
            int algorithmCount = 2;
            while(!Raylib.WindowShouldClose())
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
                {
                    currentAlgorithm = (currentAlgorithm + 1) % algorithmCount;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
                {
                    currentAlgorithm = (currentAlgorithm - 1);
                    if (currentAlgorithm < 0) currentAlgorithm = algorithmCount - 1;
                }
                switch (currentAlgorithm)
                {
                    case 0:
                        TileMap = GenerateRandomTiles(5, 50, 50);
                        PosX = TileMap.GetLength(0) / 2;
                        PosY = TileMap.GetLength(1) / 2;
                        fov.ConvertToMap(TileMap);
                        fov.ConvertToTiles();
                        break;
                }
                
                Renderer.RenderTiles(0, 0, 600, 600, TileMap, currentAlgorithm);
            }
        }
        public static Tile[,] GenerateRandomTiles(int pWalls, int width, int height)
        {
            Tile[,] tiles = new Tile[width, height];
            Random random = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tiles[x, y] = new Tile();
                    tiles[x, y].Wall = (random.Next(0, 100) <= pWalls);
                    tiles[x, y].Revealed = false;
                }
            }
            return tiles;
        }
    }
}
