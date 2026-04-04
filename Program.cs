using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Astar
{
    public class Program
    {
        public const float sqrt2 = 1.41421356f;
        static void Main(string[] args)
        {
            Graph graphMap = new Graph();

            (int x, int y)[] blocked = new (int x, int y)[]
            {
                (3,1),(3,2),(3,3),(4,1),(5,1),(2,3),(1,3),(0,3),(2,1),(2,5),
                (15,12),(15,13),(14,15),(9,11),(12,9),(6,16),(17,3),(3,7),(6,9),
                (16,12),(12,13),(13,16),(7,14),(12,8),(7,16),(16,5),(2,8),(7,10),
                (15,15),(12,12),(13,13),(14,1),(15,1),(12,3),(11,3),(10,3),(12,1),(12,5),
                (3,11),(3,12),(3,13),(4,11),(5,11),(2,13),(1,13),(0,13),(2,11),(2,15),
                (7,7),(4,20),(8,2),(7,2),(6,2),(2,9),(18,11),(3,19),(2,19),(5,19),
                (10,10),(10,11),(10,12),(11,10),(12,10),(10,9),(11,11),(12,13),(8,11),(7,11),
            };

            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (!blocked.Contains((x, y)))
                    {
                        graphMap.AddNode(x, y);
                    }
                }
            }

            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (graphMap.Nodes.TryGetValue((x, y), out Node node))
                    {
                        graphMap.TryAddEdge(sqrt2, node, (x + 1, y + 1));
                        graphMap.TryAddEdge(node, (x + 1, y));
                        graphMap.TryAddEdge(sqrt2, node, (x + 1, y - 1));
                        graphMap.TryAddEdge(node, (x, y - 1));
                    }
                }
            }

            (int x, int y) start = (7, 18);
            //(int x, int y) goal = (18, 18);
            (int x, int y) escape = (9, 16);
            //LinkedList<Node> path = graphMap.AStar(start, goal, out bool successful2);
            //drawMap(path, graphMap, start, goal);
            LinkedList<Node> path2 = graphMap.AwayStar(start, escape, 12, out bool successful);
            drawMap(path2, graphMap, start, escape);
            Console.WriteLine("\n");

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("  ");
            Console.ResetColor();
            //Console.WriteLine(" = Goal");
            Console.WriteLine(" = Escape");


            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("  ");
            Console.ResetColor();
            Console.WriteLine(" = Start");

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("  ");
            Console.ResetColor();
            Console.WriteLine(" = Path");

            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("  ");
            Console.ResetColor();
            Console.WriteLine(" = Barrier");

            Console.ReadLine();

        }

        public static void drawMap(LinkedList<Node> path, Graph graphMap, (int x, int y) start, (int x, int y) extraPoint)
        {
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if ((y, x) == extraPoint)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    }
                    else if ((y, x) == start)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else if (!graphMap.Nodes.TryGetValue((y, x), out Node node))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    else if (path.Contains(node))
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Console.Write("  " + (y == 19 ? "\n" : ""));
                }
            }
        }
    }
}
