using System.Collections;
using System.Collections.Generic;
using System;

namespace CLL
{
    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }
        public Node<T> Next { get; set; }
        public T Data { get; }
    }

    public class CircularLinkedList<T> : IEnumerable<T>
    {
        public Node<T> head { get; private set; }
        private Node<T> tail;
        int count;

        public int Count { get => count; }
        public bool IsEmpty { get => count == 0; }


        public CircularLinkedList(IEnumerable<T> collection)
        {
            foreach (var element in collection)
                Add(element);
        }

        public CircularLinkedList() { }

        public void Add(T data)
        {
            Node<T> newNode = new Node<T>(data);
            if (IsEmpty)
            {
                head = tail = newNode;
                head.Next = head;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
                tail.Next = head;
            }
            count++;
        }

        public bool Remove(T data)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CircularLinkedList is empty");

            Node<T> current = head;
            Node<T> previous = null;
            do
            {
                if (current.Data.Equals(data))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                        if (current == tail)
                            tail = previous;
                    }
                    else
                    {
                        if (count == 1)
                            head = tail = null;
                        else
                        {
                            head = head.Next;
                            tail.Next = head;
                        }
                    }
                    count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }
            while (current != head);
            return false;
        }

        public void Clear()
        {
            head = tail = null;
            count = 0;
        }

        public bool Contains(T data)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CircularLinkedList is empty");
            Node<T> current = head;
            do
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            } while (current != head);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty)
                yield break;

            Node<T> current = head;
            do
            {
                yield return current.Data;
                current = current.Next;
            } while (current != head);
        }

        public IEnumerable<T> InfinityEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

    }

}
