using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QueuingTest.Queuing
{
    public class InMemoryQueue<T> : IQueue<T>
    {
        private readonly ConcurrentQueue<T> _messages = new ConcurrentQueue<T>();
        private readonly SemaphoreSlim _messageEnqueuedSignal = new SemaphoreSlim(0);

        public async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            await _messageEnqueuedSignal.WaitAsync(cancellationToken);

            _messages.TryDequeue(out T message);
            return message;
        }

        public void Enqueue(T message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            _messages.Enqueue(message);
            _messageEnqueuedSignal.Release();
        }
    }
}
