

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);
Console.ReadLine();

void f2(IEnumerable<string> lines)
{
    var map = new List<string>();
    foreach (var line in lines)
    {
        map.Add(line);
    }

    var E = Search('E', map);
    var shortes = int.MaxValue;

    for (var x = 0; x < map[0].Length; x++)
    {
        for (var y = 0; y < map.Count; y++)
        {
            if (map[y][x] == 'a')
            {
                var parrents = FindPath(new Point(x, y), E, map);

                var path = E;
                var finalPath = new Stack<char>();
                while (parrents.ContainsKey(path))
                {
                    finalPath.Push(map[path.y][path.x]);
                    path = parrents[path];
                }
                finalPath.Push(map[path.y][path.x]);
                if (finalPath.Count == 1)
                    continue;

                if (finalPath.Count < shortes)
                    shortes = finalPath.Count;
                Console.WriteLine(finalPath.Count);
            }
        }
    }

    Console.WriteLine(shortes - 1);
}


void f1(IEnumerable<string> lines)
{
    var map = new List<string>();
    foreach (var line in lines)
    {
        map.Add(line);
    }

    var S = Search('S', map);
    var E = Search('E', map);

    var parrents = FindPath(S, E, map);

    var path = E;
    var finalPath = new Stack<char>();
    while (parrents.ContainsKey(path))
    {
        finalPath.Push(map[path.y][path.x]);
        path = parrents[path];
    }
    finalPath.Push(map[path.y][path.x]);

    Console.WriteLine(finalPath.Count);
    while (finalPath.Count > 0)
        Console.Write(" -> " + finalPath.Pop());
}

Dictionary<Point, Point> FindPath(Point start, Point end, List<string> map)
{
    var parrents = new Dictionary<Point, Point>();
    var queue = new Queue<Point>();
    var visited = new HashSet<Point>();

    queue.Enqueue(start);
    while (queue.Count > 0)
    {
        var curr = queue.Dequeue();
        if (curr == end) break;

        visited.Add(curr);
        var expand = FindNext(curr, map);
        foreach (var p in expand)
        {
            if (visited.Contains(p)) continue;
            queue.Enqueue(p);
            visited.Add(p);
            parrents[p] = curr;
        }
    }

    return parrents;
}

Point Search(char v, List<string> map)
{
    for (var y = 0; y < map.Count; y++)
    {
        for (var x = 0; x < map[0].Length; x++)
        {
            if (map[y][x] == v)
                return new Point(x, y);
        }
    }
    throw new Exception("wtf?");
}

Point[] FindNext(Point curr, List<string> map)
{
    var nextPoints = new List<Point>();
    foreach (var newPoint in new[] { new Point(curr.x, curr.y + 1),
                                    new Point(curr.x, curr.y - 1),
                                    new Point(curr.x + 1, curr.y),
                                    new Point (curr.x - 1, curr.y)})
    {
        if (newPoint.x < 0 || newPoint.y < 0) continue;
        if (newPoint.x >= map[0].Length || newPoint.y >= map.Count) continue;

        if (!Can(map[newPoint.y][newPoint.x], map[curr.y][curr.x]))
            continue;

        nextPoints.Add(newPoint);
    }

    return nextPoints.ToArray();
}

char Elevation(char c)
{
    if (c == 'S') return 'a';
    if (c == 'E') return 'z';
    return c;
}

bool Can(char next, char curr)
{
    return Elevation(curr) >= Elevation(next)
        || Elevation(next) - Elevation(curr) == 1;
}

record struct Point(int x, int y);