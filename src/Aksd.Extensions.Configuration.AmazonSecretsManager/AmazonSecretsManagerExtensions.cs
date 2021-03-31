using System;
using Microsoft.Extensions.Configuration;
using Amazon;
using Amazon.SecretsManager;

namespace Aksd.Extensions.Configuration.AmazonSecretsManager
{
    public static class AmazonSecretsManagerExtensions
    {
        public static IConfigurationBuilder AddAmazonSecretsManagerConfiguration(
            this IConfigurationBuilder builder, string secretId)
        {
            return builder.Add(new AmazonSecretsManagerSource(secretId));
        }
    }
}
