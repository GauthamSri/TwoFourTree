using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFourTree
{
    class Node
    {
        public Node parent = null;
        public List<int> Keys;
        public List<Node> children;

        public Node()
        {
            Keys = new List<int>();
            children = new List<Node>();
        }

        public  bool isNodeSizePropertySatisfied()
        {
            return (Keys.Count <= 3) ;
        }

        public bool isRoot()
        {
            return (parent == null);
        }

        public bool isExternal()
        {
            return (children.Count == 0);
        }

        public void insertKey(int value)
        {
            Keys.Add(value);
            Keys = Keys.OrderBy(t => t).ToList();
        }

        public void removeKey(int value)
        {
            Keys.Remove(value);
            //Keys.OrderBy(t => t);
        }

        public void addChild(Node childNode)
        {
            this.children.Add(childNode);
            this.children = this.children.OrderBy(t => t.Keys.First()).ToList();
        }

        public void removeChild(Node childNode)
        {
            this.children.Remove(childNode);
            
        }

       
    }
}
