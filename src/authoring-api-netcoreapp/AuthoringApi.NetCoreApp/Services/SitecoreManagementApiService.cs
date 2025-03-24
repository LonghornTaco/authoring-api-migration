using AuthoringApi.NetCoreApp.Configuration;
using AuthoringApi.NetCoreApp.Model.ManagementApi;
using AuthoringApi.NetCoreApp.Model.ManagementApi.Response.Indexes;
using AuthoringApi.NetCoreApp.Model.ManagementApi.Response.Jobs;

namespace AuthoringApi.NetCoreApp.Services
{
    public class SitecoreManagementApiService : ApiService, IManagementApiService
    {

        public SitecoreManagementApiService(IAuthoringApiConfiguration authoringApiConfiguration, ITokenManager tokenManager) : base(authoringApiConfiguration, tokenManager)
        {
        }

        public ManagementApiResponse<QueryIndexResponse> GetIndexes()
        {
            var query = @"
               query {
                  indexes(
                    indexNames:null
                  ) {
                    nodes {
                      name
                    }
                  }
                }
                ";

            var result = ExecuteQuery<ManagementApiResponse<QueryIndexResponse>>(query);
            return result;
        }

        public RebuildIndexResponse RebuildMasterIndexes()
        {
            return RebuildIndexes(new string[] { AuthoringApiConfiguration.MasterIndexName, AuthoringApiConfiguration.SxaMasterIndexName });
        }

        public RebuildIndexResponse RebuildIndexes(string[] indexes)
        {
            var query = @"
                mutation {
                  rebuildIndexes(
                    input: { indexNames: [{{indexes}}] } 
                  ) {
                    jobs {
                      name
                      handle
                      status {
                        total
                        messages
                        processed
                        jobState
                      }
                      done
                    }
                  }
                }
                ";

            var queryWithInput = query.Replace("{{indexes}}", string.Join(",", indexes.Select(index => $"\"{index}\"")));
            var result = ExecuteQuery<RebuildIndexResponse>(queryWithInput);
            return result;
        }

        public JobStatusResponse CheckJobStatus(string jobName)
        {
            var query = @"
                query {
                  job(input: { jobName: ""{{jobName}}"" }) {
                    name
                    handle
                    status {
                      messages
                      processed
                      jobState
                    }
                    done
                  }
                }
                ";

            var queryWithInput = query.Replace("{{jobName}}", jobName);
            var result = ExecuteQuery<JobStatusResponse>(queryWithInput);
            return result;
        }
    }
}
