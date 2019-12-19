using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Game
{
    public class Win : PageModel
    {
        [BindProperty] public string Winner { get; set; }
        
        public void OnGet(string winner)
        {
            Winner = winner ;
        }
    }
}