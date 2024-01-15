using System;
using System.Threading;

// SpinLock을 구현한 방식에 대해서 말해보세요.
// 1. Interlocked.Exchange를 사용하여 오리지널 반환값을 확인하는 방식으로 구현하였다.
// Exchange를 한 상태에서 오리지널 반환값이 진짜 오리지널 값이라면, 즉, Lock이 false라면 제대로 된 Lock을 획득한 것을 입증할 수 있다.
// 2. Interlocked.CompareExchange를 사용하여 구현하였다.
// Interlocked.Exchange와 같이 반환값을 확인하여 교체 시 _locked 값이 false인지를 확인, 제대로 된 Lock을 획득하여 SpinLock을 구현하였다.
namespace ServerCore
{
    
    class Program
    {
        // Mutex.WaitOne(), Mutex.ReleaseMutex() 등을 사용하여 임계 구역을 만들 수 있다. 이 또한 커널에 접근하기 때문에 부하가 큰 방식이다.

        static int num = 0;
        static Mutex _lock = new Mutex();

        static void Thread_1()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.WaitOne();
                num++;
                _lock.ReleaseMutex();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 10000; i++)
            {
                _lock.WaitOne();
                num--;
                _lock.ReleaseMutex();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(num);
        }
    }
}