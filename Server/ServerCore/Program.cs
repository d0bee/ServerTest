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
        static object _lock = new object();
        static SpinLock _lock2 = new SpinLock();
        static ReaderWriterLockSlim _lock3 = new ReaderWriterLockSlim();
        
        // 99%
        static Reward GetRewardById(int id)
        {
            _lock3.EnterReadLock();

            _lock3.ExitReadLock();

            return null;
        }
        
        // 1%
        static void AddReward(Reward reward) 
        {
            _lock3.EnterWriteLock();

            _lock3.ExitWriteLock();
        }
        
        class Reward
        {
        }

        static void Main(string[] args) 
        {
            lock (_lock)
            { 

            }
        }
    }
}