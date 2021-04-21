using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;

namespace Shadowcasting
{
    class SymmetricShadowcasting
    {
        bool is_blocking(int x, int y)
        {
            return Program.TileMap[x, y].Wall;

        }
        void mark_visible(int x, int y)
        {
            Program.TileMap[x, y].Revealed = true;
        }
        public void compute_fov(int ox, int oy)
        {
            
            for (int i = 0; i < 4; i++)
            {

                Quadrant quadrant = new Quadrant(i, ox, oy);
                void reveal(Tile tile)
                {
                    int[] xy = quadrant.transform(tile);
                    mark_visible(xy[0], xy[1]);
                }

                bool is_wall(Tile tile)
                {
                    if (tile.Equals(default(Tile))) return false;
                    int[] xy = quadrant.transform(tile);
                    return is_blocking(xy[0], xy[1]);
                }

                bool is_floor(Tile tile)
                {
                    if (tile.Equals(default(Tile))) return false;
                    int[] xy = quadrant.transform(tile);
                    return !is_blocking(xy[0], xy[1]);
                }

                void scan(Row row)
                {
                    Tile prev_tile = new Tile();
                    foreach (Tile tile in row.tiles())
                    {
                        if (is_wall(tile) || Row.is_symmetric(row, tile))
                        {
                            reveal(tile);
                        }

                        if (is_wall(prev_tile) && is_floor(tile))
                        {
                            row.start_slope = row.slope(tile);
                        }

                        if (is_floor(prev_tile) && is_wall(tile))
                        {
                            Row next_row = row.next();
                            next_row.end_slope = row.slope(tile);
                            scan(next_row);
                        }
                        prev_tile = tile;
                    }
                    if (is_floor(prev_tile))
                    {
                        scan(row.next());

                    }
                    return;
                    
                }
                Row first_row = new Row(1, -1, 1);
                scan(first_row);
            }
        }
    }
    class Quadrant
    {
        int cardinal;
        int oy;
        int ox;
        public Quadrant(int cardinal, int ox, int oy)
        {
            this.cardinal = cardinal;
            this.ox = ox;
            this.oy = oy;
        }

        public int[] transform(Tile tile)
        {
            switch (cardinal)
            {
                case 0: return new int[] { ox + tile.col, oy - tile.row };
                case 2: return new int[] { ox + tile.col, oy + tile.row };
                case 1: return new int[] { ox + tile.row, oy + tile.col };
                case 3: return new int[] { ox - tile.row, oy + tile.col };
                default: return new int[] { 0, 0 };
            }
        }
    }

    class Row
    {
        public int depth { get; set; }
        public float end_slope { get; set; }
        public float start_slope { get; set; }

        public Row(int depth, float start_slope, float end_slope)
        {
            this.depth = depth;
            this.end_slope = end_slope;
            this.start_slope = start_slope;

        }

        public List<Tile> tiles()
        {
            float min_col = round_ties_up(depth * start_slope);
            float max_col = round_ties_down(depth * end_slope);
            List<Tile> tiles = new List<Tile>();
            for (int i = (int)min_col; i < max_col + 1; i++)
            {
                Tile tile = new Tile();
                tile.row = depth;
                tile.col = i;
                tiles.Add(tile);
            }
            return tiles;
        }
        float round_ties_up(float n)
        {
            return (float)Math.Floor(n + 0.5);
        }
        float round_ties_down(float n)
        {
            return (float)Math.Ceiling(n - 0.5);
        }
        public Row next()
        {
            return new Row(depth + 1, start_slope, end_slope);
        }
        public float slope(Tile tile)
        {

            return (2f * tile.col - 1) / (2f * tile.row);

        }
        public static bool is_symmetric(Row row, Tile tile)
        {
            return tile.col >= (row.depth * row.start_slope) && tile.col <= (row.depth * row.end_slope);
        }
    }
}

