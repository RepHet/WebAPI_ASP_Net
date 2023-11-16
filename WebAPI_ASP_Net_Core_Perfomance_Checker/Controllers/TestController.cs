using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("api-tester")]
    public async Task<ActionResult<IEnumerable<TestData>>> GetTests([FromQuery] string request, [FromQuery] int numberOfRequests)
    {
        // Створення списку для результатів тестів
        List<Task<TestData>> tasks = new List<Task<TestData>>();

        // Виклик API та отримання даних для кількох запитів
        for (int i = 0; i < numberOfRequests; i++)
        {
            string apiUrl = request;
            tasks.Add(CallApiAndExtractTestData(apiUrl));
        }

        // Очікування завершення всіх асинхронних запитів
        TestData[] testResults = await Task.WhenAll(tasks);

        // Повернення списку тестів
        return testResults.ToList();
    }

    [HttpGet("api-tester-timing")]
    public async Task<ActionResult<TestExecutionTime>> GetTestsTiming([FromQuery] string request, [FromQuery] int numberOfRequests)
    {

        var stopwatch = Stopwatch.StartNew();

        using (var httpClient = new HttpClient())
        {
            for (int i = 0; i < numberOfRequests; i++)
            {
                HttpResponseMessage response = await httpClient.GetAsync(request);
            }
        }

        stopwatch.Stop();

        // Повернення загального часу виконання всіх запитів
        var responseModel = new TestExecutionTime
        {
            ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds
        };

        return responseModel;
    }

    [HttpGet("api-tester-timing-async")]
    public async Task<ActionResult<TestExecutionTime>> GetTestsTimingASync([FromQuery] string request, [FromQuery] int numberOfRequests)
    {
        Stopwatch stopwatch = new Stopwatch();

        using (HttpClient client = new HttpClient())
        {
            Console.WriteLine($"Вимірюємо час для {numberOfRequests} асинхронних запитів...");

            stopwatch.Start();

            // Створюємо масив завдань (tasks)
            Task<HttpResponseMessage>[] tasks = new Task<HttpResponseMessage>[numberOfRequests];

            // Запускаємо асинхронні запити
            for (int i = 0; i < numberOfRequests; i++)
            {
                tasks[i] = client.GetAsync(request);
            }

            // Очікуємо завершення всіх асинхронних запитів
            await Task.WhenAll(tasks);

            stopwatch.Stop();
        }

        // Повернення загального часу виконання всіх запитів
        var responseModel = new TestExecutionTime
        {
            ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds
        };

        return responseModel;
    }

    // Метод для виклику API та отримання даних з тестами
    private async Task<TestData> CallApiAndExtractTestData(string apiUrl)
    {
        string apiResponse = await CallApi(apiUrl);

        // Обробка отриманих даних та витягнення тестів
        return ExtractTestDataFromApiResponse(apiResponse);
    }

    // Метод для виклику API
    private async Task<string> CallApi(string apiUrl)
        {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }


    // Метод для обробки отриманих даних та витягнення тестів
    private TestData ExtractTestDataFromApiResponse(string apiResponse)
    {
        if (!string.IsNullOrEmpty(apiResponse))
        {
            // Використовуйте Newtonsoft.Json для десеріалізації JSON
            return JsonConvert.DeserializeObject<TestData>(apiResponse);
        }

        return null;
    }

    // Клас для десеріалізації JSON
    public class TestData
    {
        public string TestName { get; set; }
        public Metrics Metrics { get; set; }
    }

    public class Metrics
    {
        [JsonProperty("Test execution time")]
        public List<TestExecutionTime> TestExecutionTime { get; set; }
        public List<Memory> Memory { get; set; }
    }

    public class TestExecutionTime
    {
        private double _executionTimeMs;

        public double ExecutionTimeMs
        {
            get => _executionTimeMs;
            set => _executionTimeMs = Math.Round(value, 6);
        }
    }

    public class Memory
    {
        public string Title { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
    }
}
