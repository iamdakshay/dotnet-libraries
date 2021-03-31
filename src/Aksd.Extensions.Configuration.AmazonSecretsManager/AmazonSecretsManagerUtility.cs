using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Aksd.Extensions.Configuration.AmazonSecretsManager
{
    internal static class AmazonSecretsManagerUtility
    {
        public static string GetSecret(string secretId, IAmazonSecretsManager client)
        {
            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretId;
            request.VersionStage = "AWSCURRENT";

            GetSecretValueResponse response = null;

            try
            {
                response = client.GetSecretValueAsync(request).Result;
            }
            catch (DecryptionFailureException e)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (InternalServiceErrorException e)
            {
                // An error occurred on the server side.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (InvalidParameterException e)
            {
                // You provided an invalid value for a parameter.
                // Deal with the exception here, and/or rethrow at your discretion
                throw e;
            }
            catch (InvalidRequestException e)
            {
                // You provided a parameter value that is not valid for the current state of the resource.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (ResourceNotFoundException e)
            {
                // We can't find the resource that you asked for.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (System.AggregateException e)
            {
                // More than one of the above exceptions were triggered.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }

            string secret = "";
            MemoryStream memoryStream = new MemoryStream();

            // Decrypts secret using the associated KMS CMK.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null)
            {
                secret = response.SecretString;
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
            return secret;
        }
    }
}
