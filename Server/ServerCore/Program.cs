using System;
using System.Threading;

// SpinLock을 구현한 방식에 대해서 말해보세요.
// 1. Interlocked.Exchange를 사용하여 오리지널 반환값을 확인하는 방식으로 구현하였다.
// Exchange를 한 상태에서 오리지널 반환값이 진짜 오리지널 값이라면, 즉, Lock이 false라면 제대로 된 Lock을 획득한 것을 입증할 수 있다.
// 2. Interlocked.CompareExchange를 사용하여 구현하였다.
// Interlocked.Exchange와 같이 반환값을 확인하여 교체 시 _locked 값이 false인지를 확인, 제대로 된 Lock을 획득하여 SpinLock을 구현하였다.
namespace ServerCore
{
    class SpinLock
    {
        volatile int _locked = 0;

        // 잠금이 풀리길 기다리는 상태와 lock을 거는 것이 원자성으로 일어나야한다.
        public void Acquire()
        { 
            while (true) 
            {
                // Exchange, _locked를 1로 교체한다.
                // 그리고 원본 값을 반환한다.
                int original = Interlocked.Exchange(ref _locked, 1);
                if (original == 0)
                    break;

                // _locked의 original 값이 false인지를 확인 후 1로 교체, 반환 값은 original
                if (Interlocked.CompareExchange(ref _locked, 1, 0) == 0)
                    break;
            }
        }

        // 권한 해제
        public void Release() 
        {
            _locked = 0;
        }
    }

    class Program
    {
        static int num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for (int i = 0; i < 1000000; i++)
            {
                _lock.Acquire();
                num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; i++)
            {
                _lock.Acquire();
                num--;
                _lock.Release();
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