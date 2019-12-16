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
        public int[,] Board { get; set; }
        public readonly Engine Engine;
        private readonly IAppUnitOfWork _appUnitOfWork;

        public Game(IAppUnitOfWork appUnitOfWork)
        {
            _appUnitOfWork = appUnitOfWork;
            Engine = new Engine(_appUnitOfWork.States);
        }

        public BLL.DTO.GameState GameState = new BLL.DTO.GameState();


        public async void OnGet()
        {
            GameState = await Engine.CreateNewGameState();

        }
    


        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await Engine.SaveGameState(GameState);
            return Page();


        }
    }
}