//using App.Controllers;
//using App.Data;
//using App.Models.DTO;
//using AutoMapper;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace OrdersAPI.UnitTests.Systems.Controllers;

//public class TestOrdersController
//{
//    [Fact]
//    public async Task Get_OnSuccess_ReturnsStatusCode200()
//    {
//        // Arrange   
//        var sut = OrdersController();

//        // Act
//        var result = (OkObjectResult) await sut.Get();

//        // Assert
//        result.StatusCode.Should().Be(200);
//    }

//    [Fact]
//    public async Task Get_OnSuccess_InvokesOrderRepository()
//    {
//        var mockOrderRepository = Mock.Of<IOrderRepository>();
//        var mockMapper = Mock.Of<IMapper>();

//        // Arrange   
//        var sut = new OrdersController(mockOrderRepository, mockMapper);

//        // Act
//        var result = sut.Get() as OkObjectResult;

//        // Assert
//        result.StatusCode.Should().Be(200);
//    }
//}