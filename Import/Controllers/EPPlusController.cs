using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for(int row = 3; row <= rowCount; row++)
                    {
                        list.Add(new Customer
                        {
                            CustomerCode = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            FullName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            MemberCardCode = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            CustomerGroup = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            PhoneNumber = worksheet.Cells[row, 5].Value.ToString().Trim(),
                            //DateOfBirth = DateTime.Parse(worksheet.Cells[row, 6].Value.ToString().Trim()),
                            CompanyName = worksheet.Cells[row, 7].Value.ToString().Trim(),
                            TaxCode = worksheet.Cells[row, 8].Value.ToString().Trim(),
                            //Email = worksheet.Cells[row, 9].Value.ToString().Trim(),
                            //Address = worksheet.Cells[row, 10].Value.ToString().Trim(),
                            Note = worksheet.Cells[row, 11].Value.ToString().Trim(),
                        });
                    }
                }
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
