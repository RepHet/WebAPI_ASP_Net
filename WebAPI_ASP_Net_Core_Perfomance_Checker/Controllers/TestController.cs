using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

    // Метод для виклику API та отримання даних
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
        public double ExecutionTimeMs { get; set; }
    }

    public class Memory
    {
        public string Title { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
    }
}
