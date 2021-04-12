﻿using System;
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
            Raylib.ClearBackground(Color.WHITE);
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for(int y = 0; y < tiles.GetLength(1); y++)
                {
                    Tile tile = tiles[x, y];
                    //Raylib.DrawRectangle(xorig + (width / tiles.GetLength(0)) * x, yorig + (height / tiles.GetLength(1)) * y, width / tiles.GetLength(0), height / tiles.GetLength(1), Color.BLACK);
                    if (!tile.Revealed)
                    {
                        Raylib.DrawRectangle(xorig + (width / tiles.GetLength(0)) * x, yorig + (height / tiles.GetLength(1)) * y, width / tiles.GetLength(0), height / tiles.GetLength(1), Color.GRAY);
                    }
                    if (tile.Wall)
                    {
                        Raylib.DrawRectangleLines(xorig + (width / tiles.GetLength(0)) * x, yorig + (height / tiles.GetLength(1)) * y, width / tiles.GetLength(0), height / tiles.GetLength(1), Color.BLACK);
                    }
                }
            }
            Raylib.EndDrawing();
        }
    }
}
