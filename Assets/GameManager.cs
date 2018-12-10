using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // size of a square tile in pixels
    int tileSize = 8;
    int numCols;
    int numRows;
    int numTiles;
    int widthPixels;
    int heightPixels;

    int pad = 0;

    // ascii map
    char[] currentTiles;

    List<List<Point>> paths;

    HashSet<int> edges = new HashSet<int>();
    // a map of wall tiles that already belong to a built path
    HashSet<int> visited = new HashSet<int>();

    class Point
    {
        public int x;
        public int y;
        public int? cx;
        public int? cy;
    }

    // Use this for initialization
    void Start()
    {
        string map1 = "____________________________________________________________________________________|||||||||||||||||||||||||||||..........................||.|||||||.||||||||.|||||||.||o|||||||.|______|.|||||||o||......||.||||||||.||......||||.||.||..........||.||.|||__|.||.||.||||||||.||.||.|__|||.||.||.||||||||.||.||.||||...||.......||.......||...||.||||.||||| || |||||.||||.||.||||.||||| || |||||.||||.||......              ......|||||.|||| |||--||| ||||.||||||||.|||| |______| ||||.||||    .     |______|     .    ||||.|||| |______| ||||.||||||||.|||| |||||||| ||||.|||||......||          ||......||.||||.|| |||||||| ||.||||.||.||||.|| |||||||| ||.||||.||...||.......||.......||...||||.||.|||||.||.|||||.||.|||__|.||.|||||.||.|||||.||.|____|....||....  ....||....|____|.||.||.||.||.||.||.||.|__|||.||.||.||.||.||.||.||.||||...||....||.||.||....||...||.||||.|||||.||.|||||.||||.||o||||.|||||.||.|||||.||||o||............||............|||||||||||||||||||||||||||||________________________________________________________";
        Map(28, 36, map1.ToCharArray());
        draw();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // direction enums (in clockwise order)
    enum DIR
    {
        DIR_UP,
        DIR_RIGHT,
        DIR_DOWN,
        DIR_LEFT
    }

    // get direction enum from a direction vector
    DIR getEnumFromDir(Vector2 dir)
    {
        if (dir.x == -1) return DIR.DIR_LEFT;
        if (dir.x == 1) return DIR.DIR_RIGHT;
        if (dir.y == -1) return DIR.DIR_UP;
        return DIR.DIR_DOWN;
    }

    // set direction vector from a direction enum
    Vector2 setDirFromEnum(DIR dirEnum)
    {
        Vector2 dir = Vector2.zero;
        if (dirEnum == DIR.DIR_UP) { dir.x = 0; dir.y = -1; }
        else if (dirEnum == DIR.DIR_RIGHT) { dir.x = 1; dir.y = 0; }
        else if (dirEnum == DIR.DIR_DOWN) { dir.x = 0; dir.y = 1; }
        else if (dirEnum == DIR.DIR_LEFT) { dir.x = -1; dir.y = 0; }
        return dir;
    }

    // constructor
    void Map(int numCols, int numRows, char[] tiles)
    {
        // sizes
        this.numCols = numCols;
        this.numRows = numRows;
        this.numTiles = numCols * numRows;
        this.widthPixels = numCols * tileSize;
        this.heightPixels = numRows * tileSize;

        // ascii map
        this.currentTiles = tiles;

        this.parseWalls();
    }

    // we extend the x range to suggest the continuation of the tunnels
    int toIndex(int x, int y)
    {
        if (x >= -2 && x < numCols + 2 && y >= 0 && y < numRows)
            return (x + 2) + y * (numCols + 4);
        throw new System.Exception("Error");
    }

    // This is a procedural way to generate original-looking maps from a simple ascii tile
    // map without a spritesheet.
    void parseWalls()
    {
        // creates a list of drawable canvas paths to render the map walls
        this.paths = new List<List<Point>>();

        // a map of which wall tiles that are not completely surrounded by other wall tiles
        int i = 0, x = 0, y = 0;
        for (y = 0; y < this.numRows; y++)
        {
            for (x = -2; x < this.numCols + 2; x++, i++)
            {
                if (this.getTile(x, y) != char.MinValue && this.getTile(x, y) == '|' &&
                    (this.getTile(x - 1, y) != '|' ||
                    this.getTile(x + 1, y) != '|' ||
                    this.getTile(x, y - 1) != '|' ||
                    this.getTile(x, y + 1) != '|' ||
                    this.getTile(x - 1, y - 1) != '|' ||
                    this.getTile(x - 1, y + 1) != '|' ||
                    this.getTile(x + 1, y - 1) != '|' ||
                    this.getTile(x + 1, y + 1) != '|'))
                {
                    edges.Add(i);
                }
            }
        }
        // iterate through all edges, making a new path after hitting an unvisited wall edge
        i = 0;
        for (y = 0; y < this.numRows; y++)
            for (x = -2; x < this.numCols + 2; x++, i++)
                if (edges.Contains(i) && !(visited.Contains(i)))
                {
                    visited.Add(i);
                    makePath(x, y);
                }
    }

    /*
     We employ the 'right-hand rule' by keeping our right hand in contact
     with the wall to outline an individual wall piece.

     Since we parse the tiles in row major order, we will always start
     walking along the wall at the leftmost tile of its topmost row.  We
     then proceed walking to the right.  

     When facing the direction of the walk at each tile, the outline will
     hug the left side of the tile unless there is a walkable tile to the
     left.  In that case, there will be a padding distance applied.
    */
    Point getStartPoint(int tx, int ty, DIR dirEnum)
    {
        Vector2 dir = setDirFromEnum(dirEnum);
        if (!edges.Contains(toIndex(tx + (int)dir.y, ty - (int)dir.x)))
            pad = isFloorTile(tx + (int)dir.y, ty - (int)dir.x) ? 5 : 0;
        var px = -tileSize / 2 + pad;
        var py = tileSize / 2;
        var a = (int)dirEnum * Mathf.PI / 2;
        var c = Mathf.Cos(a);
        var s = Mathf.Sin(a);
        return new Point
        {
            // the first expression is the rotated point centered at origin
            // the second expression is to translate it to the tile
            x = (int)((px * c - py * s) + (tx + 0.5) * tileSize),
            y = (int)((px * s + py * c) + (ty + 0.5) * tileSize),
        };
    }

    // walks along edge wall tiles starting at the given index to build a canvas path
    void makePath(int tx, int ty)
    {
        //Debug.Log(tx + " " + ty);
        // get initial direction
        DIR dirEnum;
        if (edges.Contains(toIndex(tx + 1, ty)))
            dirEnum = DIR.DIR_RIGHT;
        else if (edges.Contains(toIndex(tx, ty + 1)))
            dirEnum = DIR.DIR_DOWN;
        else
            throw new System.Exception("tile shouldn't be 1x1 at " + tx + "," + ty);
        Vector2 dir = setDirFromEnum(dirEnum);

        // increment to next tile
        tx += (int)dir.x;
        ty += (int)dir.y;

        // backup initial location and direction
        var init_tx = tx;
        var init_ty = ty;
        var init_dirEnum = dirEnum;

        List<Point> path = new List<Point>();
        pad = 0; // (persists for each call to getStartPoint)
        Point point;
        Point lastPoint;

        bool turn = false, turnAround = false;

        while (true)
        {
            visited.Add(toIndex(tx, ty));

            // determine start point
            point = getStartPoint(tx, ty, dirEnum);

            if (turn)
            {
                // if we're turning into this tile, create a control point for the curve
                //
                // >---+  <- control point
                //     |
                //     V
                lastPoint = path[path.Count - 1];
                if (dir.x == 0)
                {
                    point.cx = point.x;
                    point.cy = lastPoint.y;
                }
                else
                {
                    point.cx = lastPoint.x;
                    point.cy = point.y;
                }
            }

            // update direction
            turn = false;
            turnAround = false;
            if (edges.Contains(toIndex(tx + (int)dir.y, ty - (int)dir.x)))
            { // turn left
                dirEnum = (DIR)(((int)dirEnum + 3) % 4);
                turn = true;
            }
            else if (edges.Contains(toIndex(tx + (int)dir.x, ty + (int)dir.y)))
            { // continue straight
            }
            else if (edges.Contains(toIndex(tx - (int)dir.y, ty + (int)dir.x)))
            { // turn right
                dirEnum = (DIR)(((int)dirEnum + 1) % 4);
                turn = true;
            }
            else
            { // turn around
                dirEnum = (DIR)(((int)dirEnum + 2) % 4);
                turnAround = true;
            }
            dir = setDirFromEnum(dirEnum);

            // commit path point
            path.Add(point);

            // special case for turning around (have to connect more dots manually)
            if (turnAround)
            {
                path.Add(getStartPoint(tx - (int)dir.x, ty - (int)dir.y, (DIR)(((int)dirEnum + 2) % 4)));
                path.Add(getStartPoint(tx, ty, dirEnum));
            }

            // advance to the next wall
            tx += (int)dir.x;
            ty += (int)dir.y;

            // exit at full cycle
            if (tx == init_tx && ty == init_ty && dirEnum == init_dirEnum)
            {
                paths.Add(path);
                break;
            }
        }
    }


    int posToIndex(int x, int y)
    {
        if (x >= 0 && x < this.numCols && y >= 0 && y < this.numRows)
            return x + y * this.numCols;
        throw new System.Exception("Error");
    }

    // retrieves tile character at given coordinate
    // extended to include offscreen tunnel space
    char getTile(int x, int y)
    {
        if (x >= 0 && x < this.numCols && y >= 0 && y < this.numRows)
            return this.currentTiles[this.posToIndex(x, y)];

        // extend walls and paths outward for entrances and exits
        if ((x == -1 && this.getTile(x + 1, y) == '|' && (this.isFloorTile(x + 1, y + 1) || this.isFloorTile(x + 1, y - 1))) ||
            (x == this.numCols && this.getTile(x - 1, y) == '|' && (this.isFloorTile(x - 1, y + 1) || this.isFloorTile(x - 1, y - 1))))
            return '|';
        if ((x == -1 && this.isFloorTile(x + 1, y)) ||
            (x == this.numCols && this.isFloorTile(x - 1, y)))
            return ' ';

        return char.MinValue;
    }

    // determines if the given character is a walkable floor tile
    bool isFloorTileChar(char tile)
    {
        return tile == ' ' || tile == '.' || tile == 'o';
    }

    // determines if the given tile coordinate has a walkable floor tile
    bool isFloorTile(int x, int y)
    {
        return isFloorTileChar(getTile(x, y));
    }

    void draw()
    {
        for (int i = 0; i < this.paths.Count; i++)
        {
            List<Vector3> positions = new List<Vector3>();
            var lineRenderer = new GameObject("lineRenderer" + i).AddComponent<LineRenderer>();
            var path = this.paths[i];

            positions.Add(new Vector3(path[0].x, path[0].y, 0));
            int j;
            for (j = 1; j < path.Count; j++)
            {
                if (path[j].cx != null)
                {
                    //ctx.quadraticCurveTo(path[j].cx, path[j].cy, path[j].x, path[j].y);
                }
                else
                {
                    positions.Add(new Vector3(path[j].x, path[j].y, 0));
                }
            }
            //ctx.quadraticCurveTo(path[j-1].x, path[0].y, path[0].x, path[0].y);
            //ctx.fill();
            //ctx.stroke();
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.loop = true;
        }
    }
}
