Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f1(lines);
Console.ReadLine();

void f1(IEnumerable<string> lines)
{
    var x = lines.Select(Snafu2Dec).Sum();
    Console.WriteLine(Dec2Snafu(x));

}


long Snafu2Dec(string snafu)
{
    long res = 0;

    int deg = snafu.Length - 1;
    foreach (var d in snafu)
    {
        if (d == '1') res += (1 * (long)Math.Pow(5, deg));
        else if (d == '2') res += (2 * (long)Math.Pow(5, deg));
        else if (d == '0') res += (0 * (long)Math.Pow(5, deg));
        else if (d == '-') res += (-1 * (long)Math.Pow(5, deg));
        else if (d == '=') res += (-2 * (long)Math.Pow(5, deg));
        else throw new Exception("xyi");
        deg--;
    }

    return res;
}

string Dec2Snafu(long dec)
{
    var res = "";
    while (dec > 0)
    {
        var d = dec % 5;
        dec /= 5;

        switch (d)
        {
            case 0: res = '0' + res; break;
            case 1: res = '1' + res; break;
            case 2: res = '2' + res; break;
            case 3: res = '=' + res; dec++; break;
            case 4: res = '-' + res; dec++; break;
        }
    }
    return res;
}