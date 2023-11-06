﻿using System.Net;
using System.Text;
using FluentAssertions;
using FluentResults;
using MediatR;
using Moq;
using Newtonsoft.Json;
using Template.Application.Customers.Common;
using Template.Application.Customers.UpdateCustomer;
using Xunit;

namespace Template.Integration.Tests.Customers.UpdateCustomer;

public class UpdateCustomerEndpointTests
{
    [Fact]
    public async Task Endpoint_Should_Return200Ok_WhenRequestIsValid()
    {
        var mockMediatorSender = new Mock<ISender>();
        mockMediatorSender
            .Setup(x => x.Send(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new Customer()));

        var client = TestClient.CreateClient(mockMediatorSender);

        var command = new UpdateCustomerCommand();

        var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
        
        var response = await client.PutAsync("/customers", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Endpoint_Should_Return400BadRequest_WhenRequestIsInvalid()
    {
        var mockMediatorSender = new Mock<ISender>();
        mockMediatorSender
            .Setup(x => x.Send(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<Customer>(""));

        var client = TestClient.CreateClient(mockMediatorSender);

        var command = new UpdateCustomerCommand();

        var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
        
        var response = await client.PutAsync("/customers", content);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
