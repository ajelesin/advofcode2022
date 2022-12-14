
Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

//Console.WriteLine("f1: " + f1(lines));
Console.WriteLine("f2: " + f2(lines));
Console.ReadLine();

static long f2(IEnumerable<string> lines)
{
    var top = Read(lines);

    Size(top);

    var total = 70_000_000;
    var need = 30_000_000;

    var used = top.NextItems.Where(o => o.Name == "/")
        .Single()
        .Size;

    var free = total - used;
    var freeUp = need - free;

    long size = FindNearestSize(top, freeUp, used);
    return size;
}

static long f1(IEnumerable<string> lines)
{
    var top = Read(lines);

    Size(top);

    long size = FindSize(top);
    return size;
}

static Item Read(IEnumerable<string> lines)
{
    var top = new Item { NextItems = { new Item { Name = "/" } } };
    var current = top;

    var mode = string.Empty;
    foreach (var line in lines)
    {
        var tokens = line.Split(' ');
        if (tokens[0] == "$" && tokens[1] == "cd")
        {
            mode = "cmd";
            if (tokens[2] == "/")
            {
                current = top.NextItems.Where(o => o.Name == "/")
                    .Single();
            }
            else if (tokens[2] == "..")
            {
                current = current.Super;
            }
            else
            {
                current = current.NextItems
                    .Where(o => o.Name == tokens[2])
                    .Single();
            }
        }
        else if (tokens[0] == "$" && tokens[1] == "ls")
        {
            mode = "ls_out";
        }
        else if (mode == "ls_out")
        {
            if (tokens[0] == "dir")
            {
                current.NextItems.Add(new Item
                {
                    Super = current,
                    Name = tokens[1],
                });
            }
            else
            {
                current.NextItems.Add(new Item
                {
                    Super = current,
                    Name = tokens[1],
                    Size = long.Parse(tokens[0]),
                    IsFile = true,
                });
            }
        }
    }

    return top;
}

static void Size(Item item)
{
    if (item.Size > 0) return;

    foreach (var nextItem in item.NextItems)
        Size(nextItem);

    item.Size = item.NextItems.Sum(o => o.Size);
}

static long FindSize(Item item)
{
    if (item.IsFile) return 0;

    long size = 0;
    if (item.Size <= 100_000)
        size = item.Size;
    
    return size + item.NextItems.Sum(FindSize);
}

static long FindNearestSize(Item item, long freeUp, long currFreeUp)
{
    if (item.IsFile) return currFreeUp;
    if (item.Size < freeUp) return currFreeUp;

    var result = Math.Min(item.Size, currFreeUp);
    foreach (var nextItem in item.NextItems)
    {
        result = Math.Min(
            FindNearestSize(nextItem, freeUp, currFreeUp),
            result);
    }

    return result;
}

class Item
{
    public string Name { get; set; }

    public long Size { get; set; }

    public bool IsFile { get; set; }

    public Item Super { get; set; }

    public List<Item> NextItems { get; } = new();
}