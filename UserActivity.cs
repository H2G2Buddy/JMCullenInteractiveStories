using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DataRepository;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using JMCullenInteractiveStories.DataRepository;
using Microsoft.Extensions.Primitives;
using StoryAI.Shared;

namespace JMCullenInteractiveStories
{
    /*
    public class UserActivity
    {
        [FunctionName("UserActivity")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
            ExecutionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody))
                return (ActionResult)new OkObjectResult(new RequestResponse<ReaderActivity>(new RequestBodyEmptyException()));
            ReaderActivity data = JsonConvert.DeserializeObject<ReaderActivity>(requestBody);

            StringValues val = "";
            if (req.Headers.TryGetValue("UserAccess", out val))
            {

            }

            ReaderActivity results = ActivityRepository.CreateNew(executionContext, "UserActivity").AddOrUpdate(data);
            if (results == null || results.PrimaryKey == 0)
                return (ActionResult)new OkObjectResult(new InsertRecordException(data.AccessKey.ToString()));
            else
                return (ActionResult)new OkObjectResult(new RequestResponse<ReaderActivity>(results));
        }
        public static class GetAllActivity
        {
            [FunctionName("ActivityList")]
            public static async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
                ExecutionContext executionContext)
            {
                IEnumerable<ReaderActivity> results = ActivityRepository.CreateNew(executionContext, "ActivityList").GetAll();
                if (results == null || results.Count() == 0)
                    return (ActionResult)new OkObjectResult(new RequestResponse<ReaderActivity>(new ErrorReadingDataFile(typeof(ReaderActivity).Name)));
                else
                    return (ActionResult)new OkObjectResult(new RequestResponse<ReaderActivity>(results));
            }
        }
    }
    */
}
