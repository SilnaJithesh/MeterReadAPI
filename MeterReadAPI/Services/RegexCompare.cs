using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using MeterReadAPI.DAL;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;
using MeterReadAPI.Extensions;

namespace MeterReadAPI.Services
{
    public class RegexCompare
    {

        string columnName { get; set; }
        string pattern { get; set; }
        bool positiveNegative { get; set; }
        bool required { get; set; }

        public static Dictionary<string, RegexCompare> dict = new Dictionary<string, RegexCompare>() {

               { Constants.AccountId, new RegexCompare() { columnName =  Constants.AccountId, pattern =@"^[0-9]+$", positiveNegative = true, required = true}},
               { Constants.MeterReadingDateTime, new RegexCompare() { columnName =  Constants.AccountId, pattern = @"\d{2}/\d{2}/\d{4} \d{2}:\d{2}", positiveNegative = true, required = true}},
               { Constants.MeterReadValue, new RegexCompare() { columnName =  Constants.AccountId, pattern = @"^[0-9]+$", positiveNegative = true, required = true}}, // This can be converted to match for just 5 digits if required changing the pattern to @"\d{5}" if reuired.

        };

        public Dictionary<string, int> ValidateAndImport(DataTable dt, ApplicationDBContext context, IFormatProvider culture)
        {

            int successCount = 0;
            int failureCount = 0;
            Dictionary<string, int> TotalCountResult = new Dictionary<string, int>();
            DataTable dtValidated = null;

            foreach (DataRow row in dt.AsEnumerable())
            {
                Boolean error = false;
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == Constants.AccountId || col.ColumnName == Constants.MeterReadingDateTime || col.ColumnName == Constants.MeterReadValue)
                    {
                        RegexCompare regexCompare = dict[col.ColumnName];
                        object colValue = row.Field<object>(col.ColumnName);
                        if (regexCompare.required)
                        {
                            if (colValue == null)
                            {

                                error = true;
                                break;
                            }
                        }
                        else
                        {
                            if (colValue == null)
                                continue;
                        }
                        string colValueStr = colValue.ToString();

                        Match match = Regex.Match(colValueStr, regexCompare.pattern);
                        if (regexCompare.positiveNegative)
                        {
                            if (!match.Success)
                            {
                                error = true;
                                break;
                            }
                            if (colValueStr.Length != match.Value.Length)
                            {
                                error = true;
                                break;
                            }
                        }
                        else
                        {
                            if (match.Success)
                            {
                                error = true;
                                break;
                            }
                        }
                    }
                }

                if (!error)
                {
                   
                    if (dtValidated == null) dtValidated = dt.Clone();
                    dtValidated.Rows.Add(row.ItemArray);
                    int acntId = int.Parse(row[Constants.AccountId].ToString());
                    var member = context.CustomerAccount.Where(x => x.AccountId == acntId);


                    if (member != null)
                    {

                        var cont = context.MeterReadings.Where(x => x.AccountId == acntId).OrderBy(x => x.MeterReadingDateTime).LastOrDefault();
                        var currentAcctDate = DateTime.ParseExact(row[Constants.MeterReadingDateTime].ToString(), "dd/MM/yyyy HH:mm", culture);

                        if (cont != null)
                        {
                            if (cont.MeterReadValue != row[Constants.MeterReadValue].ToString() && cont.MeterReadingDateTime != currentAcctDate)
                            {
                                if (DateTime.Compare(cont.MeterReadingDateTime, currentAcctDate) < 0)
                                {
                                    var newEntry = new Models.MeterReadings
                                    {
                                        AccountId = acntId,
                                        MeterReadingDateTime = currentAcctDate,
                                        MeterReadValue = row[Constants.MeterReadValue].ToString()
                                    };
                                    successCount++;
                                    context.Add(newEntry);
                                    context.SaveChanges();
                                }
                                else
                                {
                                    failureCount++;
                                }
                            }
                            else
                            {
                                failureCount++;
                            }
                        }
                        else
                        {
                            var newEntry = new Models.MeterReadings
                            {
                                AccountId = acntId,
                                MeterReadingDateTime = currentAcctDate,
                                MeterReadValue = row[Constants.MeterReadValue].ToString()
                            };
                            successCount++;
                            context.Add(newEntry);
                            context.SaveChanges();
                        }

                    }

                }
                else
                {
                    failureCount++;
                }
            }

            TotalCountResult.Add(Constants.SuccessCount, successCount);
            TotalCountResult.Add(Constants.FailureCount, failureCount);

            return TotalCountResult;
        }
    }
}
