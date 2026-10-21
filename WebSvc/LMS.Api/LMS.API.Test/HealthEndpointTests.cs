using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace LMS.API.Test;

[TestFixture]
public class HealthEndpointTests
{
    private WebApplicationFactory<Program> _factory = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Jwt:Issuer"] = "test-issuer",
                    ["Jwt:SecretKey"] = "test-secret-key-that-is-at-least-32-chars",
                    ["DBConfiguration:ConnectionString"] = "mongodb://localhost:27017",
                    ["DBConfiguration:DatabaseName"] = "LmsTests"
                })));
    }

    [TearDown]
    public void TearDown() => _factory.Dispose();

    [Test]
    public async Task Health_ReturnsOk()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Health_ReturnsHealthyPayload()
    {
        using var client = _factory.CreateClient();

        var body = await client.GetStringAsync("/health");

        StringAssert.Contains("healthy", body);
    }
}
