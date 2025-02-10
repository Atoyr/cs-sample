using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Medoz.WebApi;

namespace WebApi.Test;

public class IgnitionEndpointTests: IClassFixture<WebApplicationFactory<Program>>

{
    private readonly WebApplicationFactory<Program> _factory;

    public IgnitionEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetRequest_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync("/echo");
        // Assert
        response.EnsureSuccessStatusCode();
    }

}
