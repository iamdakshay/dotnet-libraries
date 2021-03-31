using Microsoft.Extensions.Configuration;
using Amazon;
using Amazon.SecretsManager;

namespace Aksd.Extensions.Configuration.AmazonSecretsManager
{
    public class AmazonSecretsManagerSource : IConfigurationSource
    {
        private readonly IAmazonSecretsManager _secretManager;
        private readonly string _secretId;
        public AmazonSecretsManagerSource(string secretId)
        {
            _secretId = secretId;
            _secretManager = new AmazonSecretsManagerClient();
        }

        public AmazonSecretsManagerSource(string secretId, IAmazonSecretsManager secretManager)
        {
            _secretId = secretId;
            _secretManager = secretManager;
        }

        public AmazonSecretsManagerSource(string secretId, string region)
        {
            _secretId = secretId;
            _secretManager = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AmazonSecretsManagerConfigurationProvider(_secretManager, _secretId);
        }

    }
}
