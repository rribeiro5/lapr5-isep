using System;
using System.Collections.Generic;
using System.Linq;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.ConnectionRequests
{
    public class ConnectionRequestTest
    {
        private static UserId OrigUser = new UserId(Guid.NewGuid());
        private static UserId InterUser = new UserId(Guid.NewGuid());
        private static UserId DestUser = new UserId(Guid.NewGuid());
        private const string MessageOrigToDest = "Origin User to DestUser";
        private const string MessageOrigToInter = "Origin User to InterUser";
        private const string MessageInterToDest = "Intermediary User to DestUser";
        private const int ConnStrengthReq = 10;
        private ConnectionRequest simpleRequest;
        private ConnectionRequest complexRequest;
        private List<string> _connTags;


        public ConnectionRequestTest()
        {
            _connTags = new List<string>
            {
                "Tag1",
                "Tag2",
                "Tag3"
            };
            complexRequest =  new ConnectionRequest(OrigUser, InterUser, DestUser, MessageOrigToDest,
                MessageOrigToInter, ConnStrengthReq, _connTags);
            simpleRequest = new ConnectionRequest(OrigUser, DestUser, MessageOrigToDest,
                ConnStrengthReq, _connTags);
        }

        // ConnectionRequest(UserId origUser, UserId interUser, UserId destUser, string messageOrigToDest,
        // string messageOrigToInter, int connStrength, List<string> connTags)
        [Fact]
        public void SuccessfullyCreateConnectionRequestWithIntermediaryUser()
        {
            Assert.Equal(OrigUser, complexRequest.OrigUser);
            Assert.Equal(InterUser, complexRequest.InterUser);
            Assert.Equal(DestUser, complexRequest.DestUser);
            Assert.Equal(MessageOrigToInter, complexRequest.MessageOrigToInter.Message);
            Assert.Null(complexRequest.MessageInterToDest);
            Assert.False(complexRequest.InAcceptance);
            Assert.True(complexRequest.OnApproval);
            Assert.Equal(MessageOrigToDest, complexRequest.MessageOrigToDest.Message);
            Assert.Equal(ConnStrengthReq, complexRequest.ConnStrengthReq.Strength);
            Assert.Equal(_connTags, TagsToString(complexRequest.ConnTagsReq));
        }
        
        //ConnectionRequest(UserId origUser, UserId destUser, string messageOrigToDest, int connStrength, List<string> connTags)
        [Fact]
        public void SuccessfullyCreateConnectionRequestWithoutIntermediaryUser()
        {
            Assert.Equal(OrigUser, simpleRequest.OrigUser);
            Assert.Null(simpleRequest.InterUser);
            Assert.Equal(DestUser, simpleRequest.DestUser);
            Assert.Null(simpleRequest.MessageInterToDest);
            Assert.Null(simpleRequest.MessageOrigToInter);
            Assert.False(simpleRequest.OnApproval);
            Assert.True(simpleRequest.InAcceptance);
            Assert.Equal(MessageOrigToDest, simpleRequest.MessageOrigToDest.Message);
            Assert.Equal(ConnStrengthReq, simpleRequest.ConnStrengthReq.Strength);
            Assert.Equal(_connTags, TagsToString(simpleRequest.ConnTagsReq));
        }

        [Fact]
        public void SuccessfullyApproveConnectionRequest()
        {
            complexRequest.Approve(MessageInterToDest);
            Assert.Equal(MessageInterToDest, complexRequest.MessageInterToDest.Message);
            Assert.True(complexRequest.Approved);
            Assert.True(complexRequest.InAcceptance);
            Assert.False(complexRequest.OnApproval);
        }

        [Fact]
        public void SuccessfullyDisapproveConnectionRequest()
        {
            complexRequest.Disapproved();
            Assert.False(complexRequest.Approved);
            Assert.False(complexRequest.InAcceptance);
            Assert.False(complexRequest.OnApproval);
        }
        
        [Fact]
        public void SuccessfullyAcceptConnectionRequest()
        {
            simpleRequest.AcceptRequest();
            Assert.False(simpleRequest.InAcceptance);
            Assert.True(simpleRequest.Accept);
        }
        
        [Fact]
        public void SuccessfullyRejectConnectionRequest()
        {
            simpleRequest.RejectRequest();
            Assert.False(simpleRequest.InAcceptance);
            Assert.False(simpleRequest.Accept);
        }

        [Fact]
        public void FailToCreateSimpleConnectionRequestWithoutOrigUser()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new ConnectionRequest(null, DestUser, MessageOrigToDest,
                ConnStrengthReq, _connTags));
        }

        [Fact]
        public void FailToCreateSimpleConnectionRequestWithoutDestUser()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new ConnectionRequest(OrigUser, null, MessageOrigToDest,
                ConnStrengthReq, _connTags));
        }
        
        [Fact]
        public void FailToCreateSimpleConnectionRequestWithoutUsers()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new ConnectionRequest(null, null, MessageOrigToDest,
                ConnStrengthReq, _connTags));
        }
        
        
        [Fact]
        public void FailToCreateComplexConnectionRequestWithoutOrigUser()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new ConnectionRequest(null, InterUser, DestUser, MessageOrigToDest,
                MessageOrigToInter, ConnStrengthReq, _connTags));
        }
        
        [Fact]
        public void FailToCreateComplexConnectionRequestWithoutInterUser()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new ConnectionRequest(OrigUser, null, DestUser, MessageOrigToDest,
                MessageOrigToInter, ConnStrengthReq, _connTags));
        }
        
        [Fact]
        public void FailToCreateComplexConnectionRequestWithoutDestUser()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new ConnectionRequest(OrigUser, InterUser, null, MessageOrigToDest,
                MessageOrigToInter, ConnStrengthReq, _connTags));
        }
        
        [Fact]
        public void FailToCreateComplexConnectionRequestWithoutUsers()
        {
            Assert.Throws<BusinessRuleValidationException>(()=> new ConnectionRequest(null, null, null, MessageOrigToDest,
                MessageOrigToInter, ConnStrengthReq, _connTags));
        }


        private static List<string> TagsToString(List<Tag> l)
        {
            var tmp = l.Select(tag => tag.Description).ToList();
            return tmp;
        }

    }
}