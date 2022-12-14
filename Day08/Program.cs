
using System.Globalization;
using System.Net.Sockets;

Console.Write("release? (r) ");

var lines = Console.ReadLine() == "r"
    ? File.ReadLines("..\\..\\..\\input.txt")
    : File.ReadLines("..\\..\\..\\debug.txt");

//Console.WriteLine("f1: " + f1(lines));
Console.WriteLine("f2: " + f2(lines));
Console.ReadLine();

static int f2(IEnumerable<string> lines)
{
    var forest = new List<List<int>>();

    foreach (var line in lines)
    {
        forest.Add(
            line.Select(char.ToString)
            .Select(int.Parse)
            .ToList()
        );
    }

    var l = forest[0].Count;
    var h = forest.Count;

    var score = 0;

    for (var row = 1; row < l - 1; row++)
    {
        for (var column = 1; column < h - 1; column++)
        {
            var tmpScore = Score(row, column, forest);
            if (tmpScore > score)
                score = tmpScore;
        }
    }

    return score;

}

static int Score(int row, int column, List<List<int>> forest)
{
    var x = 0;
    for (var c = column - 1; c >= 0; c--)
    {
        x++;
        if (forest[row][c] >= forest[row][column])
            break;
    }

    var y = 0;
    for (var c = column + 1; c < forest[0].Count; c++)
    {
        y++;
        if (forest[row][c] >= forest[row][column])
            break;
    }

    var z = 0;
    for (var r = row - 1; r >= 0; r--)
    {
        z++;
        if (forest[r][column] >= forest[row][column])
            break;
    }

    var k = 0;
    for (var r = row + 1; r < forest.Count; r++)
    {
        k++;
        if (forest[r][column] >= forest[row][column])
            break;
    }

    return x * y * z * k;
}

static int f1(IEnumerable<string> lines)
{
    var forest = new List<List<int>>();

    foreach (var line in lines)
    {
        forest.Add(
            line.Select(char.ToString)
            .Select(int.Parse)
            .ToList()
        );
    }

    var l = forest[0].Count;
    var h = forest.Count;

    var visibleCounter = 0;

    for (var row = 1; row < l - 1; row++)
    {
        for (var column = 1; column < h - 1; column++)
        {
            if (!Hidden(row, column, forest))
                visibleCounter++;
        }
    }

    return visibleCounter + ((l - 1) * 2 + (h - 1) * 2);
}

static bool Hidden(int row, int column, List<List<int>> forest)
{
    bool flag = false;
    for (var c = column - 1; c >= 0; c--)
    {
        if (forest[row][c] >= forest[row][column])
        {
            flag = true;
            break;
        }
    }

    if (!flag) return false;

    flag = false;
    for (var c = column + 1; c < forest[0].Count; c++)
    {
        if (forest[row][c] >= forest[row][column])
        {
            flag = true;
            break;
        }
    }

    if (!flag) return false;

    flag = false;
    for (var r = row - 1; r >= 0; r--)
    {
        if (forest[r][column] >= forest[row][column])
        {
            flag = true;
            break;
        }
    }

    if (!flag) return false;

    flag = false;
    for (var r = row + 1; r < forest.Count; r++)
    {
        if (forest[r][column] >= forest[row][column])
        {
            flag = true;
            break;
        }
    }

    if (!flag) return false;

    return true;
}