f2();

static void f2()
{
    const string rivalWins = "AZBXCYA";
    const string wins = "AYCXBZA";

    var lines = File.ReadLines("..\\..\\..\\input.txt");

    var totalScore = 0;
    foreach (var line in lines)
    {
        if (line[2] == 'X')
        {
            var i = 0; while (rivalWins[i++] != line[0]) ;
            totalScore += rivalWins[i] - 'W';
        }
        else if (line[2] == 'Z')
        {
            var i = 0; while (wins[i++] != line[0]) ;
            totalScore += 6 + wins[i] - 'W';
        }
        else
        {
            totalScore += 3 + line[0] - '@';
        }
    }

    Console.WriteLine(totalScore);
}

static void f1()
{
    const string rivalWins = "A Z B X C Y A";
    const string wins = "A Y C X B Z A";

    var lines = File.ReadLines("..\\..\\..\\input.txt");

    var totalScore = 0;
    foreach (var line in lines)
    {
        totalScore += line[2] - 'W';
        
        if (wins.Contains(line))
            totalScore += 6;
        else if (!rivalWins.Contains(line))
            totalScore += 3;
    }

    Console.WriteLine(totalScore);
}