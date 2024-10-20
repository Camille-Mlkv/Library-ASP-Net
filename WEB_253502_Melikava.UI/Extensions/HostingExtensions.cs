using WEB_253502_Melikava.UI.HelperClasses;
using WEB_253502_Melikava.UI.Services.API;
using WEB_253502_Melikava.UI.Services.Authentication;
using WEB_253502_Melikava.UI.Services.Authorization;
using WEB_253502_Melikava.UI.Services.BookService;
using WEB_253502_Melikava.UI.Services.GenreService;

namespace WEB_253502_Melikava.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IGenreService, ApiGenreService>();
            builder.Services.AddScoped<IBookService,ApiBookService>();
            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
            builder.Services.AddScoped<IAuthService,KeycloakAuthService>();
        }

    }
}
