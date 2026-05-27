# Theming Example

```cs
// 1. Theme Service
public interface IThemeService
{
    string GetCurrentTheme();
}

public class ThemeService : IThemeService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ThemeService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentTheme()
    {
        var context = _httpContextAccessor.HttpContext;

        // Cookie'den oku (kullanıcı tercihi)
        if (context.Request.Cookies.TryGetValue("theme", out var cookieTheme))
            return cookieTheme;

        // Session'dan oku
        var sessionTheme = context.Session.GetString("theme");
        if (!string.IsNullOrEmpty(sessionTheme))
            return sessionTheme;

        // Varsayılan tema
        return "Light";
    }
}

// 2. ThemeViewLocationExpander
using Microsoft.AspNetCore.Mvc.Razor;

public class ThemeViewLocationExpander : IViewLocationExpander
{
    private const string ThemeKey = "theme";

    // Called first — store the theme so Razor can cache per-theme
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        var themeService = context.ActionContext.HttpContext
            .RequestServices.GetRequiredService<IThemeService>();

        context.Values[ThemeKey] = themeService.GetCurrentTheme();
    }

    // Called to build the actual search paths
    public IEnumerable<string> ExpandViewLocations(
        ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        context.Values.TryGetValue(ThemeKey, out var theme);

        if (!string.IsNullOrEmpty(theme))
        {
            // {1} = controller, {0} = view name
            var themedLocations = new[]
            {
                $"/Views/Themes/{theme}/{{1}}/{{0}}.cshtml",
                $"/Views/Themes/{theme}/Shared/{{0}}.cshtml",
            };

            // Themed paths first → fallback to default locations
            return themedLocations.Concat(viewLocations);
        }

        return viewLocations;
    }
}

// Why PopulateValues matters: Razor caches compiled views. Storing the theme here ensures the cache is per-theme, not shared across themes.

// 3. Register in Program.cs
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IThemeService, ThemeService>();

builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());
    });

// If using Session
builder.Services.AddSession();
app.UseSession();

// 4. Theme Switching Controller
public class ThemeController : Controller
{
    public IActionResult Switch(string theme, string returnUrl = "/")
    {
        var allowedThemes = new[] { "Light", "Dark" };

        if (!allowedThemes.Contains(theme))
            return BadRequest("Invalid theme.");

        // Cookie'ye kaydet (30 gün)
        Response.Cookies.Append("theme", theme, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            HttpOnly = true,
            SameSite = SameSiteMode.Lax
        });

        return LocalRedirect(returnUrl);
    }
}

// 5. Theme Toggle in View
<!-- Views/Shared/_Layout.cshtml -->
<a href="/Theme/Switch?theme=Dark&returnUrl=@Context.Request.Path">🌙 Dark</a>
<a href="/Theme/Switch?theme=Light&returnUrl=@Context.Request.Path">☀️ Light</a>

// If a themed view doesn't exist it automatically falls back to the default — so you only need to override views that actually differ between themes.
```

```
User has "Dark" theme cookie
→ Requests /Home/Index

Search order:
1. /Views/Themes/Dark/Home/Index.cshtml   ✅ found → use this
2. /Views/Themes/Dark/Shared/Index.cshtml
3. /Views/Home/Index.cshtml               ← default fallback
4. /Views/Shared/Index.cshtml             ← default fallback
```