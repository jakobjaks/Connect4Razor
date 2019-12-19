using System.Collections.Generic;
using System.Threading.Tasks;
using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class Game : PageModel
    {
        private readonly IEngine _engine;

        public Game(IEngine engine)
        {
            _engine = engine;
        }

        [BindProperty] public BLL.DTO.GameState GameState { get; set; }


        public async Task OnGet(int? gameId, int? col)
        {
            if (gameId == null && col == null)
            {
                GameState = await _engine.CreateGameStateFromSettings();
            }
            else if (gameId != null && col == null)
            {
                GameState = _engine.GetSavedState(gameId.Value);
            }
            else if (gameId != null)
            {

                GameState = await _engine.UpdateGameState(gameId.Value, col.Value);

                if (GameState.Winner != BLL.DTO.GameState.Win.NO_WINNER)
                {
                    RedirectToPage("/Game/StartGame");
                }
            }
        }

        public async Task<RedirectToPageResult> OnPost(int gameId, int col)
        {
            GameState.StateId = gameId;
            await _engine.SaveGameStateWithName(GameState);
            return RedirectToPage("./StartGame");
        }
    }
}