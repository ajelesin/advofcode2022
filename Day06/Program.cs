
using System.Text;

Console.Write("release? (r) ");
var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

//Console.WriteLine("f1: " + f1(lines));
Console.WriteLine("f2: " + f2(lines));
Console.ReadLine();

static string f2(IEnumerable<string> lines)
{
    var sb = new StringBuilder();

    foreach (var line in lines)
    {
        var counter = f22(line);
        sb.Append(counter + " ");
    }

    return sb.ToString();
}

static int f22(string line)
{
    var queue = new Queue<char>();
    var counter = 0;

    foreach (var ch in line)
    {
        queue.Enqueue(ch);
        counter += 1;
        if (queue.Count == 14)
        {
            if (new HashSet<char>(queue).Count == 14)
            {
                return counter;
            }
            else
            {
                queue.Dequeue();
            }
        }
    }

    return -1;
}

static string f1(IEnumerable<string> lines)
{
    var sb = new StringBuilder();

    foreach (var line in lines)
    {
        var counter = f11(line);
        sb.Append(counter + " ");
    }

    return sb.ToString();
}

static int f11(string line)
{
    var queue = new Queue<char>();
    var counter = 0;

    foreach (var ch in line)
    {
        queue.Enqueue(ch);
        counter += 1;
        if (queue.Count == 4)
        {
            if (new HashSet<char>(queue).Count == 4)
            {
                return counter;
            }
            else
            {
                queue.Dequeue();
            }
        }
    }

    return -1;
}