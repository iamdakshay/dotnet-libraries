# Libraries for dotnet based applications

|    |  Project  |    |
|  -  |  ----  |  ----  |
|  1  |  Configuration extension for Amazon Secrets Manager <br/><kbd>[project](https://www.nuget.org/packages/Aksd.Extensions.Configuration.AmazonSecretsManager/)</kbd><kbd>[nuget-package](https://www.nuget.org/packages/Aksd.Extensions.Configuration.AmazonSecretsManager/)</kbd><kbd>[usage](#amazon-secrets-manager-configuration-extension)</kbd>  |  Amazon Secrets Manager service provides secure store for key/value pairs.<br/>Unlike parameter store, the data stored in Secrets Manager is encrypted by default.<br/>You can use an extension to load config values in application's Configuration object.<br/>You must have saved config values in JSON format against keys in Secrets Manager.  |    |


<br/>


-------------------------------
## Amazon Secrets Manager configuration extension
Usage:

<br/>
1. Add secret in Amazon Secrets Manager, "PubSubInfra" as secret id and configuration key/value pairs as below.  

|  key  |  value  |
|  ---  |  ----  |
|  Kafka  |  { "BootstrapServers": "localhost", "Producer": { "ClientId": "19", "StatisticsIntervalMs": 5000, "MessageTimeoutMs": 10000, "SocketTimeoutMs": 10000, "ApiVersionRequestTimeoutMs": 10000, "MetadataRequestTimeoutMs": 5000, "RequestTimeoutMs": 5000 }, "Consumer": { "GroupId": "49", "EnableAutoCommit": true, "StatisticsIntervalMs": 5000, "SessionTimeoutMs": 10000 } }  |
|  RabbitMQ  |  { "Uri": "localhost", "UserName": "root@3", "Password": "SWAMI", "DispatchConsumersAsync": true }  |

<br/>
2. Install Aksd.Extensions.Configuration.AmazonSecretsManager package. Add extesion in using following code in Program.cs
<br/>

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddAmazonSecretsManagerConfiguration("PubSubInfra");
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```

<br/>
3. Access configuration in code.

<br/>

Startup.cs - <i>Options pattern</i>
```csharp
services.Configure<KafkaConfig>(Configuration.GetSection("Kafka"));
services.Configure<RabbitMQConfig>(Configuration.GetSection("RabbitMQ"));
```

```csharp
services.AddSingleton<IKafkaConnection>(sp =>
    {
        var kafkaConfigOption = sp.GetService<IOptions<KafkaConfig>>();
        var kafkaConfig = kafkaConfigOption.Value;
        return new DefaultKafkaConnection(
            bootstrapServers: kafkaConfig.BootstrapServers,
            clientId: kafkaConfig.Producer.ClientId,
            statisticsIntervalMs: 5000,
            messageTimeoutMs: 1000,
            socketTimeoutMs: 10000,
            apiVersionRequestTimeoutMs: 10000,
            metadataRequestTimeoutMs: 5000,
            requestTimeoutMs: 5000,
            groupId: "",
            enableAutoCommit: true,
            sessionTimeoutMs: 10000);
    });

services.AddSingleton<IRabbitMQConnection>(sp =>
    {
        var rabbitMQConfigOptions = sp.GetService<IOptions<RabbitMQConfig>>();
        var rabbitMQConfig = rabbitMQConfigOptions.Value;

        var connectionFactory = new ConnectionFactory()
        {
            Uri = new Uri(rabbitMQConfig.Uri),
            UserName = rabbitMQConfig.UserName,
            Password = rabbitMQConfig.Password,
            DispatchConsumersAsync = rabbitMQConfig.DispatchConsumersAsync,
        };

        var rabbitMQConnection = new DefaultRabbitMQConnection(connectionFactory);
        return rabbitMQConnection;
    });
```