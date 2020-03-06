using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
    public interface ITree<T>: IEnumerable<T>
    {
        void Add(T node);
        void Clear();
        bool Contains(T node);
        void Remove(T node);
        int Count { get; }
        bool IsEmpty { get; }
        IEnumerable<T> nodes { get; }
    }
}
