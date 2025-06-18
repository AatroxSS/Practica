using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Practica
{
    [Serializable]
    public class DoublyLinkedList<T>
    {
        [JsonPropertyName("Head")]
        private DoublyNode<T> head;

        [JsonPropertyName("Tail")]
        private DoublyNode<T> tail;

        [JsonPropertyName("Count")]
        private int count;

        public T GetByIndex(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();

            DoublyNode<T> current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            return current.Data;
        }

        public int Count => count;
        
        public void AddToEnd(T data)
        {
            DoublyNode<T> node = new DoublyNode<T>(data);

            if (head == null)
            {
                head = node;
            }
            else
            {
                tail.Next = node;
                node.Previous = tail;
            }
            tail = node;
            count++;
        }

        public T RemoveFromStart()
        {
            if (count == 0)
                throw new InvalidOperationException("Список порожній");
            T output = head.Data;
            if (count == 1)
            {
                head = tail = null;
            }
            count--;
            return output;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();

                DoublyNode<T> current = head;
                for (int i = 0; i < index; i++)
                {
                    current = current.Next;
                }
                return current.Data;
            }
            set
            {
                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException();

                DoublyNode<T> current = head;
                for (int i = 0; i < index; i++)
                {
                    current = current.Next;
                }
                current.Data = value;
            }
        }

        public T GetLast()
        {
            if (tail == null)
                throw new InvalidOperationException("Список порожній");
            return tail.Data;
        }

        public T GetPrevious(T current)
        {
            DoublyNode<T> node = FindNode(current);
            if (node?.Previous == null)
                throw new InvalidOperationException("Попереднього елемента не існує");
            return node.Previous.Data;
        }

        private DoublyNode<T> FindNode(T data)
        {
            DoublyNode<T> current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return current;
                        current = current.Next;
            }
            return null;
        }

        public void Sort(Func<T, T, int> comparer)
        {
            head = MergeSort(head, comparer);
            tail = head;
            if (head != null)
            {
                while (tail.Next != null)
                {
                    tail.Next.Previous = tail;
                    tail = tail.Next;
                }
            }
        }

        private DoublyNode<T> MergeSort(DoublyNode<T> start, Func<T, T, int> comparer)
        {
            if (start == null || start.Next == null)
                return start;

            DoublyNode<T> middle = GetMiddle(start);
            DoublyNode<T> nextToMiddle = middle.Next;
            middle.Next = null;

            DoublyNode<T> left = MergeSort(start, comparer);
            DoublyNode<T> right = MergeSort(nextToMiddle, comparer);

            return Merge(left, right, comparer);
        }

        private DoublyNode<T> Merge(DoublyNode<T> left, DoublyNode<T> right, Func<T, T, int> comparer)
        {
            if (left == null) return right;
            if (right == null) return left;

            if (comparer(left.Data, right.Data) <= 0)
            {
                left.Next = Merge(left.Next, right, comparer);
                left.Next.Previous = left;
                left.Previous = null;
                return left;
            }
            else
            {
                right.Next = Merge(left, right.Next,comparer);
                right.Next.Previous = right;
                right.Previous = null;
                return right;
            }
        }

        private DoublyNode<T> GetMiddle(DoublyNode<T> start)
        {
            if (start == null)
                return start;

            DoublyNode<T> slow = start;
            DoublyNode<T> fast = start;

            while (fast.Next != null && fast.Next.Next != null)
            {
                slow = slow.Next;
                fast = fast.Next.Next;
            }

            return slow;
        }

        public IEnumerable<T> FindAll(Predicate<T> predicate)
        {
            DoublyNode<T> current = head;
            while (current != null)
            {
                if (predicate(current.Data))
                    yield return current.Data;
                current = current.Next;
            }
        }

        public void Printable(string header)
        {
            Console.WriteLine(header);
            Console.WriteLine(new string('-',40));
            Console.WriteLine($"{"Жанр",-15} {"Рік",-10} {"Доступність,-10"}");
            Console.WriteLine(new string('-',40));

            DoublyNode<T> current = head;
            while (current != null)
            {
                Console.WriteLine(current.Data.ToString());
                current = current.Next;
            }

            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"Всього книг: {count}");
            Console.WriteLine();
        }

        public void Serialize(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                IncludeFields = true 
            };
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, json);
        }

        public static DoublyLinkedList<T> Deserialize(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                IncludeFields = true
            };
            return JsonSerializer.Deserialize<DoublyLinkedList<T>>(json, options);
        }


    }
}
