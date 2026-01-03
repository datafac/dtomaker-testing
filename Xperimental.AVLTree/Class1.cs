using System;

namespace Xperimental.AVLTree
{
    public class AVLTree<T> where T : IComparable<T>
    {
        private class Node
        {
            public T Value;
            public Node Left;
            public Node Right;
            public int Height;

            public Node(T value)
            {
                Value = value;
                Height = 1; // New node starts with height 1
            }
        }

        private Node root;

        // Public Remove method
        public void Remove(T value)
        {
            root = Remove(root, value);
        }

        // Recursive remove method
        private Node Remove(Node node, T value)
        {
            if (node == null)
                return null;

            int compare = value.CompareTo(node.Value);

            if (compare < 0)
                node.Left = Remove(node.Left, value);
            else if (compare > 0)
                node.Right = Remove(node.Right, value);
            else
            {
                // Node found - handle deletion cases
                if (node.Left is null)
                {
                    node = node.Right;
                }
                else if (node.Right is null)
                {
                    node = node.Left;
                }
                else
                {
                    // Node with two children: Get inorder successor
                    Node successor = GetMinValueNode(node.Right);
                    node.Value = successor.Value;
                    node.Right = Remove(node.Right, successor.Value);
                }
            }

            if (node == null)
                return null;

            // Update height
            UpdateHeight(node);

            // Rebalance if needed
            return Rebalance(node);
        }

        // Get the node with the smallest value
        private Node GetMinValueNode(Node node)
        {
            Node current = node;
            while (current.Left != null)
                current = current.Left;
            return current;
        }

        // Update node height
        private void UpdateHeight(Node node)
        {
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        // Get height safely
        private int GetHeight(Node node) => node?.Height ?? 0;

        // Get balance factor
        private int GetBalance(Node node) => node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);

        // Rebalance the tree
        private Node Rebalance(Node node)
        {
            int balance = GetBalance(node);

            // Left heavy
            if (balance > 1)
            {
                if (GetBalance(node.Left) < 0)
                    node.Left = RotateLeft(node.Left); // LR case
                return RotateRight(node); // LL case
            }

            // Right heavy
            if (balance < -1)
            {
                if (GetBalance(node.Right) > 0)
                    node.Right = RotateRight(node.Right); // RL case
                return RotateLeft(node); // RR case
            }

            return node; // Already balanced
        }

        // Right rotation
        private Node RotateRight(Node y)
        {
            Node x = y.Left;
            Node T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }

        // Left rotation
        private Node RotateLeft(Node x)
        {
            Node y = x.Right;
            Node T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }

        // Utility: Insert (for testing)
        public void Insert(T value)
        {
            root = Insert(root, value);
        }

        private Node Insert(Node node, T value)
        {
            if (node == null)
                return new Node(value);

            int compare = value.CompareTo(node.Value);
            if (compare < 0)
                node.Left = Insert(node.Left, value);
            else if (compare > 0)
                node.Right = Insert(node.Right, value);
            else
                return node; // Duplicate values not allowed

            UpdateHeight(node);
            return Rebalance(node);
        }

        // Utility: In-order traversal
        public void InOrderTraversal()
        {
            InOrderTraversal(root);
            Console.WriteLine();
        }

        private void InOrderTraversal(Node node)
        {
            if (node == null) return;
            InOrderTraversal(node.Left);
            Console.Write(node.Value + " ");
            InOrderTraversal(node.Right);
        }
    }

    // Example usage
    class Program
    {
        static void Main()
        {
            var tree = new AVLTree<int>();
            tree.Insert(10);
            tree.Insert(20);
            tree.Insert(30);
            tree.Insert(40);
            tree.Insert(50);
            tree.Insert(25);

            Console.WriteLine("In-order before removal:");
            tree.InOrderTraversal();

            tree.Remove(40);

            Console.WriteLine("In-order after removal:");
            tree.InOrderTraversal();
        }
    }
}
