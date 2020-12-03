using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace quantium.cacheRefresh
{
    public static class loggerFunc
    {
        [FunctionName("loggerFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# API Called");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            log.LogInformation(requestBody);
            if (data != null)
                log.LogInformation($"EventType = {data?[0].eventType}");

            //echo validation code to be compatible with Event Grid
            if (data?[0].data?.validationCode != null)
                return new OkObjectResult("{\"validationResponse\": \""+data?[0].data?.validationCode+"\"}");
            

            return new OkObjectResult("");
        }
    }
}
