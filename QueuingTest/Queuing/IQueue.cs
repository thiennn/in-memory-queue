using System.Threading;
using System.Threading.Tasks;

namespace QueuingTest.Queuing
{
    public interface IQueue<T>
    {
        void Enqueue(T message);

        Task<T> DequeueAsync(CancellationToken cancellationToken);
    }
}
