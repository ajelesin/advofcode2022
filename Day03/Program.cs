
f2();

static void f2()
{
    var lines = File.ReadLines("..\\..\\..\\input.txt");

    var lines3 = new List<string>();
    var totalPr = 0;

    foreach (var line in lines)
    {
        lines3.Add(line);

        if (lines3.Count == 3)
        {
            var badge = lines3[0].Intersect(lines3[1]).Intersect(lines3[2]).Single();
            var w = Weight(badge);
            totalPr += w;
            lines3.Clear();
        }

    }

    Console.WriteLine(totalPr);
}

static void f1()
{
    var lines = File.ReadLines("..\\..\\..\\input.txt");

    var totaPr = 0;

    foreach (var line in lines)
    {
        var l = line.Length >> 1;
        var first = line.Substring(0, l);
        var second = line.Substring(l);

        var theType = first.Intersect(second).Single();

        var w = Weight(theType);
        totaPr += w;
    }

    Console.WriteLine(totaPr);
}

static int Weight(char ch)
{
    if (char.IsUpper(ch))
        return ch - 38;
    else
        return ch - 96;
}