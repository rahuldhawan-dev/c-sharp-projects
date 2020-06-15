using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CachingProofOfConcept.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountriesController : ControllerBase
	{
		private readonly ICountryRepository _repository;

		public CountriesController(ICountryRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Country>>> Get()
		{
			return (await _repository.Get()).ToList();
		}
	}
}
