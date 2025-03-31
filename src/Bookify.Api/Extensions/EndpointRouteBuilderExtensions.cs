using Asp.Versioning.Builder;
using Asp.Versioning;

namespace Bookify.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static ApiVersionSet CreateApiVersionSet(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
    }

}