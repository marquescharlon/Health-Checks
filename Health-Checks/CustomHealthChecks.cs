using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace Health_Checks
{
    public class CustomHealthChecks : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            HttpClient client = new HttpClient { BaseAddress = new Uri("https://jsonplaceholder.typicode.com") };
            var response = await client.GetAsync("users");
            var content = await response.Content.ReadAsStringAsync();

            //var users = JsonConvert.DeserializeObject<>(content);

            if (content == "" || content == null)
                return await Task.FromResult(new HealthCheckResult(
                status: HealthStatus.Unhealthy,
                description: "API está doente"
            ));
            else
                return await Task.FromResult(new HealthCheckResult(
                status: HealthStatus.Healthy,
                description: "API está funcionando"
            ));

            //try
            //{
            //    // Creates an HttpWebRequest for the specified URL.
            //    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://jsonplaceholder.typicode.com");
            //    // Sends the HttpWebRequest and waits for a response.
            //    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            //    if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
            //    {
            //        return await Task.FromResult(new HealthCheckResult(
            //        status: HealthStatus.Healthy,
            //        description: "API está funcionando"));
            //    }
            //    // Releases the resources of the response.
            //    myHttpWebResponse.Close();
            //}

            //catch (WebException e)
            //{
            //    return await Task.FromResult(new HealthCheckResult(
            //    status: HealthStatus.Unhealthy,
            //    description: "API está doente"));
            //}
        }
    }
}
