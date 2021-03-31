using System;
using Microsoft.Extensions.Configuration;
using Amazon.SecretsManager;
using System.Collections.Generic;


namespace Aksd.Extensions.Configuration.AmazonSecretsManager
{
    public class AmazonSecretsManagerConfigurationProvider : ConfigurationProvider
    {
        private readonly string _secretId;
        private readonly IAmazonSecretsManager _secretManager;
        public AmazonSecretsManagerConfigurationProvider(IAmazonSecretsManager secretManager, string secretId)
        {
            _secretId = secretId;
            _secretManager = secretManager;
        }

        public override void Load()
        {
            var secret = AmazonSecretsManagerUtility.GetSecret(_secretId, _secretManager);
            var data = new JsonConfigurationParser().Parse(secret);
            Data = data;
        }

    }
}
