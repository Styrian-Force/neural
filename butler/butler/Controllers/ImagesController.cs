using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using butler.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace butler.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly ILogger _logger;

        public ImagesController(ILogger<ImagesController> logger)
        {
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valuei", "valueii" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        /*[HttpPost]
        public string Post([FromBody]IFormFile image)
        {
            return "hallo";
        }*/

        [HttpPost]
        public StatusCodeResult Post()
        {
            IFormFileCollection files = HttpContext.Request.Form.Files;
            foreach(IFormFile file in files) {
                _logger.LogInformation("\nFile: {0}", file.FileName);
            }
            return StatusCode(200);
            /*try
            {
                if (file != null && file.Length > 0)
                {
                    try
                    {

                        byte[] data;
                        using (var br = new BinaryReader(file.OpenReadStream()))
                            data = br.ReadBytes((int)file.OpenReadStream().Length);

                        ByteArrayContent bytes = new ByteArrayContent(data);


                        MultipartFormDataContent multiContent = new MultipartFormDataContent();

                        multiContent.Add(bytes, "file", file.FileName);

                        //var result = client.PostAsync("api/Values", multiContent).Result;

                        return StatusCode(200);

                    }
                    catch (Exception)
                    {
                        return StatusCode(500); // 500 is generic server error
                    }
                }

                //return StatusCode(400); // 400 is bad request

            }
            catch (Exception)
            {
                return StatusCode(500); // 500 is generic server error
            }*/
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
