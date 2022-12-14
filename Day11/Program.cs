

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);

Console.ReadLine();

static void f2(IEnumerable<string> lines)
{
    var monkies = ReadMonkies(lines);
    long total = monkies.Select(o => o.Test).Aggregate(1L, (a, b) => a * b);

    for (var round = 1; round <= 10_000; round++)
    {
        foreach (var currentMokey in monkies)
        {
            while (currentMokey.Items.Count > 0)
            {
                long item = currentMokey.Items.Dequeue();
                long inspectedItem = currentMokey.Inspect(item);
                long boredItem = inspectedItem % total;
                var nextMonkeyIndex = currentMokey.NextMonkey(boredItem);
                monkies[nextMonkeyIndex].Items.Enqueue(boredItem);
            }
        }
    }

    var activeMonkies = monkies.OrderByDescending(o => o.InspectAmount)
        .Take(2)
        .ToArray();
    ulong monkeyBusiness = (ulong) activeMonkies[0].InspectAmount * (ulong) activeMonkies[1].InspectAmount;

    Console.WriteLine(monkeyBusiness);
}

static void f1(IEnumerable<string> lines)
{
    var monkies = ReadMonkies(lines);

    for (var round = 1; round <= 20; round++)
    {
        foreach (var currentMokey in monkies)
        {
            while (currentMokey.Items.Count > 0)
            {
                long item = currentMokey.Items.Dequeue();
                long inspectedItem = currentMokey.Inspect(item);
                long boredItem = inspectedItem / 3;
                var nextMonkeyIndex = currentMokey.NextMonkey(boredItem);
                monkies[nextMonkeyIndex].Items.Enqueue(boredItem);
            }
        }
    }

    var activeMonkies = monkies.OrderByDescending(o => o.InspectAmount)
        .Take(2)
        .ToArray();
    long monkeyBusiness = activeMonkies[0].InspectAmount * activeMonkies[1].InspectAmount;

    Console.WriteLine(monkeyBusiness);
}

static List<Monkey> ReadMonkies(IEnumerable<string> lines)
{
    var monkies = new List<Monkey>();
    var allLines = lines.ToArray();

    for (var i = 0; i < allLines.Length; i += 7)
    {
        var monkey = new Monkey();

        var items = allLines[i + 1]
            .Substring("  Starting items: ".Length)
            .Split(", ")
            .Select(int.Parse)
            .ToArray();

        foreach (var item in items)
            monkey.Items.Enqueue(item);

        var operationTokens = allLines[i + 2]
            .Substring("  Operation: new = ".Length)
            .Split(' ')
            .ToArray();

        monkey.Operation = operationTokens[1];
        monkey.OperandLeft = operationTokens[0];
        monkey.OperandRight = operationTokens[2];

        var test = int.Parse(allLines[i + 3]
            .Substring("  Test: divisible by ".Length)
            );

        monkey.Test = test;

        var trueMonkey = int.Parse(allLines[i + 4]
            .Substring("    If true: throw to monkey ".Length)
            );

        monkey.TrueMonkey = trueMonkey;

        var faleMonkey = int.Parse(allLines[i + 5]
            .Substring("    If false: throw to monkey ".Length)
            );

        monkey.FalseMonkey = faleMonkey;

        monkies.Add(monkey);
    }

    return monkies;
}

class Monkey
{
    public int InspectAmount { get; set; }

    public Queue<long> Items { get; } = new();

    public string Operation { get; set; }

    public string OperandLeft { get; set; }

    public string OperandRight { get; set; }

    public int Test { get; set; }

    public int TrueMonkey { get; set; }

    public int FalseMonkey { get; set; }

    public long Inspect(long item)
    {
        InspectAmount++;
        long left = OperandLeft == "old" ? item : long.Parse(OperandLeft);
        long right = OperandRight == "old" ? item : long.Parse(OperandRight);
        long result = Operation == "+"
            ? left + right
            : Operation == "*"
            ? left * right
            : throw new Exception("wtf?");

        return result;
    }

    public int NextMonkey(long item)
    {
        return item % Test == 0
            ? TrueMonkey
            : FalseMonkey;
    }

    public override string ToString()
    {
        return "[" + string.Join(", ", Items) + "]";
    }
}