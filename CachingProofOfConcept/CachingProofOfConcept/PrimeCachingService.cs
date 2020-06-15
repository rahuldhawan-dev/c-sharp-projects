using System.Threading;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.Extensions.Hosting;

namespace CachingProofOfConcept
{
	public class PrimeCachingService : BackgroundService
	{
		private readonly ICountryRepository _repository;

		public PrimeCachingService(ICountryRepository repository)
		{
			_repository = repository;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
            var task = Task.Run(() => _repository.PrimeCache(), stoppingToken);
            return task;
		}
	}
}