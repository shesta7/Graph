using System;
using System.Collections.Generic;

namespace Graph
{
    public class Graph
    {
        // структура данных, которая хранит в себе граф (ребра и вершины)
        Dictionary<string, Dictionary<string, double>> Vertexes;
        public Graph()
        {
            Vertexes = new Dictionary<string, Dictionary<string, double>>();
        }

        public (List<string>,double) Dijkstra(string start, string end)
        {
            if (!Vertexes.ContainsKey(start) || !Vertexes.ContainsKey(end))
                return (null, double.PositiveInfinity);
            Dictionary<string, (string, double)> distances = new Dictionary<string, (string, double)>();
            distances.Add(start, (start,0));
            List<string> path = new List<string>();

            Queue<string> q = new Queue<string>();
            q.Enqueue(start);
            while (q.Count>0)
            {
                string currentVertex=q.Dequeue();
                foreach (var v in Vertexes[currentVertex])
                {
                    double dist = distances[currentVertex].Item2 + GetWeight(currentVertex, v.Key);
                    if (!distances.ContainsKey(v.Key))
                    {
                        distances.Add(v.Key, (currentVertex, dist));
                        q.Enqueue(v.Key);
                    }
                    else if (dist < distances[v.Key].Item2)
                    {
                        distances[v.Key] = (currentVertex, dist);
                        q.Enqueue(v.Key); 
                    }
                }
            }
            /*
            foreach (var d in distances)
                Console.WriteLine(start + "->" + d.Key + " (" + d.Value.Item2 + " " + d.Value.Item1 + ")");
                */

            if (!distances.ContainsKey(end))
                return (null, double.PositiveInfinity);

            string currentV = end;
            path.Add(currentV);
            while (currentV != start)
            {
                currentV = distances[currentV].Item1;
                path.Add(currentV);
            }
            path.Reverse();

            return (path,distances[end].Item2);
        }

        public void AddConnection(string V1, string V2, double weight = 1, bool undirected = true)
        {
            AddVertex(V1);
            AddVertex(V2);
            AddEdge(V1, V2, weight, undirected);
        }

        public void AddVertex(string V)
        {
            if (!Vertexes.ContainsKey(V))
                Vertexes.Add(V, new Dictionary<string, double>());
        }

        public void AddEdge(string V1, string V2, double weight=1, bool undirected=true)
        {
            if (Vertexes.ContainsKey(V1) && Vertexes.ContainsKey(V2) && !(Vertexes[V1].ContainsKey(V2)))
            {
                Vertexes[V1].Add(V2, weight);
                if (undirected)
                    Vertexes[V2].Add(V1, weight);
            }
        }

        public void RemoveEdge(string V1, string V2) 
        {
            if (Vertexes.ContainsKey(V1) && Vertexes[V1].ContainsKey(V2))
                Vertexes[V1].Remove(V2);
        }

        public void RemoveVertex(string V)
        {
            if(Vertexes.ContainsKey(V))
            {
                Vertexes.Remove(V);
                foreach (var v in Vertexes)
                    if (Vertexes[v.Key].ContainsKey(V))
                        Vertexes[v.Key].Remove(V);
            }
        }
        public Dictionary<string, Dictionary<string, double>>  GetGraph()
        {
            return Vertexes;
        }

        public double GetWeight(string V1, string V2)
        {
            return (Vertexes.ContainsKey(V1) && Vertexes[V1].ContainsKey(V2)) ? Vertexes[V1][V2] : Double.PositiveInfinity;
        }

        public List<string> GetNeigbours(string V, List<string> except = null)
        {
            var res = new List<string>();
            if (Vertexes.ContainsKey(V))
            {
                foreach (var v in Vertexes[V])
                { 
                    if(except !=null && !(except.Contains(v.Key)))
                    res.Add(v.Key);
                }
            }
            return res;
        }


        public string Neigbours(string V)
        {
            string res = string.Empty;
            if(Vertexes.ContainsKey(V))
               foreach (var v in Vertexes[V])
                    res += v.Key + " (" + v.Value + ") ";
            return res;
        }


        public override string ToString()
        {
            string res = string.Empty;
            foreach (var v in Vertexes)
                res += v.Key + ": " + Neigbours(v.Key) + "\n";
            return res;
        }

    }
}