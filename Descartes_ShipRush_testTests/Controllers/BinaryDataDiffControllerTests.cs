using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace Descartes_ShipRush_test.Controllers.Tests
{
    public class BinaryDataDiffControllerTests
    {
        //fix url endpoint
        private const string BaseUrl = "https://localhost:44372/swagger/index.html";

        //integration fact tests
        [Fact]
        public async Task TestBinaryDataDiff()
        {
            // Arrange
            int id = 1;
            string leftData = Convert.ToBase64String(Encoding.UTF8.GetBytes("hello"));
            string rightData = Convert.ToBase64String(Encoding.UTF8.GetBytes("heXlo"));

            // Send data to the left endpoint
            await SendDataToEndpoint($"v1/diff/{id}/left", new { Data = leftData });

            // Send data to the right endpoint
            await SendDataToEndpoint($"v1/diff/{id}/right", new { Data = rightData });

            // Act
            HttpResponseMessage response = await GetDiffResult(id);

            // Assert
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

            Assert.Equal("ContentDoNotMatch", result.DiffResultType.ToString());
            Assert.NotNull(result.Diffs);
            Assert.Equal(2, result.Diffs.Count);
        }

        private async Task SendDataToEndpoint(string endpoint, object data)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{BaseUrl}/{endpoint}", content);
                response.EnsureSuccessStatusCode();
            }
        }

        private async Task<HttpResponseMessage> GetDiffResult(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                return await client.GetAsync($"{BaseUrl}/v1/diff/{id}");
            }
        }


    }
}