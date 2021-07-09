using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Security;

namespace Graph
{
    class MainClass
    {
        public static void generatePermutations(int k, int n, List<string>P, ref List<List<string>> Pathes)
        {
            if(k==n)
            {
                Pathes.Add(new List<string>());
                foreach (var v in P)
                    Pathes[Pathes.Count - 1].Add(v);
                Pathes[Pathes.Count - 1].Add(Pathes[Pathes.Count - 1][0]);
            }
            else
            {
                for (int j = k; j < n; j++)
                {
                    Swap(ref P,k, j);
                    generatePermutations(k + 1, n, P, ref Pathes);
                    Swap(ref P,k, j);
                }
            }
        }

        static void Swap(ref List<string> l, int i, int j)
        {
            string temp = l[i];
            l[i] = l[j];
            l[j] = temp;
        }

        public static string Min(List <string> vertexes, List<string> path, Graph g)
        {
            int min = int.MaxValue;
            string minV = vertexes[0];
            foreach (var v in vertexes)
            {
                int currentMin = g.GetNeigbours(v, path).Count;
                if (min > currentMin)
                {
                    min = currentMin;
                    minV = v;
                }
            }
            return minV;
        }

        public static double fitness(List<string> p, Graph g)
        {
            double res = 0;
            for (int i=0; i<p.Count-1; i++)
            {
                res += g.GetWeight(p[i], p[i + 1]);
            }
            return res;
        }
        public static string PrintList(List<string>S)
        {
            string res = string.Empty;
            foreach (var s in S)
                res+=s + " ";
            return res;
        }
        public static void Main(string[] args)
        {

            List<string> P = new List<string>() { "A", "B", "C", "D", "E", "F" };
            List <List<string>> Pathes = new List<List<string>>();
           
            generatePermutations(0, P.Count, P, ref Pathes);

            var g = new Graph();
            foreach (var p in P)
                g.AddVertex(p);

            g.AddEdge("A", "B", 6);
            g.AddEdge("A", "C", 9);
            g.AddEdge("A", "D", 11);
            g.AddEdge("A", "E", 10);
            g.AddEdge("A", "F", 8);
            g.AddEdge("B", "D", 6);
            g.AddEdge("B", "C", 9);
            g.AddEdge("B", "E", 8);
            g.AddEdge("B", "F", 12);
            g.AddEdge("C", "D", 7);
            g.AddEdge("C", "E", 8);
            g.AddEdge("C", "F", 3);
            g.AddEdge("D", "E", 6);
            g.AddEdge("D", "F", 5);
            g.AddEdge("E", "F", 4);

            Console.WriteLine(g);
            Console.WriteLine();

            Stopwatch stw = new Stopwatch();




            stw.Start();
            double min = double.PositiveInfinity;
            List<string> minPath = new List<string>();

            foreach (var v in Pathes)
            {
                double c = fitness(v,g);
                if (c < min)
                {
                    min = c;
                    minPath = v;
                }
                for (int i=0; i< v.Count; i++)
                {
                    //if (i != v.Count - 1)
                    //c += g.GetWeight(v[i],v[i + 1]);
                    Console.Write(v[i] + " ");
                }
                Console.Write(c + "\n");
            }

            stw.Stop();
            Console.WriteLine("\n");
            Console.WriteLine(PrintList(minPath) + min);
            Console.WriteLine("Решение перебором заняло "+ stw.ElapsedMilliseconds+"мс");
            Console.WriteLine();
            stw.Restart();

             min = double.PositiveInfinity;
            minPath = new List<string>();
            double T = 100;
            double α = 0.99;
            List<string> S = new List<string> {"A", "B", "C", "D", "E","F","A"};
            double L = fitness(S, g);
            Random rnd = new Random();
            while (T > 1)
            {
                var S1 = new List<string>(S);
                Console.WriteLine(PrintList(S1) + fitness(S1, g));
                S1.Remove(S1[S1.Count - 1]);
                Swap(ref S1, rnd.Next(S1.Count), rnd.Next(S1.Count));
                S1.Add(S1[0]);
                Console.WriteLine(PrintList(S1) + fitness(S1, g));
                double L1 = fitness(S1, g);
                if (L1 < L)
                {
                    S = S1;
                    L = L1;
                }
                else
                {
                    double prob = Math.Exp(-(L1 - L) / T);
                    if (prob <= rnd.NextDouble())
                    {
                        S = S1;
                        L = L1;
                    }
                }
                T *= α;
                if (L<min)
                {
                    min = L;
                    minPath = new List<string>(S);
                }
            }
            Console.WriteLine();
            Console.WriteLine("\n");
            if (L<min)
            {
                Console.WriteLine(PrintList(S) + fitness(S, g));
            }
            else
            {
                Console.WriteLine(PrintList(minPath) + min);
            }
            stw.Stop();

            Console.WriteLine("Решение методом отжига заняло " + stw.ElapsedMilliseconds + "мс");

            /*

            var gr = new Graph();
            string[] letters = {"A", "B", "C", "D", "E", "F", "G", "H"};
            string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8" };

            foreach (var l in letters)
                foreach (var n in numbers)
                    gr.AddVertex(l + n);
           

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    string v1 = letters[i] + numbers[j];
                    string v2;
                    if (i - 1 >= 0 && j + 2 < 8)
                    {
                        v2 = letters[i - 1] + numbers[j + 2];
                        gr.AddEdge(v1, v2);
                    }
                    if (i - 2 >= 0 && j + 1 < 8)
                    {
                        v2 = letters[i - 2] + numbers[j + 1];
                        gr.AddEdge(v1, v2);
                    }
                    if (i + 1 < 8 && j + 2 < 8)
                    {
                        v2 = letters[i + 1] + numbers[j + 2];
                        gr.AddEdge(v1, v2);
                    }
                    if (i + 2 < 8 && j + 1 < 8)
                    {
                        v2 = letters[i + 2] + numbers[j + 1];
                        gr.AddEdge(v1, v2);
                    }
                }

            Console.WriteLine("A1 -> H8 "+ gr.Dijkstra("A1", "H8").Item2);
            */
            /*

            string currentVertex = "A3";
            List<string> path = new List<string>();
            path.Add(currentVertex);

            while (path.Count!=64)
            {
                List<string> neigbours = gr.GetNeigbours(currentVertex, path);
                currentVertex = Min(neigbours, path, gr);
                path.Add(currentVertex);
            }
            foreach (var p in path)
                Console.Write(p + " ");
                */





            /*
            var gr = new Graph();





            gr.AddConnection("A", "B", 3);
            gr.AddConnection("A", "C", 4);
            gr.AddConnection("B", "D", 1);
            gr.AddConnection("B", "E", 3);
            gr.AddConnection("B", "F", 2);
            gr.AddConnection("C", "F", 5);
            gr.AddConnection("F", "Z", 3);
            gr.AddConnection("D", "Z", 4);
            gr.AddConnection("E", "Z", 1);

            Console.WriteLine(gr);
            (List<string>, double) path = gr.Dijkstra("A", "Z").;

            Console.WriteLine("A->Z = " + path.Item2);
            foreach (var v in path.Item1)
                Console.Write(v + " ");


            */




































            //string [] file = { "A B 1", "A C 1", "A D 10", "D E 2", "E G 5" };
            //g.Create(file);
            //g.BFS("A");
            /*
              var g = new Graph();
             string [] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
             string [] numbers = { "1", "2", "3", "4", "5", "6", "7", "8" };


             foreach (var l in letters)
                 foreach (var n in numbers)
                 {
                     g.AddVertex(l+n);
                 }

             for (int i = 0; i < 8; i++)
             {
                 for (int j = 0; j < 8; j++)
                 {
                     string v1 = letters[i] + numbers[j];
                     string v2;
                     if (i + 2 < 8 && j + 1 < 8)
                     {
                         v2 = letters[i + 2] + numbers[j + 1];
                         g.AddEdge(v1, v2);
                     }
                     if (i - 2 >= 0 && j + 1 < 8)
                     {
                         v2 = letters[i - 2] + numbers[j + 1];
                         g.AddEdge(v1, v2);
                     }
                     if (i + 1 < 8 && j + 2 < 8)
                     {
                         v2 = letters[i + 1] + numbers[j + 2];
                         g.AddEdge(v1, v2);
                     }
                     if (i - 1 >= 0 && j + 2 < 8)
                     {
                         v2 = letters[i - 1] + numbers[j + 2];
                         g.AddEdge(v1, v2);
                     }
                 }
             }
             g.Dijkstra("a5","h8");
             Console.WriteLine(g);
             */
        }
    }
}
