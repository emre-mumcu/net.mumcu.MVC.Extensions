using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace net.mumcu.MVC.Extensions.RazorExtensions;

public static class ViewLocationExpanderExtension
{
    public static IServiceCollection _AddCustomViewLocations(this IServiceCollection services)
    {
        services
            .Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

        return services;
    }
}

public class ViewLocationExpander : IViewLocationExpander
{
    // {2} is area, {1} is controller, {0} is the action  
    private readonly IEnumerable<string> _customLocations = new[]
    {
        "/Partials/{0}/{0}" + RazorViewEngine.ViewExtension,
        "/Partials/{0}" + RazorViewEngine.ViewExtension,
        "/Partials/Shared/{0}" + RazorViewEngine.ViewExtension,
    };

    #region Optional Constructor to get values at the time of registration

    public ViewLocationExpander() { }

    public ViewLocationExpander(IEnumerable<string> customLocations)
    {
        _customLocations = customLocations;
    }

    #endregion

    public void PopulateValues(ViewLocationExpanderContext context)
    {
        context.Values["customviewlocation"] = nameof(ViewLocationExpander);
    }

    // Runs in every request
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        // return customLocations.Concat(viewLocations);
        // return customLocations.Union(viewLocations);
        IEnumerable<string> allviewLocations = _customLocations.Union(viewLocations);
        return allviewLocations;
    }
}


/*
    Add Custom View Locations

    builder.Services.AddControllersWithViews()
        .AddRazorOptions(options =>
        {
            // Add custom view locations
            // {0} = view name, {1} = controller name, {2} = area name
            options.ViewLocationFormats.Add("/Views/Partials/{0}.cshtml");
            options.ViewLocationFormats.Add("/Views/Shared/Partials/{0}.cshtml");
            options.ViewLocationFormats.Add("/Views/Components/{0}.cshtml");
        });    

    For Area-Based Custom Locations

    builder.Services.AddControllersWithViews()
        .AddRazorOptions(options =>
        {
            // For areas: {2} = area, {1} = controller, {0} = view
            options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Partials/{0}.cshtml");
            options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
        });


    Using a Custom IViewLocationExpander (Most Flexible)

    builder.Services.AddControllersWithViews()
        .AddRazorOptions(options =>
        {
            options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
        });


    Recommendation: Use ViewLocationFormats.Add(...) for simple cases, and IViewLocationExpander when you need conditional or dynamic path resolution (e.g., theming, multi-tenancy).
*/
