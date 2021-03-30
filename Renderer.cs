using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;

namespace Shadowcasting
{
    static class Renderer
    {
        public static void RenderTiles(int xorig, int yorig, int width, int height, Tile[,] tiles)
        {
            Raylib.BeginDrawing();
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for(int y = 0; y < tiles.GetLength(1); y++)
                {
                    Tile tile = tiles[x, y];
                    //Raylib.DrawRectangle(xorig + (width / tiles.GetLength(0)) * x, yorig + (height / tiles.GetLength(1)) * y, width / tiles.GetLength(0), height / tiles.GetLength(1), Color.BLACK);
                    if (tile.Revealed)
                    {
                        if (tile.Wall)
                        {
                            Console.WriteLine("revealed wall");
                            Raylib.DrawRectangleLines(xorig + (width / tiles.GetLength(0)) * x, yorig + (height / tiles.GetLength(1)) * y, width / tiles.GetLength(0), height / tiles.GetLength(1), Color.BLACK);
                        }
                        else
                        {
                            Console.WriteLine("revealed space");
                        }
                    }
                    else
                    {
                        Raylib.DrawRectangle(xorig + (width / tiles.GetLength(0)) * x, yorig + (height / tiles.GetLength(1)) * y, width / tiles.GetLength(0), height / tiles.GetLength(1), Color.GRAY);
                    }
                }
            }
            Raylib.EndDrawing();
        }
    }
}
