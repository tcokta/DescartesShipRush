using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Descartes_ShipRush_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BinaryDataDiffController : ControllerBase
    {
        private static Dictionary<string, string> leftDataStore = new Dictionary<string, string>();
        private static Dictionary<string, string> rightDataStore = new Dictionary<string, string>();

        [HttpPost]
        [Route("v1/diff/{id}/left")]
        // currently non async
        // previously async Task<IActionResult>
        public IActionResult Left(string id, [FromBody] BinaryDataRequest request)
        {
            leftDataStore[id] = request.Data;
            return Ok();
        }

        [HttpPost]
        [Route("v1/diff/{id}/right")]
        public IActionResult Right(string id, [FromBody] BinaryDataRequest request)
        {
            rightDataStore[id] = request.Data;
            return Ok();
        }

        [HttpGet]
        [Route("v1/diff/{id}")]
        public IActionResult Diff(string id)
        {
            if (!leftDataStore.ContainsKey(id) || !rightDataStore.ContainsKey(id))
            {
                return NotFound();
            }

            string leftData = leftDataStore[id];
            string rightData = rightDataStore[id];

            if (leftData == rightData)
            {
                return Ok(new { Result = "Equal" });
            }

            if (leftData.Length != rightData.Length)
            {
                return Ok(new { Result = "NotEqualSize" });
            }

            List<DiffDetail> diffs = GetDifferences(leftData, rightData);

            return Ok(new { Result = "NotEqual", Diffs = diffs });
        }

        private List<DiffDetail> GetDifferences(string left, string right)
        {
            List<DiffDetail> diffs = new List<DiffDetail>();

            // Compare the two strings and find differences
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    int j = i;
                    while (j < left.Length && left[j] != right[j])
                    {
                        j++;
                    }

                    int diffLength = j - i;
                    diffs.Add(new DiffDetail { Offset = i, Length = diffLength });
                    i = j - 1;
                }
            }

            return diffs;
        }
        // simple in-memory dictionary to store binary data for the left and right endpoints, and it calculates the differences when the diff endpoint is called, error handling was not considered in this

    }
}
