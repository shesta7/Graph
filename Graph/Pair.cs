using System;
using System.Collections.Generic;
namespace Graph
{
    // Pair<List<string>,double> p = new Pair<List<string>, double>(new List<string>(), 342);

    public class Pair<U,T>
    {
        public U Item1;
        public T Item2;
        public Pair()
        {

        }
        public Pair(U Item1, T Item2)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
        }

        public override string ToString()
        {
            return Item1.ToString() + " "+ Item2.ToString();
        }
    }
}
