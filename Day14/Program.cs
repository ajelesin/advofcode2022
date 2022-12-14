
Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);
Console.ReadLine();

void f2(IEnumerable<string> lines)
{
    var space = ReadWalls(lines);
    space.Add(new Point(500, 0), '+');
    var maxY = space.Max(o => o.Key.Y) + 2;

    var abyss = false;
    while (!abyss)
    {
        var currPosition = new Point(500, 0);

        while (!abyss)
        {
            var next = Step(currPosition, space);
            if (next == currPosition)
            {
                if (!space.ContainsKey(next))
                    space.Add(next, 'o');
                else
                    abyss = true;
                break;
            }

            if (IsAbyssY(next, space, maxY))
            {
                if (space.ContainsKey(next))
                    abyss = true;
                else
                    space.Add(next, '#');
                break;
            }

            currPosition = next;
        }
    }

    CalcBound(space, out var boundMin, out var boundMax);
    PrintSpace(space, boundMin, boundMax);

    var amount = space.Where(o => o.Value == 'o').Count() + 1;
    Console.WriteLine(amount);
}

void f1(IEnumerable<string> lines)
{
    var space = ReadWalls(lines);
    space.Add(new Point(500, 0), '+');
    CalcBound(space, out var boundMin, out var boundMax);

    var abyss = false;
    while (!abyss)
    {
        var currPosition = new Point(500, 0);

        while (!abyss)
        {
            var next = Step(currPosition, space);
            if (next == currPosition)
            {
                space.Add(next, 'o');
                break;
            }

            if (IsAbyss(next, space, boundMin, boundMax))
            {
                abyss = true;
                break;
            }

            currPosition = next;
        }
    }

    PrintSpace(space, boundMin, boundMax);
    var amount = space.Where(o => o.Value == 'o').Count();
    Console.WriteLine(amount);
}

void CalcBound(Dictionary<Point, char> space, out Point boundMin, out Point boundMax)
{
    var minX = space.Min(o => o.Key.X);
    var maxX = space.Max(o => o.Key.X);
    var minY = space.Min(o => o.Key.Y);
    var maxY = space.Max(o => o.Key.Y);

    boundMin = new Point(minX, minY);
    boundMax = new Point(maxX, maxY);
}

bool IsAbyssY(Point point, Dictionary<Point, char> space, int maxY)
{
    if (point.Y >= maxY)
        return true;
    return false;
}

bool IsAbyss(Point point, Dictionary<Point, char> space, Point min, Point max)
{
    if (point.X > max.X || point.X < min.X|| point.Y > max.Y)
        return true;
    return false;
}

Point Step(Point currPosition, Dictionary<Point, char> space)
{
    var possible = new Point(currPosition.X, currPosition.Y + 1);
    if (!space.ContainsKey(possible))
        return possible;

    possible = new Point(currPosition.X - 1, currPosition.Y + 1);
    if (!space.ContainsKey(possible))
        return possible;

    possible = new Point(currPosition.X + 1, currPosition.Y + 1);
    if (!space.ContainsKey(possible))
        return possible;

    return currPosition;
}

void PrintSpace(Dictionary<Point, char> space, Point min, Point max)
{
    for (var y = min.Y;  y <= max.Y; y++)
    {
        for (var x = min.X; x <= max.X; x++)
        {
            var point = new Point(x, y);
            if (space.ContainsKey(point))
                Console.Write(space[point]);
            else
                Console.Write('.');                
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

Dictionary<Point, char> ReadWalls(IEnumerable<string> lines)
{
    var walls = new Dictionary<Point, char>();
    foreach (var line in lines)
    {
        var points = line.Split(" -> ")
            .Select(o => o.Split(','))
            .Select(o => new Point(int.Parse(o[0]), int.Parse(o[1])))
            .ToArray();

        for (var i = 0; i < points.Length - 1; i++)
        {
            var startPoint = points[i];
            var endPoint = points[i + 1];

            if (startPoint.X < endPoint.X)
                PopulateX(startPoint, endPoint, walls, '#');
            else if (startPoint.X > endPoint.X)
                PopulateX(endPoint, startPoint, walls, '#');
            else if (startPoint.Y < endPoint.Y)
                PopulateY(startPoint, endPoint, walls, '#');
            else if (startPoint.Y > endPoint.Y)
                PopulateY(endPoint, startPoint, walls, '#');
            else
                throw new Exception("wtf?");
        }
    }

    return walls;
}

void PopulateX(Point startPoint, Point endPoint, Dictionary<Point, char> space, char populator)
{
    for (var x = startPoint.X; x <= endPoint.X; x++)
    {
        var newPoint = new Point(x, startPoint.Y);
        if (space.ContainsKey(newPoint))
            continue;

        space.Add(newPoint, populator);
    }
}

void PopulateY(Point startPoint, Point endPoint, Dictionary<Point, char> space, char populator)
{
    for (var y = startPoint.Y; y <= endPoint.Y; y++)
    {
        var newPoint = new Point(startPoint.X, y);
        if (space.ContainsKey(newPoint))
            continue;

        space.Add(newPoint, populator);
    }
}

record struct Point(int X, int Y);