using System;
using System.Collections.Generic;
namespace Graph
{
    public class Graph3
    {
        // структура данных, в которой хранится граф
        Dictionary<string, Dictionary<string, double>> Vertexes;

        public Graph3()
        {
            Vertexes = new Dictionary<string, Dictionary<string, double>>();
        }

        public void AddVertex(string V)
        {
            if (!Vertexes.ContainsKey(V))
                Vertexes.Add(V, new Dictionary<string, double>());
        }
        public void AddEdge(string V1, string V2, double weight=1, bool undirected=true)
        {
            if (Vertexes.ContainsKey(V1) && Vertexes.ContainsKey(V2))
            {
                if (!Vertexes[V1].ContainsKey(V2))
                    Vertexes[V1].Add(V2, weight);
                else
                    Vertexes[V1][V2] = weight;
                if (undirected)
                    AddEdge(V2, V1, weight, false);
            }
        }
        public void RemoveEdge(string V1, string V2)
        {
            if (Vertexes.ContainsKey(V1) && Vertexes[V1].ContainsKey(V2))
                Vertexes[V1].Remove(V2);
        }
        public void RemoveVertex(string V)
        { 
            if (Vertexes.ContainsKey(V))
            {
                foreach (var v in Vertexes)
                    if (Vertexes[v.Key].ContainsKey(V))
                        RemoveEdge(v.Key, V);
                Vertexes.Remove(V);
            }
        }
        public double GetWeight(string V1, string V2)
        {
            return (Vertexes.ContainsKey(V1) && Vertexes[V1].ContainsKey(V2))? Vertexes[V1][V2] : Double.PositiveInfinity;
        }
        public string Neighbours(string V)
        {
            string res = string.Empty;
            if (Vertexes.ContainsKey(V))
            {
                foreach (var v in Vertexes[V])
                    res += v.Key + " (" + v.Value + ") ";
            }
            return res;
        }

        public override string ToString()
        {
            string res = string.Empty;
            foreach(var v in Vertexes)
            {
                res += v.Key + ": " + Neighbours(v.Key) + "\n";
            }
            return res;
        }


    }
}
