using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLPLogixGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //populate initial state randomly 20 rows and 20 columns
            var data = Enumerable.Range(1, 20).Select(x =>
                 Enumerable.Range(1, 20).Select(y =>
                      (new Random().NextDouble() > 0.5 ? Convert.ToChar('X') : ' ')
                 ).ToList()
            ).ToList();

            // print intial state to console
            PrintToConsole(data);

            List<List<char>> nextRound = null;
            // evaluate next round data
            nextRound = ApplyNextRound(data);

            Task.Run(async () =>
            {
                while (true)
                {
                    //every 1 second evaluate next round and print
                    await Task.Delay(1000);

                    PrintToConsole(nextRound);
                    nextRound = ApplyNextRound(nextRound);

                }
            });

            Console.Read();
        }

        private static List<List<char>> ApplyNextRound(List<List<char>> data)
        {
            List<List<char>> nextRound = Enumerable.Range(1, data.Count).Select(x =>
                        Enumerable.Range(1, data[0].Count).Select(y =>
                          Convert.ToChar(' ')
                        ).ToList()
                        ).ToList();
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {

                    var neighbors = getNeighboringNodes(data, i, j);

                    int aliveCnt = neighbors.FindAll(x => x == 'X').Count;

                    nextRound[i][j] = data[i][j];
                    //Any live cell with fewer than two or more than three live neighbours dies
                    if (data[i][j] == 'X' && (aliveCnt < 2 || aliveCnt > 3))
                        nextRound[i][j] = ' ';
                    //Any dead cell with three live neighbours becomes a live cell
                    else if (data[i][j] == ' ' && aliveCnt == 3)
                        nextRound[i][j] = 'X';
                }

            }
            return nextRound;
        }

        private static void PrintToConsole(List<List<char>> data)
        {
            Console.Clear();
            foreach (var row in data)
            {
                Console.WriteLine();
                Console.Write(' ');
                foreach (var item in row)
                {
                    Console.Write(item + " | ");
                }
                Console.WriteLine();
            }
        }

        private static List<char> getNeighboringNodes(List<List<char>> data, int i, int j)
        {
            List<char> neighbours = new List<char>();
            var row_limit = data.Count;
            if (row_limit > 0)
            {
                int column_limit = data[0].Count;
                for (int x = Math.Max(0, i - 1); x <= Math.Min(i + 1, row_limit); x++)
                {
                    for (int y = Math.Max(0, j - 1); y <= Math.Min(j + 1, column_limit); y++)
                    {
                        if (y == column_limit || x == row_limit) continue; //to prevent indexoutofrange exception
                        if ((x != i) || (y != j))// don't consider the element itself as a neighbor
                        {
                            neighbours.Add(data[x][y]);
                        }
                    }

                }
            }
            return neighbours;
        }
    }
}