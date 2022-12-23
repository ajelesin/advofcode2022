using System.Collections.Generic;

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f1(lines);
Console.ReadLine();

void f1(IEnumerable<string> lines)
{
    var movements = lines.First();
    var movementPointer = 0;

    var gens = new[] { CreateLine, CreatePlus, CreateCorner, CreateVLine, CreateQ };
    var genPointer = 0;

    int minY = 0;
    var fgs = new HashSet<Point>();
    var fggs = new LinkedList<Fg>();
    var rocks = 0L;

    while (true)
    {
        var fg = gens[genPointer](minY + 3);
        //PrintField(CalcField(fgs.Concat(new[] { fg })));

        var tempMinY = 0;

        while (true)

        {
            if (movements[movementPointer] == '<')
                fg.Left(fgs, 0);
            else
                fg.Right(fgs, 6);
            movementPointer = (movementPointer + 1);
            if (movementPointer == movements.Length)
                movementPointer = 0;

            //PrintField(CalcField(fgs.Concat(new[] { fg })));

            fg.Down(fgs, 0);

            if (tempMinY == fg.MinY)
                break;

            //PrintField(CalcField(fgs.Concat(new[] { fg })));
            tempMinY = fg.MinY;
        }

        genPointer = (genPointer + 1);
        if (genPointer == gens.Length)
            genPointer = 0;

        foreach (var p in fg.Points)
            fgs.Add(p);
        fggs.AddLast(fg);

        if (fg.MaxY >= minY)
            minY = fg.MaxY + 1;

        //PrintField(CalcField(fgs));

        rocks++;
        if (rocks > 17_406_975)
            break;
        if (rocks % 100_000 == 0)
            Console.WriteLine(rocks);
    }

    Console.WriteLine(minY - 2);
}

void PrintField(HashSet<Point> field)
{
    Bound(field, out var min, out var max);
    for (var y = max.Y; y >= min.Y; y--)
    {
        for (var x = 0; x <= 6; x++)
        {
            if (field.Contains(new Point(x, y)))
                Console.Write('#');
            else
                Console.Write('.');
        }
        Console.WriteLine();
    }
    Console.WriteLine();
    Console.ReadLine();
}

HashSet<Point> CalcField(IEnumerable<Fg> fgs) => fgs.SelectMany(o => o.Points).ToHashSet();

void Bound1(IEnumerable<Fg> fgs, out Point min, out Point max)
{
    var minX = fgs.SelectMany(o => o.Points).Min(o => o.X);
    var minY = fgs.SelectMany(o => o.Points).Min(o => o.Y);
    var maxX = fgs.SelectMany(o => o.Points).Max(o => o.X);
    var maxY = fgs.SelectMany(o => o.Points).Max(o => o.Y);

    min = new Point(minX, minY);
    max = new Point(maxX, maxY);
}

void Bound(HashSet<Point> field, out Point min, out Point max)
{
    var minX = field.Min(o => o.X);
    var minY = field.Min(o => o.Y);
    var maxX = field.Max(o => o.X);
    var maxY = field.Max(o => o.Y);

    min = new Point(minX, minY);
    max = new Point(maxX, maxY);
}

Fg CreateLine(int y) => new()
{
    Points = new Point[]
    {
        new Point(2, y),
        new Point(3, y),
        new Point(4, y),
        new Point(5, y),
    },
    MinX = 2,
    MinY = y,
    MaxX = 5,
    MaxY = y,
};

Fg CreatePlus(int y) => new()
{
    Points = new Point[]
    {
        new Point(3, y + 2),
        new Point(2, y + 1),
        new Point(3, y + 1),
        new Point(4, y + 1),
        new Point(3, y),
    },
    MinX = 2,
    MinY = y,
    MaxX = 4,
    MaxY = y + 2,
};

Fg CreateCorner(int y) => new()
{
    Points = new Point[]
    {
        new Point(2, y),
        new Point(3, y),
        new Point(4, y),
        new Point(4, y + 1),
        new Point(4, y + 2),
    },
    MinX = 2,
    MinY = y,
    MaxX = 4,
    MaxY = y + 2,
};

Fg CreateVLine(int y) => new()
{
    Points = new Point[]
    {
        new Point(2, y),
        new Point(2, y + 1),
        new Point(2, y + 2),
        new Point(2, y + 3),
    },
    MinX = 2,
    MinY = y,
    MaxX = 2,
    MaxY = y + 3,
};

Fg CreateQ(int y) => new()
{
    Points = new Point[]
    {
        new Point(2, y),
        new Point(3, y),
        new Point(2, y + 1),
        new Point(3, y + 1),
    },
    MinX = 2,
    MinY = y,
    MaxX = 3,
    MaxY = y + 1,
};

class Fg
{
    public IEnumerable<Point> Points { get; set; }

    public int MaxY { get; set; }

    public int MinY { get; set; }

    public int MaxX { get; set; }

    public int MinX { get; set; }

    public void Down(HashSet<Point> fgs, int minY)
    {
        if (MinY - 1 < minY)
            return;

        var newArray = Points.Select(o => o with { Y = o.Y - 1 });

        if (newArray.Any(o => fgs.Contains(o)))
            return;

        MinY--;
        MaxY--;
        Points = newArray;
    }

    public void Left(HashSet<Point> fgs, int minX)
    {
        if (MinX - 1 < minX)
            return;

        var newArray = Points.Select(o => o with { X = o.X - 1 });

        if (newArray.Any(o => fgs.Contains(o)))
            return;

        MinX -= 1;
        MaxX -= 1;
        Points = newArray;
    }

    public void Right(HashSet<Point> fgs, int maxX)
    {
        if (MaxX + 1 > maxX)
            return;

        var newArray = Points.Select(o => o with { X = o.X + 1 });

        if (newArray.Any(o => fgs.Contains(o)))
            return;

        MaxX += 1;
        MinX += 1;
        Points = newArray;
    }
}

record struct Point(int X, int Y);