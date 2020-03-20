using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
    public class TreeException : Exception
    {
        public TreeException(string message="Problem with a tree") : base(message) { }
    }
    public class FullTreeException : TreeException
    {
        public FullTreeException(string message="Full Tree Array") 
            : base(message) { }
    }
    public class TryToGetEmptyTree: TreeException
    {
        public TryToGetEmptyTree(string message="The tree is empty"): base(message)
        { }
    }
    public class AttemptOfChangingUnmutableTree :TreeException
    {
        public AttemptOfChangingUnmutableTree(string message = "Attempt of changing unmutable tree"):
            base(message)
        { }
    }
    public class RemovingFromEmptyTree :TreeException
    {
        public RemovingFromEmptyTree(string message = "Removing From Empty Tree") : base(message) { }
    }

}
