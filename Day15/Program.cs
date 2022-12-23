
using System.Text.RegularExpressions;

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);
Console.ReadLine();

void f2(IEnumerable<string> lines)
{
    var sensors = ReadSensors(lines);

    var min = new Point(0, 0);
    var max = new Point(4000000, 4000000);

    var points = new HashSet<Point>();

    for (int i = 0; i < sensors.Count; i++)
    {
        var s1 = sensors[i];

        for (int j = i + 1; j < sensors.Count; j++)
        {
            var s2 = sensors[j];

            if (Distance(s1.Position, s2.Position) != s1.Distance + s2.Distance + 2)
                continue;

            var maxY = Math.Min(s1.Position.Y + s1.Distance, s2.Position.Y + s2.Distance);
            var minY = Math.Max(s1.Position.Y - s1.Distance, s2.Position.Y - s2.Distance);
            var maxX = Math.Min(s1.Position.X + s1.Distance, s2.Position.X + s2.Distance);
            var minX = Math.Max(s1.Position.X - s1.Distance, s2.Position.X - s2.Distance);

            for (var y = minY; y < maxY; y++)
            {
                var x1 = s1.Position.X + (s1.Distance + 1 - Math.Abs(y - s1.Position.Y));
                var x2 = s1.Position.X - (s1.Distance + 1 - Math.Abs(y - s1.Position.Y));

                if (x1 >= min.X && x1 <= max.X && x1 >= minX && x1 <= maxX)
                {
                    points.Add(new Point(x1, y));
                }

                if (x2 >= min.X && x2 <= max.X && x2 >= minX && x2 <= maxX)
                {
                    points.Add(new Point(x2, y));
                }
            }
        }
    }

    foreach (var point in points)
    {
        if (CannotBe(point.X, point.Y, sensors))
            continue;

        Console.WriteLine(point);
        Console.WriteLine(point.X * 4000000L + point.Y);
        break;
    }

}

void f1(IEnumerable<string> lines)
{
    var sensors = ReadSensors(lines);

    CalcSpaceBound(sensors, out var globalMin, out var globalMax);

    var amount = TraceY(sensors, globalMin.X, globalMax.X, 2000000);
    Console.WriteLine(amount);
}

List<Sensor> ReadSensors(IEnumerable<string> lines)
{
    var sensors = new List<Sensor>();
    var rx = new Regex(@"x=(-?\d+), y=(-?\d+)");

    foreach (var line in lines)
    {
        var tokens = rx.Matches(line);

        var sensor = CreateSensor(
            new Point(int.Parse(tokens[0].Groups[1].Value), int.Parse(tokens[0].Groups[2].Value)),
            new Point(int.Parse(tokens[1].Groups[1].Value), int.Parse(tokens[1].Groups[2].Value))
            );

        sensors.Add(sensor);
    }

    return sensors;
}

void CalcSpaceBound(List<Sensor> sensors, out Point min, out Point max)
{
    min = new Point(0, 0);
    max = new Point(0, 0);

    foreach (var sensor in sensors)
    {
        CalcLocalBound(sensor.Position, sensor.Distance, out var sensorBoundMin, out var sensorBoundMax);
        ExpandBound(min, max, sensorBoundMin, sensorBoundMax, out var newMin, out var newMax);
        min = newMin;
        max = newMax;
    }
}

int TraceY(List<Sensor> sensors, int minX, int maxX, int y)
{
    var amount = 0;

    for (var x = minX; x <= maxX; x++)
    {
        var possiblePosition = CannotBe(x, y, sensors);
        if (possiblePosition)
            amount++;
    }

    return amount;
}

bool CannotBe(int x, int y, List<Sensor> sensors)
{
    var testPoint = new Point(x, y);
    var possiblePosition = false;

    foreach (var sensor in sensors)
    {
        var d = Distance(sensor.Position, testPoint);
        if (d > sensor.Distance) continue;

        if (sensor.Beacon == testPoint)
            continue;

        possiblePosition = true;
        break;
    }

    return possiblePosition;
}

void ExpandBound(Point currMin, Point currMax, Point min, Point max, out Point newMin, out Point newMax)
{
    newMin = new Point(Math.Min(currMin.X, min.X), Math.Min(currMin.Y, min.Y));
    newMax = new Point(Math.Max(currMax.X, max.X), Math.Max(currMax.Y, max.Y));
}

void CalcLocalBound(Point position, int distance, out Point min, out Point max)
{
    min = new Point(position.X - distance, position.Y - distance);
    max = new Point(position.X + distance, position.Y + distance);
}

int Distance(Point a, Point b)
{
    return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
}

Sensor CreateSensor(Point position, Point beacon)
{
    var d = Distance(position, beacon);
    return new Sensor(position, beacon, d);
}

record struct Sensor(Point Position, Point Beacon, int Distance);
record struct Point(int X, int Y);