
f2();

static void f2()
{
    var lines = File.ReadLines("..\\..\\..\\input.txt");

    var intersected = 0;

    foreach (var line in lines)
    {
        var parts = line.Split(',').ToArray();
        var first = parts[0].Split('-').Select(int.Parse).ToArray();
        var second = parts[1].Split('-').Select(int.Parse).ToArray();

        int a = first[0], b = first[1], c = second[0], d = second[1];

        if (b < c || d < a) continue;
        intersected++;

    }

    Console.WriteLine(intersected);
}

static void f1()
{
    var lines = File.ReadLines("..\\..\\..\\input.txt");

    var intersected = 0;

    foreach (var line in lines)
    {
        var parts = line.Split(',').ToArray();
        var first = parts[0].Split('-').Select(int.Parse).ToArray();
        var second = parts[1].Split('-').Select(int.Parse).ToArray();

        if (first[0] >= second[0] && first[1] <= second[1]
            || second[0] >= first[0] && second[1] <= first[1])
            intersected++;
    }

    Console.WriteLine(intersected);
}