# Libraries for dotnet based applications

|    |  Project  |  Description  |  Nuget package  |
|  --  |  ---  |  ---  |  ---  |
|  1  |  [Configuration extension for Amazon Secrets Manager](#amazon-secrets-manager-configuration-extension)  |  description |  [link](https://www.nuget.org/packages/Aksd.Extensions.Configuration.AmazonSecretsManager/)  |

-------------------------------

## Amazon Secrets Manager configuration extension
Amazon Secrets Manager service provides secure store for key/value pairs. Unlike parameter store, the data stored in Secrets Manager is encrypted by default. You can use an extension to load config values in application's Configuration object. You must have saved config values in JSON format against keys in Secrets Manager.

Usage:
```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddAmazonSecretsManagerConfiguration("SecretId");
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```
