using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWeb3._0
{
    public static class RegisterConsul
    {
        public static void RegistConsul(this IConfiguration configuration)
        {
            #region 注册
            string ip = configuration["ip"] ?? "Localhost";
            int port = string.IsNullOrWhiteSpace(configuration["port"]) ? 44344 : int.Parse(configuration["port"]);
            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri("http://127.0.0.1:8500");
                c.Datacenter = "dc1";
            });
            Task<WriteResult> result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "ApiTest-" + Guid.NewGuid(),
                Name = "ApiTest",
                Address = ip,
                Port = port,
                Tags = new string[] { },//设置权重
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = $"http://{ip}:{port}/health",
                    Timeout = TimeSpan.FromSeconds(5)
                }
            });

            #endregion
        }
    }
}
