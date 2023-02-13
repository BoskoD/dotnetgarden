using System.Collections.Generic;

namespace GenericLinkedList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GenericLinkedList<string> linkedList = new();
            linkedList.AddLast("Hello");
            linkedList.AddLast("From");
            linkedList.AddLast("Generic");
            linkedList.AddLast("LinkedList");

            Console.WriteLine("Current list:");
            PrintList(linkedList);

            linkedList.Remove(linkedList.Head.Next);
            PrintList(linkedList);
        }

        private static void PrintList<T>(GenericLinkedList<T> linkedList)
        {
            GenericLinkedListNode<T> current = linkedList.Head;
            while (current != null)
            {
                Console.Write(current.Value + " ");
                current = current.Next;
            }
            Console.WriteLine();
        }
    }

    public class GenericLinkedListNode<T>
    {
        public T Value { get; set; }
        public GenericLinkedListNode<T>? Next { get; set; }

        public GenericLinkedListNode(T value)
        {
            Value = value;
            Next = null;
        }
    }
    public class GenericLinkedList<T>
    {
        public GenericLinkedListNode<T> Head { get; private set; }
        public GenericLinkedListNode<T> Tail { get; private set; }

        public void AddLast(T value)
        {
            GenericLinkedListNode<T> newNode = new(value);

            if (Tail == null)
            {
                Head = newNode;
                Tail = newNode;
            }
            else
            {
                Tail.Next = newNode;
                Tail = newNode;
            }
        }

        public void Remove(GenericLinkedListNode<T> node)
        {
            if (Head == node)
            {
                Head = Head.Next;
                node.Next = null;
                return;
            }

            GenericLinkedListNode<T> current = Head;
            while (current != null)
            {
                if (current.Next == node)
                {
                    current.Next = node.Next;
                    node.Next = null;
                    if (Tail == node)
                    {
                        Tail = current;
                    }
                    return;
                }
                current = current.Next;
            }
        }
    }
}