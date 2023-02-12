using PollingStationsResolver.Api.Options;

namespace PollingStationsResolver.Api.Services.Credentials;

public static class Installer
{
    public static IServiceCollection AddApplicationAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenSections = configuration.GetSection(TokenOptions.SectionKey);
        services.AddJWTBearerAuth(tokenSections.Get<TokenOptions>()!.SigningKey);


        services.Configure<TokenOptions>(tokenSections);
        services.Configure<ApplicationAdminCredentials>(configuration.GetSection(ApplicationAdminCredentials.SectionKey));
        services.AddSingleton<ICredentialsChecker, CredentialsChecker>();

        return services;
    }
}