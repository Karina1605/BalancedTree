using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancedTree
{
    /*Классы исключений, которые могут быть сгенерированы при работе с девером 
     Есть базовый TreeException - Наследник Exception, остальные  - Наследники TreeEcxeption*/

    /// <summary>
    ///Базовый класс для всех исключений дерева 
    /// </summary>
    public class TreeException : Exception
    {
        public TreeException(string message="Problem with a tree") : base(message) { }
    }

    /// <summary>
    ///Исключение, выбрасываемое при попытке добавить в полное дерево (используется только в сплошной реализации) 
    /// </summary>
    public class FullTreeException : TreeException
    {
        public FullTreeException(string message="Full Tree Array") 
            : base(message) { }
    }


    /// <summary>
    ///Исключение, выбрасываемое при попытке перебора пустого дерева (используется в обеих реализациях дерева)
    /// </summary>
    public class TryToGetEmptyTree: TreeException
    {
        public TryToGetEmptyTree(string message="The tree is empty"): base(message)
        { }
    }


    /// <summary>
    ///Исключение, выбрасываемое при любой попытке изменить неизменяемое дерево (используется в обеих реализациях) 
    /// </summary>
    public class AttemptOfChangingUnmutableTree :TreeException
    {
        public AttemptOfChangingUnmutableTree(string message = "Attempt of changing unmutable tree"):
            base(message)
        { }
    }

    /// <summary>
    ///Исключение, выбрасываемое при попытке удаления из пустого дерева (используется в обеих реализациях дерева) 
    /// </summary>
    public class RemovingFromEmptyTree :TreeException
    {
        public RemovingFromEmptyTree(string message = "Removing From Empty Tree") : base(message) { }
    }

}
