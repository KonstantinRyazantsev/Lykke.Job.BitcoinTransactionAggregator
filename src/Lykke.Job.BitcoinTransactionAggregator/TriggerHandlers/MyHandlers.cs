using System.Threading.Tasks;
using Lykke.Job.BitcoinTransactionAggregator.Contract;
using Lykke.Job.BitcoinTransactionAggregator.Core.Services;
using Lykke.JobTriggers.Triggers.Attributes;

namespace Lykke.Job.BitcoinTransactionAggregator.TriggerHandlers
{
    // NOTE: This is the trigger handlers class example
    public class MyHandlers
    {
        private readonly IMyFooService _myFooService;
        private readonly IMyBooService _myBooService;
        private readonly IHealthService _healthService;

        // NOTE: The object is instantiated using DI container, so registered dependencies are injects well
        public MyHandlers(IMyFooService myFooService, IMyBooService myBooService, IHealthService healthService)
        {
            _myFooService = myFooService;
            _myBooService = myBooService;
            _healthService = healthService;
        }

        [TimerTrigger("00:00:10")]
        public async Task TimeTriggeredHandler()
        {
            try
            {
                _healthService.TraceFooStarted();

                await _myFooService.FooAsync();

                _healthService.TraceFooCompleted();
            }
            catch
            {
                _healthService.TraceFooFailed();
            }
        }

        [QueueTrigger("queue-name")]
        public async Task QueueTriggeredHandler(MyMessage msg)
        {
            try
            {
                _healthService.TraceBooStarted();

                await _myBooService.BooAsync();

                _healthService.TraceBooCompleted();
            }
            catch
            {
                _healthService.TraceBooFailed();
            }
        }
    }
}