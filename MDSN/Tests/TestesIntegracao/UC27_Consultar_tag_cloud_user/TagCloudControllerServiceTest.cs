﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC27_Consultar_Tag_Cloud_Utilizador
{
    public class TagCloudControllerServiceTest
    {
        private UserId _userId;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IConnectionRepository> _connRepo;
        private readonly TagCloudController _controller;
        private readonly TagService _service;
        private Dictionary<Tag, int> _repoResult;
        private List<TagCloudDTO> _result;

        public TagCloudControllerServiceTest()
        {
            _userId = new UserId(Guid.NewGuid());
            _userRepo = new Mock<IUserRepository>();
            _connRepo = new Mock<IConnectionRepository>();
            _service = new TagService(_userRepo.Object, _connRepo.Object);
            _controller = new TagCloudController(_service);
            _repoResult = new Dictionary<Tag, int>();
            _repoResult.Add(new Tag("tag1"),5);
            _repoResult.Add(new Tag("tag2"),13);
            _repoResult.Add(new Tag("tag3"),13);
            _repoResult.Add(new Tag("tag4"),1);
            _result = new List<TagCloudDTO>()
            {
                new("tag1", 5),
                new("tag2", 13),
                new("tag3", 13),
                new("tag4", 1),
            };
        }

        [Fact]
        public async void ReturnBadRequestResultWhenThrowsException()
        {
            _userRepo.Setup(x => x.GetUserTagCloud(_userId)).Throws(new BusinessRuleValidationException("test"));
            var result = await _controller.GetUserTagCloud(_userId.AsGuid());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void ReturnOkResultWhenRequestExists()
        {
            _userRepo.Setup(x => x.GetUserTagCloud(_userId)).Returns(Task.FromResult(_repoResult));
            var result = await _controller.GetUserTagCloud(_userId.AsGuid());
            Assert.IsType<OkObjectResult>(result);
        }
        

        [Fact]
        public async void ReturnOkResultWithSpecificResultWhenRequestExists()
        {
            _userRepo.Setup(x => x.GetUserTagCloud(_userId)).Returns(Task.FromResult(_repoResult));
            var result = await _controller.GetUserTagCloud(_userId.AsGuid()) as OkObjectResult;
            var lst = (List<TagCloudDTO>) result?.Value;
            
            Assert.Empty(_result.Intersect(lst));
        }
        
        [Fact]
        public async void ReturnOkResultWithSpecificCountWhenRequestExists()
        {
            _userRepo.Setup(x => x.GetUserTagCloud(_userId)).Returns(Task.FromResult(_repoResult));
            var result = await _controller.GetUserTagCloud(_userId.AsGuid()) as OkObjectResult;
            Assert.Equal(4, ((List<TagCloudDTO>)result?.Value).Count);
        }
    }
}