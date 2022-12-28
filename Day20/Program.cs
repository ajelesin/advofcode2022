using System.Xml.Linq;

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);
Console.ReadLine();

void f2(IEnumerable<string> lines)
{
    var list = new LinkedList<long>();
    var nodes = lines.Select(long.Parse)
        .Select(o => o * 811589153L)
        .Select(list.AddLast)
        .ToArray();

    for (var k = 0; k < 10; k++)
    {
        for (var i = 0; i < nodes.Length; i++)
        {
            var node = nodes[i];
            var shift = node.Value % (list.Count - 1);

            if (shift > 0)
            {
                for (var j = 0; j < shift; j++)
                {
                    var tempNode = node.Next ?? node.List.First;
                    list.Remove(node);
                    list.AddAfter(tempNode, node);
                }
            }
            else if (shift < 0)
            {
                for (var j = 0; j > shift; j--)
                {
                    var tempNode = node.Previous ?? node.List.Last;
                    list.Remove(node);
                    list.AddBefore(tempNode, node);
                }
            }
        }
    }

    long result = 0;
    var found = nodes.Where(o => o.Value == 0).First();
    for (var i = 0; i <= 3000; i++)
    {
        if (i == 1000 || i == 2000 || i == 3000)
            result += found.Value;
        found = found.Next ?? found.List.First;
    }

    Console.WriteLine(result);
}

void f1(IEnumerable<string> lines)
{
    var list = new LinkedList<int>();
    var nodes = lines.Select(int.Parse)
        .Select(list.AddLast)
        .ToArray();

    for (var i = 0; i < nodes.Length; i++)
    {
        var node = nodes[i];
        var shift = node.Value % (list.Count - 1);

        if (shift > 0)
        {
            for (var j = 0; j < shift; j++)
            {
                var tempNode = node.Next ?? node.List.First;
                list.Remove(node);
                list.AddAfter(tempNode, node);
            }
        }
        else if (shift < 0)
        {
            for (var j = 0; j > shift; j--)
            {
                var tempNode = node.Previous ?? node.List.Last;
                list.Remove(node);
                list.AddBefore(tempNode, node);
            }
        }
    }

    var result = 0;
    var found = nodes.Where(o => o.Value == 0).First();
    for (var i = 0; i <= 3000; i++)
    {
        if (i == 1000 || i == 2000 || i == 3000)
            result += found.Value;
        found = found.Next ?? found.List.First;
    }

    Console.WriteLine(result);
}

LinkedListNode<T> SwapRight<T>(LinkedListNode<T> first, LinkedList<T> list)
{
    var next = first.Next;
    list.Remove(first);
    if (next is null)
        list.AddAfter(list.First, first);
    else
        list.AddAfter(next, first);
    return first;
}

LinkedListNode<T> SwapLeft<T>(LinkedListNode<T> first, LinkedList<T> list)
{
    var prev = first.Previous;
    list.Remove(first);
    if (prev is null)
        list.AddLast(first);
    else
        list.AddBefore(prev, first);
    return first;
}