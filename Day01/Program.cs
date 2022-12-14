
var lines = File.ReadLines("..\\..\\..\\input.txt");
var calloriesPerElf = new List<int>();
var currentCalories = 0;

foreach (var line in lines)
{
    if (!string.IsNullOrEmpty(line))
    {
        currentCalories += int.Parse(line);
    }
    else
    {
        calloriesPerElf.Add(currentCalories);
        currentCalories = 0;
    }
}
calloriesPerElf.Add(currentCalories);

Console.WriteLine(calloriesPerElf.OrderByDescending(o => o).Take(3).Sum());