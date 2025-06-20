using System.Collections.Concurrent;
using System.Threading;

namespace Philosophers.Server
{
    public class DiningPhilosophers
    {
        private readonly SemaphoreSlim[] _forks;
        private readonly ConcurrentDictionary<int, string> _philosopherStates;

        public DiningPhilosophers(int philosopherCount)
        {
            _forks = new SemaphoreSlim[philosopherCount];
            _philosopherStates = new ConcurrentDictionary<int, string>();

            for (int i = 0; i < philosopherCount; i++)
            {
                _forks[i] = new SemaphoreSlim(1, 1);
                _philosopherStates[i] = "Thinking";
            }
        }

        public async Task StartDiningAsync(int philosopherId)
        {
            _philosopherStates[philosopherId] = "Hungry";

            var leftFork = _forks[philosopherId];
            var rightFork = _forks[(philosopherId + 1) % _forks.Length];

            await Task.WhenAll(leftFork.WaitAsync(), rightFork.WaitAsync());

            _philosopherStates[philosopherId] = "Eating";
            await Task.Delay(10000); // Simulate eating time

            leftFork.Release();
            rightFork.Release();

            _philosopherStates[philosopherId] = "Thinking";
        }

        public Dictionary<int, string> GetStates()
        {
            return _philosopherStates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
