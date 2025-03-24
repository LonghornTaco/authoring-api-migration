using AuthoringApi.NetCoreApp.Configuration;
using AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Create;
using AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Delete;
using AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Search;
using AuthoringApi.NetCoreApp.Model.AuthoringApi.Response.Update;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace AuthoringApi.NetCoreApp.Services
{
    public class SitecoreAuthoringApiService : ApiService, IAuthoringApiService
    {
        public SitecoreAuthoringApiService(IAuthoringApiConfiguration authoringApiConfiguration, ITokenManager tokenManager) : base(authoringApiConfiguration, tokenManager)
        {
        }

        public QueryItemResponse QueryItem(string itemPath)
        {
            var query = @"
                query {
                  item (
                    where: {
                      path:""{{path}}""
                    }
                  )
                   {
                    itemId,
                    displayName,
                    name,
                    path,
                    fields (excludeStandardFields:true) { 
                      nodes {
                        name, 
                        value
                      }
                    },
                    parent {
                      itemId,
                      displayName,
                      hasPresentation,
                      name,
                      template {
                        fullName,
                        name,
                        templateId
                      },
                      version
                    }
                    template{
                      fullName,
                      name,
                      templateId
                    },
                        version
                  }
                }
                ";
            var queryWithInput = query.Replace("{{path}}", itemPath);
            var result = ExecuteQuery<QueryItemResponse>(queryWithInput);
            return result;
        }

        public UpdateItemResponse UpdateItem(string input)
        {
            var query = @"
                mutation UpdateItem {
                    updateItem(
                    input:{
                        {{input}}
                    }
                    ) {
                    item {
                        itemId,
                        displayName,
                        name,
                        path,
                        fields (excludeStandardFields:true) { 
                            nodes {
                            name, 
                            value
                            }
                        },
                        parent {
                            itemId,
                            displayName,
                            hasPresentation,
                            name,
                            template {
                                fullName,
                                name,
                                templateId
                                },
                            version
                        }
                        template{
                            fullName,
                            name,
                            templateId
                        },
                        version
                    }
                  }
                }
                ";
            var queryWithInput = query.Replace("{{input}}", input);
            var result = ExecuteQuery<UpdateItemResponse>(queryWithInput);
            return result;
        }

        public CreateItemResponse CreateItem(string input)
        {
            var query = @"
                mutation CreateItem {
                    createItem(
                    input:{
                        {{input}}
                    }
                    ) {
                    item {
                      itemId,
                        displayName,
                        name,
                        path,
                        fields (excludeStandardFields:true) { 
                            nodes {
                            name, 
                            value
                            }
                        },
                        parent {
                            itemId,
                            displayName,
                            hasPresentation,
                            name,
                            template {
                                fullName,
                                name,
                                templateId
                                },
                            version
                        }
                        template{
                            fullName,
                            name,
                            templateId
                        },
                        version
                    }
                  }
                }
                ";
            var queryWithInput = query.Replace("{{input}}", input);
            var result = ExecuteQuery<CreateItemResponse>(queryWithInput);
            return result;
        }
    }
}