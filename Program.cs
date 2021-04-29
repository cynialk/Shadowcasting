using System;
using Raylib_cs;
using System.Timers;
using System.Numerics;


namespace Shadowcasting
{
    public struct Tile
    {
        public int row;
        public int col;
        public bool Wall;
        public bool Revealed;
    }
    class Program
    {
        public static Tile[,] TileMap;
        public static Vector2 playerPos;
        public static int FPS;
        static int width = 600;
        static int height = 600;
        static int FramesSinceStart;
        static int SecondsSinceChange;
        static int average;
        static int totalframes;
        static Timer timer;

        static void Main(string[] args)
        {
            bool KeepMap = false;
            bool drawWall = false;
            timer = new Timer(1000);
            timer.Elapsed += CalculateFPS;
            FramesSinceStart = 0;
            SecondsSinceChange = 0;
            FOVRecurse fov = new FOVRecurse();
            SymmetricShadowcasting ssc = new SymmetricShadowcasting();
            TileMap = GenerateRandomTiles(-1, 50, 50, true);
            playerPos = new Vector2();
            playerPos.X = TileMap.GetLength(0) / 2;
            playerPos.Y = TileMap.GetLength(1) / 2;
            Raylib.InitWindow(width, height, "Shadowcasting");
            int currentAlgorithm = 0;
            int algorithmCount = 2;
            timer.Start();
            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    TileMap = GenerateRandomTiles( -1, 50, 50, true);
                    playerPos.X = TileMap.GetLength(0) / 2;
                    playerPos.Y = TileMap.GetLength(1) / 2;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    KeepMap = !KeepMap;
                }
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

                //Movement
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_W))
                {
                    playerPos.Y--;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
                {
                    playerPos.Y++;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_D))
                {
                    playerPos.X++;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_A))
                {
                    playerPos.X--;
                }


                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Vector2 mouse = Raylib.GetMousePosition();
                    int tilewidth = width / TileMap.GetLength(0);
                    int tileheight = height / TileMap.GetLength(1);
                    int selectedX = (int)Math.Floor(mouse.X / tilewidth);
                    int selectedY = (int)Math.Floor(mouse.Y / tileheight);
                    drawWall = !TileMap[selectedX, selectedY].Wall;
                }
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Vector2 mouse = Raylib.GetMousePosition();
                    int tilewidth = width / TileMap.GetLength(0);
                    int tileheight = height / TileMap.GetLength(1);
                    int selectedX = (int)Math.Floor(mouse.X / tilewidth);
                    int selectedY = (int)Math.Floor(mouse.Y / tileheight);
                    TileMap[selectedX, selectedY].Wall = drawWall;
                }

                if (!KeepMap)
                {
                    TileMap = GenerateRandomTiles(1, 50, 50, true);
                    
                }
                else
                {
                    for (int x = 0; x < TileMap.GetLength(0); x++)
                    {
                        for (int y = 0; y < TileMap.GetLength(1); y++)
                        {
                            TileMap[x, y].Revealed = false;
                        }
                    }
                }
                

                switch (currentAlgorithm)
                {
                    case 0:
                        fov.ConvertToMap(TileMap);
                        fov.ConvertToTiles();
                        break;
                    case 1:
                        
                        ssc.compute_fov((int)playerPos.X, (int)playerPos.Y);
                        break;
                }
                FramesSinceStart++;
                Vector2 origin = new Vector2();
                origin.Y = 0;
                origin.X = 0;
                Renderer.RenderTiles(origin, 600, 600, TileMap, currentAlgorithm,FPS, average, Raylib.GetMousePosition(),playerPos);
            }
        }
        public static void CalculateFPS(Object source, ElapsedEventArgs e)
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
            }

            
            timer.Start();

        }
        public static Tile[,] GenerateRandomTiles(int pWalls, int width, int height)
        {
            return GenerateRandomTiles(pWalls, width, height, false);
        }
        public static Tile[,] GenerateRandomTiles(int pWalls, int width, int height, bool walls)
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
            
            if (walls)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x, 0].Wall = true;
                    tiles[x, height - 1].Wall = true;
                }
                for (int y = 0; y < height; y++)
                {
                    tiles[0, y].Wall = true;
                    tiles[width - 1, y].Wall = true;
                }
            }
            return tiles;
        }
    }
}
