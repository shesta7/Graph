using System;
using System.Collections.Generic;
namespace Graph
{
    public class Graph2
    {
        Dictionary<string, Dictionary<string,double>> Vertexes;        
        public Graph2()
        {
            Vertexes = new Dictionary<string, Dictionary<string, double>>();
        }


        public void AddVertex(string V)
        {
            // Проверить существует ли вершина с именем V
            if (Vertexes.ContainsKey(V))
                return;
            Vertexes.Add(V, new Dictionary<string, double>());
            // Если ее нет, то добавляем ее в матрицу смежности (расши ряем массив размерности N до N+1)
        }

        public void AddConnection(string V1, string V2, double weight=1, bool undirected =  true)
        {
            AddVertex(V1);
            AddVertex(V2);
            AddEdge(V1, V2, weight, undirected);
        }

        public void AddEdge(string V1,string V2, double weight=1, bool undirected=true)
        { 
            if (Vertexes.ContainsKey(V1) && Vertexes.ContainsKey(V2))
            {
                Vertexes[V1][V2] = weight;
                if (undirected)
                    Vertexes[V2][V1] = weight;
            }
        }

        public void RemoveVertex(string V)
        {
            if (Vertexes.ContainsKey(V))
            {
                foreach (var v in Vertexes)
                {
                    if (Vertexes[v.Key].ContainsKey(V))
                        RemoveEdge(v.Key, V, false);
                }
                Vertexes.Remove(V);
            }
        }

        public void RemoveEdge(string V1, string V2, bool undirected = true)
        {
            if (Vertexes.ContainsKey(V1) && Vertexes.ContainsKey(V2))
            {
                if (Vertexes[V1].ContainsKey(V2))
                    Vertexes[V1].Remove(V2);
                if (undirected && Vertexes[V2].ContainsKey(V1))
                    Vertexes[V2].Remove(V1);
            }
        }

        public Pair<List<string>, double> BFS(string start, string end)
        {
            Pair<List<string>, double> res = new Pair<List<string>, double>(null,Double.PositiveInfinity);
            if (!Vertexes.ContainsKey(start) || !Vertexes.ContainsKey(end))
                return res;
            Dictionary<string, Pair<string,double>> distances = new Dictionary<string, Pair<string,double>>();
            distances.Add(start, new Pair<string, double>(start,0));
            List<string> path = new List<string>();
            Queue<string> q = new Queue<string>();
            q.Enqueue(start);
            while(q.Count>0)
            {
                string currentVertex = q.Dequeue();
                foreach (var v in Vertexes[currentVertex])
                {
                    double dist = distances[currentVertex].Item2 + GetWeight(currentVertex, v.Key);
                    if (!distances.ContainsKey(v.Key))
                    {
                        distances.Add(v.Key, new Pair<string,double>(currentVertex, dist));
                        q.Enqueue(v.Key);
                    }
                    else if (dist < distances[v.Key].Item2)
                    {
                        distances[v.Key].Item1 = currentVertex;
                        distances[v.Key].Item2 = dist;
                        q.Enqueue(v.Key);
                    }
                }
            }

            if (!distances.ContainsKey(end))
                return res;
            string currentV = end;
            path.Add(end);
            while (currentV !=start)
            {
                path.Add(distances[currentV].Item1);
                currentV = distances[currentV].Item1;
            }
            path.Reverse();
            //path??????

            res.Item1 = path;
            res.Item2 = distances[end].Item2;
            return res;
        }

        public double GetWeight(string V1, string V2)
        {
            return (Vertexes.ContainsKey(V1) && Vertexes[V1].ContainsKey(V2))? Vertexes[V1][V2]: Double.PositiveInfinity;
        }

        public string Neigbours(string V)
        {
            string res = string.Empty;
            if (Vertexes.ContainsKey(V))
            {
                foreach (var v in Vertexes[V])
                {
                    res += "("+ v.Key +" "+ v.Value+")";
                }
            }
            return res;
        }

        public override string ToString()
        {
            string res = string.Empty;
            foreach (var v in Vertexes)
            {
                res += v.Key+" "+ Neigbours(v.Key) +"\n";
            }
            return res;
        }
        // A B 10
        public void Create(string[] str)
        {
            foreach (var s in str)
            {
                string[] res = s.Split(' ');
                AddConnection(res[0], res[1], Double.Parse(res[2]));
            }

        }
    }
}
