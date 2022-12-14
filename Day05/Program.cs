
Console.Write("release? (r) ");
var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\default_input.txt");

//Console.WriteLine("f1: " + f1(lines));
Console.WriteLine("f2: " + f2(lines));

static string f2(IEnumerable<string> lines)
{
    var stacks = new Dictionary<int, Stack<char>>();

    var enumerator = lines.GetEnumerator();

    while (enumerator.MoveNext())
    {
        var line = enumerator.Current;
        if (string.IsNullOrEmpty(line))
            break;

        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == '[')
            {
                var index = (i / 4) + 1;
                if (!stacks.ContainsKey(index))
                    stacks.Add(index, new Stack<char>());

                stacks[index].Push(line[i + 1]);
            }
        }
    }

    stacks = stacks.ToDictionary(
        o => o.Key,
        o => new Stack<char>(o.Value));

    while (enumerator.MoveNext())
    {
        var line = enumerator.Current;
        var tokens = line.Split(' ');
        var count = int.Parse(tokens[1]);
        var from = int.Parse(tokens[3]);
        var to = int.Parse(tokens[5]);

        var tempStack = new Stack<char>();
        for (var i = 0; i < count; i++)
        {
            tempStack.Push(stacks[from].Pop());
        }

        for (var i = 0; i < count; i++)
        {
            stacks[to].Push(tempStack.Pop());
        }
    }

    var result = string.Empty;
    foreach (var i in stacks.Keys.OrderBy(o => o))
    {
        result += stacks[i].Peek();
    }

    return result;
}

static string f1(IEnumerable<string> lines)
{
    var stacks = new Dictionary<int, Stack<char>>();

    var enumerator = lines.GetEnumerator();

    while (enumerator.MoveNext())
    {
        var line = enumerator.Current;
        if (string.IsNullOrEmpty(line))
            break;

        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == '[')
            {
                var index = (i / 4) + 1;
                if (!stacks.ContainsKey(index))
                    stacks.Add(index, new Stack<char>());
                
                stacks[index].Push(line[i + 1]);                    
            }
        }
    }

    stacks = stacks.ToDictionary(
        o => o.Key,
        o => new Stack<char>(o.Value));

    while (enumerator.MoveNext())
    {
        var line = enumerator.Current;
        var tokens = line.Split(' ');
        var count = int.Parse(tokens[1]);
        var from = int.Parse(tokens[3]);
        var to = int.Parse(tokens[5]);

        for (var i = 0; i < count; i++)
        {
            stacks[to].Push(stacks[from].Pop());
        }
    }

    var result = string.Empty;
    foreach (var i in stacks.Keys.OrderBy(o => o))
    {
        result += stacks[i].Peek();
    }

    return result;
}