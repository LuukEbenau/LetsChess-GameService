using LetsChess_GameService.Logic;
using LetsChess_GameService.Messages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsChess_GameService.Controllers
{
	[ApiController]
	[Route("/game")]
	public class GameController : ControllerBase
	{
		private readonly ILogger<GameController> _logger;
		private readonly Game game;

		public GameController(ILogger<GameController> logger,Game game)
		{
			_logger = logger;
			this.game = game;
		}

		[HttpPost("takemove")]
		public IActionResult TakeMove(TakeMoveMessage move)
		{
			game.TakeMove(move.MatchId,move.UserId,move.From,move.To);
			return Ok("move was taken");
		}
	}
}
