using System;
using System.Threading;

namespace ServerCore
{
    class Program
    {
        static int number = 0;

        // Critical Section의 Key로 사용될 객체
        static object _obj = new object();

        static void Thread_1()
        {
            for (int i = 0; i < 10; i++) 
            {
                // case 1. Interlocked
                // 단일 변수에 대해서 Lock을 구현하는 방식.
                // Interlocked.Increment(ref number);

                // case 2. Critical Section 1
                // 임계 영역을 만들어 Lock을 구현하는 방식
                /*
                try 
                {
                    Monitor.Enter(_obj);
                    number++;

                    // finally가 없다면 Monitor.Exit()가 실행되지 않는 문제 발생
                    return;
                }
                finally 
                {
                    Monitor.Exit(_obj); 
                }
                */

                // case 3. Critical Section 2
                lock (_obj) 
                {
                    number++;
                }
            }
        }

        static void Main(string[] args) 
        {

        }
    }
}