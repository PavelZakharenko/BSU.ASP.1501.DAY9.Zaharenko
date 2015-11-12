using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public sealed class BinaryTree<T> : IEnumerable<T>
    {
      
        class Node<T>
        {
            public T Value { get; set; }
            public Node<T> Left { get; set; }
            public Node<T> Right { get; set; }
            public Node(T value)
            {
                Value = value;
            }
        }
        Node<T> root;
        Comparison<T> comparator;
    
        public BinaryTree() :this(Comparer<T>.Default)
        {

        }
        public BinaryTree(IComparer<T> comp)
        {
            if (comp == null)
                throw new ArgumentNullException();
            comparator = comp.Compare;
        }
        public BinaryTree(Comparison<T> comp)
        {
            if (comp == null)
                throw new ArgumentNullException();
            comparator = comp;
        }

        public void Add(T item)
        {
            var node = new Node<T>(item);

            if (root == null)
                root = node;
            else
            {
                Node<T> current = root, parent = null;

                while (current != null)
                {
                    parent = current;
                    if (comparator(item, current.Value) < 0)
                        current = current.Left;
                    else
                        current = current.Right;
                }

                if (comparator(item, parent.Value) < 0)
                    parent.Left = node;
                else
                    parent.Right = node;
            }

        }
        public bool Remove(T item)
        {
            Node<T> current = root, parent = null;

            int result;
            do
            {
                result = comparator(item, current.Value);
                if (result < 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                if (current == null)
                    return false;
            }
            while (result != 0);

            if (current.Right == null)
            {
                if (current == root)
                    root = current.Left;
                else
                {
                    result = comparator(current.Value, parent.Value);
                    if (result < 0)
                        parent.Left = current.Left;
                    else
                        parent.Right = current.Left;
                }
            }
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;
                if (current == root)
                    root = current.Right;
                else
                {
                    result = comparator(current.Value, parent.Value);
                    if (result < 0)
                        parent.Left = current.Right;
                    else
                        parent.Right = current.Right;
                }
            }
            else
            {
                Node<T> min = current.Right.Left, prev = current.Right;
                while (min.Left != null)
                {
                    prev = min;
                    min = min.Left;
                }
                prev.Left = min.Right;
                min.Left = current.Left;
                min.Right = current.Right;

                if (current == root)
                    root = min;
                else
                {
                    result = comparator(current.Value, parent.Value);
                    if (result < 0)
                        parent.Left = min;
                    else
                        parent.Right = min;
                }
            }
            return true;
        }
        public IEnumerable<T> PreOrder()
        {

            if (root == null)
                yield break;
            var stack = new Stack<Node<T>>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current.Value;
                if (current.Right != null)
                    stack.Push(current.Right);
                if (current.Left != null)
                    stack.Push(current.Left);

            }
        }
        public IEnumerable<T> InOrder()
        {
            if (root == null)
                yield break;
            var stack = new Stack<Node<T>>();
            var current = root;
            while (current != null || stack.Count > 0)
            {
                if (current == null)
                {
                    current = stack.Pop();
                    yield return current.Value;
                    current = current.Right;
                }
                else
                {
                    stack.Push(current);
                    current = current.Left;
                }
            }
        }
        public IEnumerable<T> PostOrder()
        {
            if (root == null)
                yield break;
            var stack = new Stack<Node<T>>();
            var current = root;

            while (stack.Count > 0 || current != null)
            {
                if (current == null)
                {
                    current = stack.Pop();
                    if (stack.Count > 0 && current.Right == stack.Peek())
                    {
                        stack.Pop();
                        stack.Push(current);
                        current = current.Right;
                    }
                    else
                    {
                        yield return current.Value;
                        current = null;
                    }
                }
                else
                {
                    if (current.Right != null)
                        stack.Push(current.Right);
                    stack.Push(current);
                    current = current.Left;
                }
            }
        }
        public bool Contains(T key)
        {
            var current = root;
            while (current != null)
            {
                if (comparator(current.Value, key) == 0)
                    return true;
                if (comparator(current.Value, key) < 0)
                    current = current.Left;
                else current = current.Right;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrder().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

