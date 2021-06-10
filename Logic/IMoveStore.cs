using System.Threading.Tasks;

namespace LetsChess_GameService.Logic
{
	public interface IMoveStore
	{
		Task<bool> Add(string matchId, string from, string to);
	}
}
