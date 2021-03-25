using System;
using Raylib_cs;

namespace Shadowcasting
{
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(200, 800, "cool");
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.WHITE);
                Raylib.DrawText("Yo what's up homie", 2, 5, 10, Color.BLACK);
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}
