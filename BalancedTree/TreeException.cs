using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
    class TreeException : Exception
    {
        public TreeException(string message="Problem with a tree") : base(message) { }
    }
    class FullTreeException : TreeException
    {
        public FullTreeException(string message="Full Tree Array") 
            : base(message) { }
    }
    class TryToGetEmptyTree: TreeException
    {
        public TryToGetEmptyTree(string message="The tree is empty"): base(message)
        { }
    }
    class AttemptOfChangingUnmutableTree :TreeException
    {
        public AttemptOfChangingUnmutableTree(string message = "Attempt of changing unmutable tree"):
            base(message)
        { }
    }

}
