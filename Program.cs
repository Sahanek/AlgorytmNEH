using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var path = Path.Combine(currentPath + @"\NEH9.dat"); // Zmień nazwe pliku, jeśli chcesz inny wynik
var tasks = new List<List<int>>();
using (StreamReader sr = new(path))
{
    var line = sr.ReadLine();
    var numberOfTask = int.Parse(line.Split(' ')[0]);
    for (int i = 0; i < numberOfTask; i++)
    {
        tasks.Add(new List<int>(sr.ReadLine().Split(' ').Where(x => x.Length > 0).Select(x => int.Parse(x))));
    }
}


//var sortedList = tasks.OrderByDescending(x => x.Sum()).ToList();


Console.WriteLine(CountNehTree(tasks.OrderByDescending(x => x.Sum()).ToList()));

int CountCmax(IReadOnlyList<IReadOnlyList<int>> tasks)
{
    var arrayWithCmax = new int[tasks.Count + 1, tasks[0].Count + 1];

    for (int i = 0; i < tasks.Count; i++)
    {
        for (int j = 0; j < tasks[i].Count; j++)
        {
            arrayWithCmax[i + 1, j + 1] = Math.Max(arrayWithCmax[i, j + 1], arrayWithCmax[i + 1, j]) + tasks[i][j];
        }
    }

    return arrayWithCmax[tasks.Count, tasks[0].Count];
}

int CountNehTree(List<List<int>> orderedTasks)
{
    int min = Int32.MaxValue;
    var listOfTaskToCountCmax = new List<List<int>>();
    List<List<int>> listWithBestCmax = new();

    for (var index = 0; index < orderedTasks.Count; index++)
    {
        var task = orderedTasks[index];
        min = Int32.MaxValue;
        for (int i = 0; i <= index; i++)
        {
            if (i >= listOfTaskToCountCmax.Count)
                listOfTaskToCountCmax.Add(task);
            else
                listOfTaskToCountCmax.Insert(i, task);

            var cMax = CountCmax(listOfTaskToCountCmax);

            if (min > cMax)
            {
                min = cMax;
                listWithBestCmax = listOfTaskToCountCmax.ToList();
            }

            listOfTaskToCountCmax.RemoveAt(i);
        }

        listOfTaskToCountCmax = listWithBestCmax.ToList();
    }

    return min;
}