Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);
Console.ReadLine();

void f2(IEnumerable<string> lines)
{
    var cubes = lines
        .Select(o => o.Split(',').Select(int.Parse).GetEnumerator())
        .Select(o => new Point(Next(o), Next(o), Next(o)))
        .ToHashSet();

    Bound(cubes, out var min, out var max);

    var bfsMin = new Point(min.X - 1, min.Y - 1, min.Z - 1);
    var bfsMax = new Point(max.X + 1, max.Y + 1, max.Z + 1);

    var water = new HashSet<Point>();
    var visited = new HashSet<Point>();
    
    var q = new Queue<Point>();
    q.Enqueue(bfsMin);

    while (q.Count > 0)
    {
        var p = q.Dequeue();
        water.Add(p);

        foreach (var n in Near(p))
        {
            if (n.X >= bfsMin.X && n.X <= bfsMax.X
                && n.Y >= bfsMin.Y && n.Y <= bfsMax.Y
                && n.Z >= bfsMin.Z && n.Z <= bfsMax.Z)
            {
                if (water.Contains(n))
                    continue;

                if (cubes.Contains(n))
                    continue;

                if (visited.Contains(n))
                    continue;

                q.Enqueue(n);
                visited.Add(n);
            }
        }
    }

    for (var x = min.X; x <= max.X; x++)
    {
        for (var y = min.Y; y <= max.Y; y++)
        {
            for (var z = min.Z; z <= max.Z; z++)
            {
                var testPoint = new Point(x, y, z);
                if (water.Contains(testPoint))
                    continue;
                cubes.Add(testPoint);
            }
        }
    }

    var s = 0;
    foreach (var c in cubes)
    {
        foreach (var n in Near(c))
        {
            if (!cubes.Contains(n))
                s += 1;
        }
    }

    Console.WriteLine(s);
}

void Bound(HashSet<Point> cubes, out Point min, out Point max)
{
    min = new Point(cubes.Min(o => o.X),
        cubes.Min(o => o.Y),
        cubes.Min(o => o.Z));    

    max = new Point(cubes.Max(o => o.X),
        cubes.Max(o => o.Y),
        cubes.Max(o => o.Z));
}

void f1(IEnumerable<string> lines)
{
    var cubes = lines
        .Select(o => o.Split(',').Select(int.Parse).GetEnumerator())
        .Select(o => new Point(Next(o), Next(o), Next(o)))
        .ToHashSet();

    var s = 0;
    foreach (var c in cubes)
    {
        foreach (var n in Near(c))
        {
            if (!cubes.Contains(n))
                s += 1;
        }
    }

    Console.WriteLine(s);
}

T Next<T>(IEnumerator<T> iterator)
{
    iterator.MoveNext();
    return iterator.Current;
}

IEnumerable<Point> Near(Point point)
{
    var (x, y, z) = point;
    yield return new Point(x - 1, y, z);
    yield return new Point(x + 1, y, z);
    yield return new Point(x, y - 1, z);
    yield return new Point(x, y + 1, z);
    yield return new Point(x, y, z - 1);
    yield return new Point(x, y, z + 1);
}

record struct Point(int X, int Y, int Z);