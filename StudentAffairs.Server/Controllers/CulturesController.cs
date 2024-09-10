namespace StudentAffairs.Server.Controllers;

[Route("[controller]/[action]")]
public class CulturesController : Controller
{
    public IActionResult SetCulture(string culture, string redirectUri)
    {
        if (culture is not null)
            HttpContext.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)));

        return LocalRedirect(redirectUri);
    }
}
