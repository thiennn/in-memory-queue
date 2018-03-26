using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QueuingTest.Queuing;

namespace QueuingTest
{
    public class TestDequeuer : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IQueue<string> _queue;
        private readonly CancellationTokenSource _stopTokenSource = new CancellationTokenSource();
        private Task _dequeueMessagesTask;

        public TestDequeuer(ILogger<TestDequeuer> logger, IQueue<string> queue)
        {
            _logger = logger;
            _queue = queue;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("starting dequeuer");
            _dequeueMessagesTask = Task.Run(DequeueMessagesAsync);

            _logger.LogInformation("dequeuer return");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _stopTokenSource.Cancel();
            return Task.WhenAny(_dequeueMessagesTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        private async Task DequeueMessagesAsync()
        {
            while (!_stopTokenSource.IsCancellationRequested)
            {
                _logger.LogInformation("calling dequeue");
                string message = await _queue.DequeueAsync(_stopTokenSource.Token);

                if (!_stopTokenSource.IsCancellationRequested)
                {
                    await Task.Delay(10000);
                    _logger.LogInformation($"from back ground: {message}");
                }
            }
        }
    }
}
