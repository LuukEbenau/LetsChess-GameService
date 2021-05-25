using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsChess_GameService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GameController : ControllerBase
	{
		private readonly ILogger<GameController> _logger;

		public GameController(ILogger<GameController> logger)
		{
			_logger = logger;
		}

		[HttpGet("takemove")]
		public IActionResult TakeMove()
		{
			return Ok();
		}
	}
}
