using AuthoringApi.NetCoreApp.Communication;
using AuthoringApi.NetCoreApp.Model;
using AuthoringApi.NetCoreApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthoringApi.NetCoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClientLogger _clientLogger;
        private readonly IManagementApiService _managementApiService;
        private readonly IAuthoringApiService _authoringApiService;

        public HomeController(IClientLogger clientLogger, IManagementApiService managementApiService, IAuthoringApiService authoringApiService)
        {
            _clientLogger = clientLogger ?? throw new ArgumentNullException(nameof(clientLogger));
            _managementApiService = managementApiService ?? throw new ArgumentNullException(nameof(managementApiService));
            _authoringApiService = authoringApiService ?? throw new ArgumentNullException(nameof(authoringApiService));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("RebuildIndex")]
        public IActionResult RebuildIndex(string indexName)
        {
            _clientLogger.Info("**************************************************************");
            _clientLogger.Info("Rebuilding Indexes");
            var rebuildIndexResponse = _managementApiService.RebuildIndexes( [ indexName ]);

            foreach (var job in rebuildIndexResponse.Data.RebuildIndexes.Jobs)
            {
                _clientLogger.Info($"Job {job.Name} started");
            }

            var allJobsComplete = false;
            while (!allJobsComplete)
            {
                allJobsComplete = true;
                foreach (var job in rebuildIndexResponse.Data.RebuildIndexes.Jobs)
                {
                    var jobStatus = _managementApiService.CheckJobStatus(job.Name);
                    _clientLogger.Info($"{jobStatus.Data.Job.Name}: Processed: {jobStatus.Data.Job.Status.Processed}  Done: {jobStatus.Data.Job.Done}");

                    if (!jobStatus.Data.Job.Done)
                    {
                        allJobsComplete = false;
                    }
                }
                if (!allJobsComplete)
                {
                    Thread.Sleep(5000);
                }
            }

            _clientLogger.Info("Indexing complete");
            _clientLogger.Info("**************************************************************");
            return Ok("Index rebuilt successfully");
        }

        [HttpPost, ActionName("UpdateItem")]
        public IActionResult UpdateItem([FromBody] UpdateItemRequest request)
        {
            _clientLogger.Info("**************************************************************");
            _clientLogger.Info($"Updating the {request.FieldName} field on {request.ItemName} to {request.NewValue}");

            var query = @"path:""/sitecore/content/taco-demo/authoring-api/home/{{itemname}}""
                          fields:[
                            {name:""{{fieldname}}"", value:""{{fieldvalue}}""}
                          ]";
            var queryWithValues = query
                .Replace("{{itemname}}", request.ItemName)
                .Replace("{{fieldname}}", request.FieldName)
                .Replace("{{fieldvalue}}", request.NewValue);

            var result = _authoringApiService.UpdateItem(queryWithValues);

            if (result.Success)
            {
                _clientLogger.Info($"{request.ItemName} was updated successfully!");
            }
            else
            {
                _clientLogger.Error($"Failed to update {request.ItemName}");
                foreach(var error in result.Errors)
                {
                    _clientLogger.Error(error.Message);
                }
            }

            _clientLogger.Info("**************************************************************");
            return Ok();
        }

        [HttpPost, ActionName("CreateItem")]
        public IActionResult CreateItem([FromBody] CreateItemRequest request)
        {
            _clientLogger.Info("**************************************************************");
            _clientLogger.Info($"Creating item {request.ItemName}");

            var query = @"name: ""{{itemname}}""
                  templateId: ""{B4BE8618-2F26-45C9-80AD-6E4874F16B1E}""
                  parent: ""{91559D18-C7A4-4BE6-A5F6-EB31CC82F645}""
                  language: ""en""
                  fields: [
                    { name: ""title"", value: ""{{title}}"" }
                    { name: ""navigationtitle"", value: ""{{title}}"" }
                  ]";
            var queryWithValues = query
                .Replace("{{itemname}}", request.ItemName)
                .Replace("{{title}}", request.Title);

            var result = _authoringApiService.CreateItem(queryWithValues);

            if (result.Success)
            {
                _clientLogger.Info($"{request.ItemName} was created successfully!");
            }
            else
            {
                _clientLogger.Error($"Failed to create {request.ItemName}");
                foreach (var error in result.Errors)
                {
                    _clientLogger.Error(error.Message);
                }
            }

            _clientLogger.Info("**************************************************************");
            return Ok();
        }
    }
}
