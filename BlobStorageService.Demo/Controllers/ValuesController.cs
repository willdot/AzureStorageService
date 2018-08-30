using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlobStorageService.Service;
using Microsoft.AspNetCore.Mvc;

namespace BlobStorageService.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IAzureProvider azureProvider;
        public ValuesController(IAzureProvider _azureProvider)
        {
            azureProvider = _azureProvider;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            Console.WriteLine(azureProvider);

            BlobStorageTools tools = new BlobStorageTools(azureProvider);

            //tools.Upload("sample-de542a93-8a61-4ba6-b2e5-ed3e76f0c56c", @"c:\temp\Upload.txt");

            tools.Download("sample-de542a93-8a61-4ba6-b2e5-ed3e76f0c56c", @"c:\temp\", "Test.txt");


            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
