using LibraryManagement.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.BusinessLogic.Extensions;

public static class BusinessLogicDependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBookService, BookService>();
        
        return services;
    }
}
