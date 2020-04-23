﻿using System;
using System.Threading.Tasks;
using Blauhaus.Graphql.StrawberryShake.DtoCommandHandlers;
using Blauhaus.Graphql.StrawberryShake.Exceptions;
using Blauhaus.Graphql.StrawberryShake.TestHelpers;
using Blauhaus.Graphql.Tests.TestObjects;
using Blauhaus.TestHelpers.BaseTests;
using Blauhaus.TestHelpers.MockBuilders;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using StrawberryShake;
using Error = Blauhaus.Common.ValueObjects.Errors.Error;

namespace Blauhaus.Graphql.Tests.Tests.StrawberryShakeTests
{
    public class MutationClientHandlerTests : BaseServiceTest<MutationClientHandler<TestDto, TestGraphqlResponse, TestCommandDto>>
    {
        private TestCommandDto _commandDto;
        private IOperationResult<TestGraphqlResponse> _operationResult;
        private TestDto _dto;


        private MockBuilder<IGraphqlClient<TestGraphqlResponse, TestCommandDto>> MockGraphqlClient => AddMock<IGraphqlClient<TestGraphqlResponse, TestCommandDto>>().Invoke();
        private MockBuilder<IOperationResultConverter<TestDto, TestGraphqlResponse>> MockOperationResultConverter => AddMock<IOperationResultConverter<TestDto, TestGraphqlResponse>>().Invoke();

        [SetUp]
        public void Setup()
        {
            Cleanup();

            _commandDto = new TestCommandDto{Name = "Command"};
            _dto = new TestDto{Name = "Dto"};
            _operationResult = new OperationResultMockBuilder<TestGraphqlResponse>()
                .With(x => x.Data, new TestGraphqlResponse
                {
                    Dto = _dto
                }).Object;

            MockOperationResultConverter.Mock.Setup(x => x.Convert(_operationResult)).Returns(Result.Success(_dto));
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken)).ReturnsAsync(_operationResult);

            AddService(x => MockGraphqlClient.Object);
            AddService(x => MockOperationResultConverter.Object);
        }

        [Test]
        public async Task IF_Mutation_executes_successfully_SHOULD_convert_result_to_dto_and_returns()
        {
            //Act
            var result = await Sut.HandleAsync(_commandDto, CancellationToken);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(_dto, result.Value);
        }

        [Test]
        public void IF_Mutation_fails_with_Exception_SHOULD_throw_exception()
        {
            //Arrange
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ThrowsAsync(new Exception("oops"));

            //Act
            Assert.ThrowsAsync<Exception>(async () => await Sut.HandleAsync(_commandDto, CancellationToken), "oops");

        }

        [Test]
        public void IF_Mutation_fails_with_non_Error_error_SHOULD_throw()
        {
            //Arrange
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ReturnsAsync(new OperationResultMockBuilder<TestGraphqlResponse>()
                    .With_Error("oops").Object);

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await Sut.HandleAsync(_commandDto, CancellationToken), "oops");
        }

        [Test]
        public void IF_Mutation_fails_with_IError_with_message_extension_SHOULD_use_it_as_message()
        {
            //Arrange
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ReturnsAsync(new OperationResultMockBuilder<TestGraphqlResponse>()
                    .WithExtension("message", "underlying errsor message").Object);

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await Sut.HandleAsync(_commandDto, CancellationToken));
        }

        [Test]
        public void IF_Mutation_fails_with_multiple_non_Error_error_SHOULD_throw()
        {
            //Arrange
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ReturnsAsync((new OperationResultMockBuilder<TestGraphqlResponse>()
                    .With_Error("oops").Object));

            //Act
            Assert.ThrowsAsync<GraphqlException>(async () => await Sut.HandleAsync(_commandDto, CancellationToken));
        }

        [Test]
        public async Task IF_Mutation_fails_with_Error_SHOULD_return_failure()
        {
            //Arrange
            var error = Error.Create("Bad Thing");
            MockGraphqlClient.Mock.Setup(x => x.GetResultAsync(_commandDto, CancellationToken))
                .ReturnsAsync(new OperationResultMockBuilder<TestGraphqlResponse>()
                    .With_Error(error.ToString()).Object);

            //Act
            var result = await Sut.HandleAsync(_commandDto, CancellationToken);

            //Assert
            Assert.AreEqual(error.ToString(), result.Error);
        }
        
        protected Func<MockBuilder<TMock>> AddMock<TMock>() where TMock : class
        {
            return Mocks.AddMock<MockBuilder<TMock>, TMock>();
        }
        protected void AddService<T>(Func<IServiceProvider, T> func) where T : class
        {
            Services.AddSingleton<T>(func);
        }
        protected  void AddService<T>(T service) where T : class
        {
            Services.AddSingleton<T>(service);
        }
    }
}