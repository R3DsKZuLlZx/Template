using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Template.Integration.Tests;

public static class TestClient
{
    public static HttpClient CreateClient(Mock<ISender> mockMediatorSender)
    {
        return new TestServer(new WebHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddRouting();
                services.AddScoped(_ => mockMediatorSender.Object);
                services.AddCarter();
            })
            .Configure(app => 
            { 
                app.UseRouting();
                app.UseEndpoints(cfg => cfg.MapCarter());
            })
        ).CreateClient();
    }
}
