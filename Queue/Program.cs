using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ThreadSafeQueue
{
    public enum Operation
    {
        PUSH = 1,
        POP,
        PEEK
    }
    public class Queue
    {
        private int front;
        private int rear;
        private List<object> Q;

        //Lock object to synchronize the multiple threads....
        //The thread which aquire the lock will execute and other thread will wait untill current thread completes its tasks
        private static readonly object lockObject = new object();

        /// <summary>
        /// Constructor - Initialize Queue.
        /// </summary>
        public Queue()
        {
            front = rear = -1;
            Q = new List<object>();
        }

        /// <summary>
        /// Push Operation Handler
        /// </summary>
        /// <param name="item"></param>
        public void Push(object item)
        {
            try
            {
                lock (lockObject)
                {
                    if (front == rear && front == -1)
                    {
                        front = rear = 0;
                        Q.Add(item);
                        PrintOperation(Operation.PUSH, item);
                    }
                    else
                    {
                        rear++;
                        Q.Add(item);
                        PrintOperation(Operation.PUSH, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       

        /// <summary>
        /// Pop operation Handler...
        /// </summary>
        /// <returns></returns>
        public object Pop()
        {
            object item = new object();
            try
            {
                lock (lockObject)
                {
                    if (!QueueIsEmpty())
                    {
                        item = Q[front];
                        //Q.Remove(item);
                        front++;
                        PrintOperation(Operation.POP, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;
        }

        /// <summary>
        /// Peek Operation Handler...
        /// </summary>
        /// <returns></returns>
        public object Peek()
        {
            object item = new object();
            try
            {
                lock (lockObject)
                {
                    if (!QueueIsEmpty())
                    {
                        item = Q[rear];
                        PrintOperation(Operation.PEEK, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return item;
        }


        // Private method no need to expose to the clients...
        /// <summary>
        /// Checks wheather Queue is empty or not...
        /// </summary>
        /// <returns></returns>
        private bool QueueIsEmpty()
        {
            if (front == -1 || front > rear)
                 return true;
            
            return false;
        }

        public void PrintQueue()
        {
            for (int i = front; i <= rear; i++)
                Console.WriteLine(Q[i]);
        }

        private void PrintOperation(Operation operation, object item)
        {
            Thread t = Thread.CurrentThread;
            Console.Write(string.Format("{0}-{1}-", "ThreadID", t.ManagedThreadId));
            switch (operation)
            {
                case Operation.PUSH: Console.WriteLine(string.Format("{0}-{1}", "PUSH", item)); break;
                case Operation.POP: Console.WriteLine(string.Format("{0}-{1}", "POP", item)); break;
                case Operation.PEEK: Console.WriteLine(string.Format("{0}-{1}", "PEEK", item)); break;
                default:
                    break;
            }
        }
    }
    
    /// <summary>
    /// Main Class or Client Application...
    /// </summary>
    public class Program
    {
        
        public static Queue q = new Queue(); //Single queue object will be invoked by multiple threads...
        
        //Main Methods...
        public static void Main(string[] args)
        {
            Console.WriteLine("Press any key to print Queue data.. ");
            
            // Two threads t1 & t2 will run concurrently...
            Thread t1 = new Thread(new ThreadStart(SampleInput1));
            t1.Start();

            Thread t2 = new Thread(new ThreadStart(SampleInput2));
            t2.Start();

            Console.ReadKey();
            Console.WriteLine("Queue Data: ");
            q.PrintQueue();
            Console.ReadKey();//Hold Screen...
        }

        // Sample Input1 method...
        private static void SampleInput2()
        {
            object item = new object();

            
            q.Push(10); //PUSH
            q.Push("xyz"); //PUSH 

            item = q.Peek(); //PEEK
            
            q.Push(12.5); //PUSH
            q.Push(321321321); //PUSH
            
            item = q.Pop(); //POP
            item = q.Pop(); //POP
        }

        //Sample Input2 method...
        private static void SampleInput1()
        {
            object item = new object();

            //Queue q = new Queue();
            q.Push(20); //PUSH
            q.Push("abc"); //PUSH
            q.Push(65.125); //PUSH

            item = q.Peek(); //PEEK
            
            q.Push(987522);  //PUSH
            
            item = q.Pop(); //POP
            item = q.Pop(); //POP

            //Console.WriteLine("Popped item: " + item.ToString());
        }

       
    }
   
}
