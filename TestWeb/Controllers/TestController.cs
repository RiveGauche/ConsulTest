using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string defaultUrl = "http://apitest/api/values";
            var targetUrl = this.ResolveUrl(defaultUrl);
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Get;
                requestMessage.RequestUri = new Uri(targetUrl);
                var result = client.SendAsync(requestMessage).Result;
                return new JsonResult(new
                {
                    targetUrl,
                    Content = result.Content.ReadAsStringAsync().Result
                });
            }
        }

        private string ResolveUrl(string url)

        {
            Uri uri = new Uri(url);
            string serviceName = uri.Host;//ApiTest
            var baseUrl = this.GetServiceAddress(serviceName);
            return $"{uri.Scheme}://{baseUrl}{uri.PathAndQuery}";
        }


        private string GetServiceAddress(string serviceName)
        {
            using (ConsulClient client = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
            {
                Dictionary<string, AgentService> services = client.Agent.Services().Result.Response;
                var agentServices = services.Where(c => c.Value.Service.Equals(serviceName, StringComparison.CurrentCultureIgnoreCase)).Select(s => s.Value);
                var agentService = agentServices.ElementAt(Environment.TickCount % agentServices.Count());
                return $"{agentService.Address}:{agentService.Port}";
            }
        }


    }
}