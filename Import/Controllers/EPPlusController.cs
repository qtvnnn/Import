using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Import.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class EPPlusController : ControllerBase
    {
        // GET: api/<EPPlusController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EPPlusController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EPPlusController>
        [HttpPost]
        public async Task<Response<List<Customer>>> Post(IFormFile formFile, CancellationToken cancellationToken)
        {
            if(formFile == null || formFile.Length <= 0)
            {
                return Response<List<Customer>>.GetResult(-1, "formfile is empty");
            }
            if(!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return Response<List<Customer>>.GetResult(-1, "Not support file extension");
            }

            var list = new List<Customer>();

            using(var stream = new MemoryStream())
            {
                await FormFile.CopyToAsync(stream, cancellationToken);
            }

            return Response<List<Customer>>.GetResult(0, "OK", list);
        }

        // PUT api/<EPPlusController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EPPlusController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
