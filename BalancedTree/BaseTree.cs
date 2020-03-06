using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
    public abstract class BaseTree<T> : ITree<T> where T: IComparable
    {
        public abstract int Count { get;}
        public abstract bool IsEmpty { get; }
        public abstract IEnumerable<T> nodes { get; }

        public abstract void Add(T node);
        public abstract void Clear();
        public abstract bool Contains(T node);
        public abstract IEnumerator<T> GetEnumerator();
        public abstract void Remove(T node);

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
