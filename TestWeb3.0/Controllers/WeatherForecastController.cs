using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestWeb3._0.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        async IAsyncEnumerable<int> GetBigResultsAsync()
        {
            await foreach (var result in GetResultsAsync())
            {
                if (result > 20) yield return result;
            }
        }

        private IAsyncEnumerable<int> GetResultsAsync()
        {
            throw new NotImplementedException();
        }

        async IAsyncEnumerable<int> GetValuesFromServer()
        {
            await foreach (var number in GenerateSequence())
            {
                Console.WriteLine(number);
            }
            while (true)
            {
                IEnumerable<int> batch = await GetNextBatch();
                if (batch == null) yield break;

                foreach (int item in batch)
                {
                    yield return item;
                }
            }
        }

        private Task<IEnumerable<int>> GetNextBatch()
        {
            throw new NotImplementedException();
        }

        public static async IAsyncEnumerable<int> GenerateSequence()
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }
       

    public struct Point
        {
            public double X { get; set; }
            public double Y { get; set; }
            public readonly double Distance => Math.Sqrt(X * X + Y * Y);
            public readonly override string ToString() => $"({X}, {Y}) is {Distance} from the origin";
        }


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("XXXXX");
            Range range = 1..4;
            Index index = 1;
            var quickBrownFox = Summaries[range];
            var sb = new StringBuilder();
            foreach (var item in quickBrownFox)
            {
                sb.Append(item).Append("-");
            }
            var quickBrown =  Summaries[index];
            _logger.LogInformation($"XXXXX{sb.ToString()}-{quickBrown}");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet]
        [Route("image")]
        public IActionResult Image() 
        {
            string filePath = @"E:\Pictures\123.jpg";
            byte[] buffer;
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            return File(buffer, "image/jpeg");
        }
    }
}
