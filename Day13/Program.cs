
using System.Text.Json.Nodes;

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);
Console.ReadLine();

void f2(IEnumerable<string> lines)
{
    var separatorList = (new[] { "[[2]]", "[[6]]" })
        .Select(o => JsonNode.Parse(o))
        .ToArray();

    var list = lines
        .Where(o => !string.IsNullOrEmpty(o))
        .Select(o => JsonNode.Parse(o))
        .Concat(separatorList)
        .ToList();

    list.Sort(Compare);

    var result = 
        (list.IndexOf(separatorList[0]) + 1)
        * (list.IndexOf(separatorList[1]) + 1);

    Console.WriteLine(result);
}


void f1(IEnumerable<string> lines)
{
    var pairs = lines
        .Where(o => !string.IsNullOrEmpty(o))
        .Select(o => JsonNode.Parse(o))
        .Chunk(2)
        .Select((pair, index) => Compare(pair[0], pair[1]) < 0 ? index + 1 : 0)
        .Sum();

    Console.WriteLine(pairs);
}



int Compare(JsonNode left, JsonNode right)
{
    if (left is JsonValue && right is JsonValue)
    {
        return (int)left - (int)right;
    }
    else
    {
        var leftList = left as JsonArray ?? new JsonArray((int)left);
        var rightList = right as JsonArray ?? new JsonArray((int)right);
        return Enumerable.Zip(leftList, rightList)
            .Select(p => Compare(p.First, p.Second))
            .FirstOrDefault(c => c != 0, leftList.Count - rightList.Count);
    }
}