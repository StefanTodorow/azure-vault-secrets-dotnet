using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace dotnetcore
{
    class Program
    {
        static void Main(string[] args)
        {
            string secretName = "MySecret";
            string keyVaultName = "vault-no-protect";
            var kvUri = "https://vault-no-protect.vault.azure.net/";

            SecretClientOptions options = new() {
                Retry = {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret(secretName);

            Console.WriteLine("GetSecret: " + secret.Value);
            Console.Write("Enter Secret > ");

            string secretValue = Console.ReadLine();
            client.SetSecret(secretName, secretValue);
            Console.WriteLine("SetSecret:");
            Console.Write(" Key: " + secretName);
            Console.WriteLine(" Value: " + secret.Value);

            client.StartDeleteSecret(secretName);
            Console.WriteLine("StartDeleteSecret: " + keyVaultName);
        }
    }
}
