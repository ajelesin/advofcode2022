
Console.Write("release? (r): ");
var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);
Console.ReadLine();

static void f2(IEnumerable<string> lines)
{
    var field = new Dictionary<(int, int), char>();
    var knots = Enumerable.Range(0, 10).Select(o => (0, 0)).ToArray();

    SetPoint(field, knots[9].Item1, knots[9].Item2, '#');

    var totalsteps = 0;
    foreach (var line in lines)
    {
        var tokens = line.Split(' ').ToArray();
        var steps = int.Parse(tokens[1]);
        var directions = tokens[0];

        for (var i = 1; i <= steps; i++)
        {
            totalsteps++;

            MoveHead(knots[0].Item1, knots[0].Item2, directions,
                out var newx, out var newy);

            int oldx = knots[0].Item1, oldy = knots[0].Item2;

            knots[0].Item1 = newx;
            knots[0].Item2 = newy;

            for (var k = 1; k < knots.Length; k++)
            {
                MoveTail(oldx, oldy, newx, newy,
                    knots[k].Item1, knots[k].Item2,
                    out var newtx, out var newty);

                newx = newtx;
                newy = newty;

                oldx = knots[k].Item1;
                oldy = knots[k].Item2;

                knots[k].Item1 = newx;
                knots[k].Item2 = newy;
            }

            SetPoint(field, knots[9].Item1, knots[9].Item2, '#');
        }
    }

    Print(field);
    var count = field.Values.Where(o => o == '#').Count();
    Console.WriteLine(count);

}

static void f1(IEnumerable<string> lines)
{
    var field = new Dictionary<(int, int), char>();
    int hx = 0, hy = 0;
    int tx = 0, ty = 0;

    SetPoint(field, tx, ty, '#');

    foreach (var line in lines)
    {
        var tokens = line.Split(' ').ToArray();
        var steps = int.Parse(tokens[1]);
        var directions = tokens[0];

        for (var i = 1; i <= steps; i++)
        {
            MoveHead(hx, hy, directions, out var newhx, out var newhy);
            MoveTail(hx, hy, newhx, newhy, tx, ty, out var newtx, out var newty);

            hx = newhx;
            hy = newhy;
            tx = newtx;
            ty = newty;

            SetPoint(field, tx, ty, '#');
        }
    }

    Print(field);
    var count = field.Values.Where(o => o == '#').Count();
    Console.WriteLine(count);
}

static void MoveTail(int oldhx, int oldhy, int newhx, int newhy,
    int tx, int ty, out int newtx, out int newty)
{
    if (newhx == tx && newhy == ty)
    {
        newtx = tx;
        newty = ty;
    }
    else if (Math.Abs(newhx - tx) <= 1 && Math.Abs(newhy - ty) <= 1)
    {
        newtx = tx;
        newty = ty;
    }
    else if (newhy == ty)
    {
        newty = ty;
        newtx = newhx > tx ? newhx - 1 : newhx + 1;
    }
    else if (newhx == tx)
    {
        newtx = tx;
        newty = newhy > ty ? newhy - 1 : newhy + 1;
    }
    else if (Math.Abs(newhx - tx) == 1)
    {
        newtx = newhx;
        newty = newhy > ty ? newhy - 1 : newhy + 1;
    }
    else if (Math.Abs(newhy - ty) == 1)
    {
        newty = newhy;
        newtx = newhx < tx ? newhx + 1 : newhx - 1;
    }
    else
    {
        newtx = newhx > tx ? newhx - 1 : newhx + 1;
        newty = newhy > ty ? newhy - 1 : newhy + 1;
    }
}

static void MoveHead(int hx, int hy, string directions, out int newhx, out int newhy)
{
    var steps = 1;

    if (directions == "R")
    {
        newhx = hx + steps;
        newhy = hy;
    }
    else if (directions == "L")
    {
        newhx = hx - steps;
        newhy = hy;
    }
    else if (directions == "U")
    {
        newhx = hx;
        newhy = hy - steps;
    }
    else if (directions == "D")
    {
        newhx = hx;
        newhy = hy + steps;
    }
    else
    {
        throw new Exception("whf?");
    }
}

static void Print(Dictionary<(int, int), char> field)
{
    var minX = field.Keys.Min(o => o.Item1);
    var maxX = field.Keys.Max(o => o.Item1);
    var minY = field.Keys.Min(o => o.Item2);
    var maxY = field.Keys.Max(o => o.Item2);

    for (var i = minY; i <= maxY; i++)
    {
        for (var j = minX; j <= maxX; j++)
        {
            if (field.ContainsKey((j, i)))
                Console.Write(field[(j, i)]);
            else
                Console.Write('.');
        }
        Console.WriteLine();
    }
}

static void SetPoint(Dictionary<(int, int), char> field, int x, int y, char c)
{
    field[(x, y)] = c;
}