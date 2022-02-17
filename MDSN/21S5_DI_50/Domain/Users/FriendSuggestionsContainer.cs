using System;
using System.Collections.Generic;
using DDDSample1.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDSample1.Domain.Users
{
    public class FriendSuggestionsContainer
    {
        private readonly Dictionary<string, Action> _dictionary;
        private readonly IServiceCollection _services;
        private readonly IConfiguration _reader;
        private readonly string _configFileKey;

        public FriendSuggestionsContainer(IServiceCollection services, IConfiguration reader, string configFileKey)
        {
            _dictionary = new Dictionary<string, Action>();
            _services = services;
            _reader = reader;
            _configFileKey = configFileKey;
            _initDictionary();
        }

        public void MapService()
        {
            //read from appsettings.json
            var serviceKey = _reader.GetSection(_configFileKey)
                .GetSection("Algorithm")
                .Value;
            _dictionary[serviceKey].Invoke();
        }

        private void _initDictionary()
        {
            _dictionary.Add("random",_randomAlgorithm);
        }

        private void _randomAlgorithm()
        {
            _services.AddTransient<ISuggestedUsersService, RandomSuggestedUsersService>();
        }
    }
}