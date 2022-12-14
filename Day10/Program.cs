

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f1(lines);

Console.ReadLine();

static void f1(IEnumerable<string> lines)
{
    int X = 1;
    var cycle = 0;
    var result = 0;
    var crt = string.Empty;

    var x = lines.ToList();
    for (var i = 0; i < x.Count; i++)
    {
        var line = x[i];
        var tokens = line.Split(' ').ToArray();

        var currCycles = 0;
        var done = false;

        while(!done)
        {

            crt += GetChar(X, cycle % 40);

            cycle++;
            currCycles++;

            if (cycle % 40 == 20)
            {
                result += (cycle * X);
            }

            if (tokens[0] == "noop")
            {
                done = true;
            }

            if (tokens[0] == "addx")
            {
                if (currCycles == 2)
                {
                    X += int.Parse(tokens[1]);
                    done = true;
                }
            }
        }
    }

    Console.WriteLine(result);
    var index = 0;
    foreach (var ch in crt)
    {
        Console.Write(ch);
        index++;
        if (index % 40 == 0)
            Console.WriteLine();
    }
}

static string GetChar(int X, int position)
{
    if (position == X - 1 || position == X || position == X + 1)
        return "#";
    return ".";
}