namespace GameCatalouge.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;


    [Authorize]
    public class DeveloperController : Controller
    {
        public async Task<IActionResult> Become()
        {
            return View();
        }
    }
}
