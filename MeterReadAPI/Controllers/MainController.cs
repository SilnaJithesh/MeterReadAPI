using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using MeterReadAPI.DAL;
using System.Globalization;
using MeterReadAPI.Extensions;
using MeterReadAPI.Services;

namespace MeterReadAPI.Controllers
{
    [ApiController]
    [Route("api/meter-reading-uploads")]
    public class MainController : ControllerBase
    {

        private ApplicationDBContext _context;

        IFormatProvider culture = new CultureInfo(Constants.CultureName, true);
        public MainController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Microsoft.AspNetCore.Http.IFormFile file)
        {
            Dictionary<string, int> DtValidated = new Dictionary<string, int>();

            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), Constants.Directory,
                file.FileName);

            //Copy the file to local path to do the validations 
            using (var stream = new FileStream(path, FileMode.Create))
                await file.CopyToAsync(stream);

            try
            {
                //Convert the csv to datatable
                DataTable dt = ConvertCSVtoDataTable(path);

                RegexCompare compare = new RegexCompare();

                //Validate the columns with the regular exp pattern defined & then import
                DtValidated = compare.ValidateAndImport(dt, _context, culture);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("Some Exception occured");
               
            }

            var result = new ViewModels.ResultReturn
            {
                SuccessCount = DtValidated[Constants.SuccessCount].ToString(),
                FailureCount = DtValidated[Constants.FailureCount].ToString()
            };

            return Ok(result);
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();

            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }

            return dt;
        }
       

    }
}





