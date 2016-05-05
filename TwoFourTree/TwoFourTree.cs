using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFourTree
{
    class TwoFourTree
    {
        Node root;

        public TwoFourTree()
        {
            root = new Node();
        }

        public int InsertValue(int value)
        {
            int splitCount = 0;
            InsertIntoNode(root, value, ref splitCount);
            return splitCount;
        }

        private void InsertIntoNode(Node node, int value,ref int splitCount)
        {
            if (node.children.Count == 0)
            {
                node.insertKey(value);
                // do splitting if key > 3
                if (!node.isNodeSizePropertySatisfied())
                {
                    splitCount++;
                    SplitNode(node);

                }

            }
            else
            {

                int i = 0;
                foreach (var t in node.Keys)
                {
                    if (value < t)
                        break;
                    i++;
                }
                InsertIntoNode(node.children[i], value, ref splitCount);
            }
        }

        private void SplitNode(Node node)
        {
            Node childNode = new Node();

            childNode.insertKey(node.Keys[0]);
            childNode.insertKey(node.Keys[1]);
            node.removeKey(node.Keys[0]);
            node.removeKey(node.Keys[0]);


            if (node.isRoot())
            {
                Node newRoot = new Node();
                newRoot.insertKey(node.Keys[0]);
                node.removeKey(node.Keys[0]);
                childNode.parent = newRoot;
                node.parent = newRoot;
                newRoot.addChild(childNode);
                newRoot.addChild(node);
                root = newRoot;
               
            }
            else
            {
                childNode.parent = node.parent;
                node.parent.insertKey(node.Keys[0]);
                node.removeKey(node.Keys[0]);
                node.parent.addChild(childNode);

                if (!node.parent.isNodeSizePropertySatisfied())
                {
                    SplitNode(node.parent);
                }
            }
            if (node.children.Count != 0)
            {
                childNode.addChild(node.children[0]);
                childNode.children[0].parent = childNode;
                childNode.addChild(node.children[1]);
                childNode.children[1].parent = childNode;
                childNode.addChild(node.children[2]);
                childNode.children[2].parent = childNode;
                node.removeChild(node.children[0]);
                node.removeChild(node.children[0]);
                node.removeChild(node.children[0]);
            }
        }

        public void searchTree(int value)
        {

            if (searchNode(root, value))
                Console.WriteLine("{0} Found!",value);
            else
                Console.WriteLine("{0} Not Found!",value);
        }

        private bool searchNode(Node node, int value)
        {
            bool result = false;

            int i = 0;

            if (node.Keys.Contains(value))
            {
                result =  true;
            }
            else
            {
                foreach (var t in node.Keys)
                {
                    if (value < t)
                        break;
                    i++;
                }

                if(i < node.children.Count)
                    result = searchNode(node.children[i], value);
            }

            return result;
        }

        public Node searchValueAndGetNode(int value)
        {
            return getNode(root, value);
        }

        private Node getNode(Node node, int value)
        {
            Node nodeToBeReturned = null;
            int i = 0;

            if (node.Keys.Contains(value))
            {
                return node;
            }
            else
            {
                foreach (var t in node.Keys)
                {
                    if (value < t)
                        break;
                    i++;
                }

                if( i < node.children.Count)
                    nodeToBeReturned = getNode(node.children[i], value);
            }

            return nodeToBeReturned;
        }

        public int deleteKey(int value)
        {
            int fusionCount = 0;

            Node targetNode = searchValueAndGetNode(value);

            if (targetNode == null)
            {
                Console.WriteLine("No Element found");
                return 0;
            }

            if (targetNode.isExternal())
            {
                targetNode.removeKey(value);
                if (targetNode.Keys.Count == 0)
                    FixUnderflow(targetNode, ref fusionCount);
            }
            else
            {
                int valueSuccessor = 0;
                var targetIndex = targetNode.Keys.FindIndex(t => t == value);
                Node sibilingNode = null;


                sibilingNode = FindReplacement(targetNode.children[targetIndex + 1]);
                valueSuccessor = sibilingNode.Keys[0];

                targetNode.Keys[targetIndex] = valueSuccessor;
                sibilingNode.removeKey(valueSuccessor);



                if (sibilingNode != null && sibilingNode.Keys.Count == 0)
                    FixUnderflow(sibilingNode, ref fusionCount);
            }

            return fusionCount;
        }

        private Node FindReplacement(Node inputNode)
        {
            Node nodeToBeReplaced = null;

            if (inputNode.isExternal())
                return inputNode;
            else
            {
                nodeToBeReplaced = FindReplacement(inputNode.children[0]);
            }

            return nodeToBeReplaced;
        }



        private void FixUnderflow(Node node, ref int fusionCount)
        {
            fusionCount++;

            if (node.isRoot())
            {
                root = node.children[0];
                node.children = null;
                root.parent = null;
            }
            else
            {
                var nodeIndexInChildren = node.parent.children.FindIndex(t => t.Keys.Count == 0);

                if (nodeIndexInChildren <= 0)
                {
                    Node nextSibiling = node.parent.children[nodeIndexInChildren + 1];

                    if (nextSibiling.Keys.Count == 1)
                    {
                        node.parent.removeChild(nextSibiling);
                        var nextSibilingChildren = nextSibiling.children.ToList();

                        if (!nextSibiling.isExternal())
                        {
                            foreach (var child in nextSibilingChildren)
                            {
                                node.addChild(child);
                                child.parent = node;
                                nextSibiling.removeChild(child);
                            }
                        }
                        nextSibiling.parent = null;
                        var keyToBeMoved = node.parent.Keys.First();
                        node.insertKey(keyToBeMoved);
                        node.insertKey(nextSibiling.Keys.First());
                        nextSibiling.Keys.Clear();
                        node.parent.removeKey(keyToBeMoved);
                    }
                    else
                    {
                        var keyToBeMovedToChild = nextSibiling.parent.Keys.First();
                        node.insertKey(keyToBeMovedToChild);
                        nextSibiling.parent.removeKey(keyToBeMovedToChild);
                        var keyToBeMovedToParent = nextSibiling.Keys.First();
                        nextSibiling.parent.insertKey(keyToBeMovedToParent);
                        nextSibiling.removeKey(keyToBeMovedToParent);

                        if (!nextSibiling.isExternal())
                        {
                            var childToBeMoved = nextSibiling.children.Last();
                            node.addChild(childToBeMoved);
                            nextSibiling.removeChild(childToBeMoved);
                            childToBeMoved.parent = node;
                        }
                    }
                }
                else
                {
                    Node prevChild = node.parent.children[nodeIndexInChildren - 1];

                    if (prevChild.Keys.Count == 1)
                    {
                        node.insertKey(prevChild.Keys.First());
                        prevChild.removeKey(prevChild.Keys.First());
                        node.parent.removeChild(prevChild);
                        var prevNodeChildren = prevChild.children.ToList();
                        if (!prevChild.isExternal())
                        {
                            foreach (var child in prevNodeChildren)
                            {
                                node.addChild(child);
                                child.parent = node;
                                prevChild.removeChild(child);
                            }
                        }
                        prevChild.parent = null;

                        int keyToBeMoved = node.parent.Keys[nodeIndexInChildren - 1];

                        node.insertKey(keyToBeMoved);
                        node.parent.removeKey(keyToBeMoved);
                    }
                    else
                    {
                        int keyToBeMoved = node.parent.Keys[nodeIndexInChildren - 1];
                        node.insertKey(keyToBeMoved);
                        node.parent.removeKey(keyToBeMoved);
                        int keyToBeMovedToParent = prevChild.Keys.Last();

                        prevChild.parent.insertKey(keyToBeMovedToParent);
                        prevChild.removeKey(keyToBeMovedToParent);

                        if (!prevChild.isExternal())
                        {
                        var childToBeMoved = prevChild.children.Last();
                        node.addChild(childToBeMoved);
                        prevChild.removeChild(childToBeMoved);
                        childToBeMoved.parent = node;
                        }
                    }
                }

                if (node.parent.Keys.Count == 0)
                    FixUnderflow(node.parent, ref fusionCount);

            }

        }

        public void InOrderTraversal()
        {
            printInOrder(root);
        }

        private void printInOrder(Node node)
        {
            if (node.Keys.Count == 0)
                return;

            int i = 0;
            foreach (var k in node.Keys)
            {
                if (!node.isExternal() && i < node.children.Count)
                {
                        printInOrder(node.children[i]);
                }
                Console.Write(k + " ");
                i++;
            }
            if (!node.isExternal() && i < node.children.Count)
            {
                printInOrder(node.children[i]);
            }

        }

    }
}
