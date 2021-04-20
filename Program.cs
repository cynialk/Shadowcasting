using System;
using Raylib_cs;
using System.Timers;


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
        public static int FPS;
        static int FramesSinceStart;
        static int SecondsSinceChange;
        static int average;
        static int totalframes;
        static Timer timer;

        static void Main(string[] args)
        {
            timer = new Timer(1000);
            timer.Elapsed += CalculateFPS;
            FramesSinceStart = 0;
            SecondsSinceChange = 0;

            FOVRecurse fov = new FOVRecurse();
            
            Raylib.InitWindow(600, 600, "Shadowcasting");
            int currentAlgorithm = 0;
            int algorithmCount = 2;
            timer.Start();
            while (!Raylib.WindowShouldClose())
            {
                
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
                {
                    SecondsSinceChange = 0;
                    totalframes = 0;
                    currentAlgorithm = (currentAlgorithm + 1) % algorithmCount;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
                {
                    SecondsSinceChange = 0;
                    totalframes = 0;
                    currentAlgorithm = (currentAlgorithm - 1);
                    if (currentAlgorithm < 0) currentAlgorithm = algorithmCount - 1;
                }
                switch (currentAlgorithm)
                {
                    case 0:
                        TileMap = GenerateRandomTiles(5, 600, 600);
                        PosX = TileMap.GetLength(0) / 2;
                        PosY = TileMap.GetLength(1) / 2;
                        fov.ConvertToMap(TileMap);
                        fov.ConvertToTiles();
                        break;
                }
                FramesSinceStart++;
                Renderer.RenderTiles(0, 0, 600, 600, TileMap, currentAlgorithm,FPS, average);
            }
        }
        public static void CalculateFPS(Object source, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            FPS = FramesSinceStart;
            FramesSinceStart = 0;
            totalframes += FPS;
            SecondsSinceChange++;
            if (SecondsSinceChange == 1)
            {
                average = FPS;
            }
            else
            {
                average = totalframes  / SecondsSinceChange;
                Console.WriteLine(average);
            }

            Console.WriteLine(SecondsSinceChange);
            
            timer.Start();

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
                    tiles[x, y].Wall = (random.Next(0, 1000) <= pWalls);
                    tiles[x, y].Revealed = false;
                }
            }
            return tiles;
        }
    }
}
