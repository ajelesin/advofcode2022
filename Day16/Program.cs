Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f1(lines);
Console.ReadLine();

void f1(IEnumerable<string> lines)
{

}

class Item
{
    public int Flow { get; set; }

    public int SpentMinutes { get; set; }

    public int Cost(int restMinutes)
    {
        return Flow * restMinutes;
    }
}