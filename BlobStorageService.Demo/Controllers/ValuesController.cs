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
            azureProvider.Initialize();
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            BlobStorageTools tools = new BlobStorageTools(azureProvider);

            string err = "";
            try
            {
                tools.Move("blah", "blah",  "blah", "blah");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                err += ex.Message;
            }

            try
            {
                tools.Upload("sample-de542a93-8a61-4ba6-b2e5-ed3e76f0c56c", @"./test1.txt");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                err += ex.Message;
            }
            
            try
            {
                tools.Download("sample-de542a93-8a61-4ba6-b2e5-ed3e76f0c56c", "test1.txt", "./copy-test1.txt");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                err += ex.Message;
            }
            
            if (err == "")
            {
                return Ok();
            }

            return BadRequest(err);
        }

    }
}
