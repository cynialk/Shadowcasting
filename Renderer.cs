using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using Raylib_cs;

namespace Shadowcasting
{
    class Renderer
    {
        public static void RenderTiles(Vector2 origin, int width, int height, Tile[,] tiles, int currentAlgorithm, int fps, int averagefps, Vector2 mouse, Vector2 playerpos)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            int tilewidth = width / tiles.GetLength(0);
            int tileheight = height / tiles.GetLength(1);
            //Tiles
            
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for(int y = 0; y < tiles.GetLength(1); y++)
                {
                    Tile tile = tiles[x, y];
                    //Raylib.DrawRectangle((int)origin.X + (width / tiles.GetLength(0)) * x, (int)origin.Y + (height / tiles.GetLength(1)) * y, width / tiles.GetLength(0), height / tiles.GetLength(1), Color.BLACK);
                    if (!tile.Revealed)
                    {
                        Raylib.DrawRectangle((int)origin.X + tilewidth * x, (int)origin.Y + tileheight * y, tilewidth, tileheight, Color.DARKGRAY);
                    }
                    if (tile.Wall)
                    {
                        Raylib.DrawRectangleLines((int)origin.X + tilewidth * x, (int)origin.Y + tileheight * y, tilewidth, tileheight, Color.BLACK);
                    }
                }
            }

            //Mouse/hover
            int selectedX = (int)Math.Floor(mouse.X / tilewidth);
            int selectedY = (int)Math.Floor(mouse.Y / tileheight);
            Raylib.DrawRectangleLines(selectedX*tilewidth,selectedY*tileheight,tilewidth,tileheight,Color.RED);

            switch(currentAlgorithm)
            {
                case 0:
                    Raylib.DrawText("Current Algorithm: Recursive", 10, 10, 20, Color.BLACK);
                    break;
                case 1:
                    Raylib.DrawText("Current Algorithm: Symmetric", 10, 10, 20, Color.BLACK);
                    break;
            }
            Raylib.DrawText($"Current FPS: " + fps, 10, 30, 20, Color.BLACK);
            Raylib.DrawText($"Average FPS: " + averagefps, 10, 50, 20, Color.BLACK);
            Raylib.EndDrawing();
        }
    }
}
