using System.Numerics;

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

f2(lines);
Console.ReadLine();

void f2(IEnumerable<string> lines)
{
    var d = lines
        .Select(o => o.Split(":", StringSplitOptions.TrimEntries))
        .ToDictionary(o => o[0], o => o[1]);

    var tokens = d["root"].Split(' ');
    var left = EvalC(d, tokens[0]);
    var right = EvalC(d, tokens[2]);

    var res = (left.Real - right.Real) / (right.Imaginary - left.Imaginary);
    Console.WriteLine(res);
}

Complex EvalC(Dictionary<string, string> d, string name)
{
    if (name == "humn")
        return new Complex(0, 1);

    var tokens = d[name].Split(' ');

    if (tokens.Length == 1)
        return new Complex(long.Parse(tokens[0]), 0);

    var first = EvalC(d, tokens[0]);
    var second = EvalC(d, tokens[2]);

    return tokens[1] switch
    {
        "+" => first + second,
        "-" => first - second,
        "/" => first / second,
        "*" => first * second,
        _ => throw new Exception("wft?"),
    };
}

void f1(IEnumerable<string> lines)
{
    var d = lines
        .Select(o => o.Split(":", StringSplitOptions.TrimEntries))
        .ToDictionary(o => o[0], o => o[1]);

    var result = Eval(d, "root");
    Console.WriteLine(result);
}

long Eval(Dictionary<string, string> list, string root)
{
    var tokens = list[root].Split(' ');

    if (tokens.Length == 1)
        return long.Parse(tokens[0]);

    var first = Eval(list, tokens[0]);
    var second = Eval(list, tokens[2]);

    return tokens[1] switch
    {
        "+" => first + second,
        "-" => first - second,
        "/" => first / second,
        "*" => first * second,
        _ => throw new Exception("wft?"),
    };
}