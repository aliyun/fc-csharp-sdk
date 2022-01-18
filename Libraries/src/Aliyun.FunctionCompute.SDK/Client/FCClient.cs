using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using RestSharp;
using Aliyun.FunctionCompute.SDK.Request;
using Aliyun.FunctionCompute.SDK.Config;
using Aliyun.FunctionCompute.SDK.Response;

namespace Aliyun.FunctionCompute.SDK.Client
{
    public class FCClient
    {
        protected RestClient RestHttpClient { get; private set; }
        public FCConfig Config { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Aliyun.FunctionCompute.SDK.Client.FCClient"/> class.
        /// </summary>
        /// <param name="region">Region.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="accessKeyId">Access key identifier.</param>
        /// <param name="accessKeySecret">Access key secret.</param>
        /// <param name="securityToken">Security token.</param>
        public FCClient(string region, string uid, string accessKeyId, string accessKeySecret, string securityToken = "")
        {
            Config = new FCConfig(region, uid, accessKeyId, accessKeySecret, securityToken, false);
            RestHttpClient = new RestClient(Config.Endpoint);
            RestHttpClient.Timeout = 1800000; // 30min (unit: ms)
        }

        /// <summary>
        /// Set RestClient timeout.
        /// </summary>
        /// <param name="timeout">RestClient timeout.</param>
        public void SetClientTimeout(int timeout)
        {
            this.RestHttpClient.Timeout = timeout;
        }

        /// <summary>
        /// Get the RestClient timeout.
        /// </summary>
        /// <returns>RestClient timeout.</returns>
        public int GetClientTimeout()
        {
            return this.RestHttpClient.Timeout;
        }

        public void SetEndpoint(string endpoint)
        {
            this.Config.Endpoint = endpoint;
            RestHttpClient = new RestClient(endpoint);
        }

        public T DoRequestCommon<T>(RestRequest r) where T : IResponseBase
        {
            IRestResponse response = this.RestHttpClient.Execute(r);
            if (response.StatusCode.GetHashCode() > 299)
            {
                Console.WriteLine(string.Format("ERROR: response status code = {0}; detail = {1}", response.StatusCode.GetHashCode(), response.Content));
            }
            T res = Activator.CreateInstance<T>();
            res.SetStatusContent(response.Content, response.StatusCode.GetHashCode(), response.RawBytes);
            var respHeaders = new Dictionary<string, object> { };
            foreach (var item in response.Headers)
                respHeaders.Add(item.Name, item.Value);

            res.SetHeaders(respHeaders);
            return res;
        }

        // reference: https://help.aliyun.com/document_detail/52877.html
        #region Service
        /// <summary>
        /// Create the service.
        /// </summary>
        /// <returns>The service.</returns>
        /// <param name="createServiceRequest">Create service request.</param>
        public CreateServiceResponse CreateService(CreateServiceRequest createServiceRequest)
        {
            return this.DoRequestCommon<CreateServiceResponse>(createServiceRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Get the service.
        /// </summary>
        /// <returns>The service.</returns>
        /// <param name="getServiceRequest">Get service request.</param>
        public GetServiceResponse GetService(GetServiceRequest getServiceRequest)
        {
            return this.DoRequestCommon<GetServiceResponse>(getServiceRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Updates the service.
        /// </summary>
        /// <returns>The service.</returns>
        /// <param name="updateServiceRequest">Update service request.</param>
        public UpdateServiceResponse UpdateService(UpdateServiceRequest updateServiceRequest)
        {
            return this.DoRequestCommon<UpdateServiceResponse>(updateServiceRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Deletes the service.
        /// </summary>
        /// <returns>The service.</returns>
        /// <param name="deleteServiceRequest">Delete service request.</param>
        public DeleteServiceResponse DeleteService(DeleteServiceRequest deleteServiceRequest)
        {
            return this.DoRequestCommon<DeleteServiceResponse>(deleteServiceRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Lists the services.
        /// </summary>
        /// <returns>The services.</returns>
        /// <param name="listServicesRequest">List services request.</param>
        public ListServicesResponse ListServices(ListServicesRequest listServicesRequest)
        {
            return this.DoRequestCommon<ListServicesResponse>(listServicesRequest.GenHttpRequest(Config));
        }
        #endregion service


        #region function
        /// <summary>
        /// Creates the function.
        /// </summary>
        /// <returns>The function.</returns>
        /// <param name="createFunctionRequest">Create function request.</param>
        public CreateFunctionResponse CreateFunction(CreateFunctionRequest createFunctionRequest)
        {
            return this.DoRequestCommon<CreateFunctionResponse>(createFunctionRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Gets the function.
        /// </summary>
        /// <returns>The function.</returns>
        /// <param name="getFunctionRequest">Get function request.</param>
        public GetFunctionResponse GetFunction(GetFunctionRequest getFunctionRequest)
        {
            return this.DoRequestCommon<GetFunctionResponse>(getFunctionRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Gets the function code.
        /// </summary>
        /// <returns>The function code.</returns>
        /// <param name="getFunctionCodeRequest">Get function code request.</param>
        public GetFunctionCodeResponse GetFunctionCode(GetFunctionCodeRequest getFunctionCodeRequest)
        {
            return this.DoRequestCommon<GetFunctionCodeResponse>(getFunctionCodeRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Updates the function.
        /// </summary>
        /// <returns>The function.</returns>
        /// <param name="updateFunctionRequest">Update function request.</param>
        public UpdateFunctionResponse UpdateFunction(UpdateFunctionRequest updateFunctionRequest)
        {
            return this.DoRequestCommon<UpdateFunctionResponse>(updateFunctionRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Deletes the function.
        /// </summary>
        /// <returns>The function.</returns>
        /// <param name="deleteFunctionRequest">Delete function request.</param>
        public DeleteFunctionResponse DeleteFunction(DeleteFunctionRequest deleteFunctionRequest)
        {
            return this.DoRequestCommon<DeleteFunctionResponse>(deleteFunctionRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Lists the functions.
        /// </summary>
        /// <returns>The functions.</returns>
        /// <param name="listFunctionsRequest">List functions request.</param>
        public ListFunctionsResponse ListFunctions(ListFunctionsRequest listFunctionsRequest)
        {
            return this.DoRequestCommon<ListFunctionsResponse>(listFunctionsRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Invokes the function.
        /// </summary>
        /// <returns>The function.</returns>
        /// <param name="invokeFunctionRequest">Invoke function request.</param>
        public InvokeFunctionResponse InvokeFunction(InvokeFunctionRequest invokeFunctionRequest)
        {
            return this.DoRequestCommon<InvokeFunctionResponse>(invokeFunctionRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Invokes the function with http trigger.
        /// </summary>
        /// <returns>The function.</returns>
        /// <param name="httpInvokeFunctionRequest">Http invoke function request.</param>
        public HttpInvokeFunctionResponse InvokeHttpFunction(HttpInvokeFunctionRequest httpInvokeFunctionRequest)
        {
            return this.DoRequestCommon<HttpInvokeFunctionResponse>(httpInvokeFunctionRequest.GenHttpRequest(Config));
        }

        #endregion function


        #region trigger
        /// <summary>
        /// Creates the trigger.
        /// </summary>
        /// <returns>The trigger.</returns>
        /// <param name="createTriggerRequest">Create trigger request.</param>
        public CreateTriggerResponse CreateTrigger(CreateTriggerRequest createTriggerRequest)
        {
            return this.DoRequestCommon<CreateTriggerResponse>(createTriggerRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Gets the trigger.
        /// </summary>
        /// <returns>The trigger.</returns>
        /// <param name="getTriggerRequest">Get trigger request.</param>
        public GetTriggerResponse GetTrigger(GetTriggerRequest getTriggerRequest)
        {
            return this.DoRequestCommon<GetTriggerResponse>(getTriggerRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Updates the trigger.
        /// </summary>
        /// <returns>The trigger.</returns>
        /// <param name="updateTriggerRequest">Update trigger request.</param>
        public UpdateTriggerResponse UpdateTrigger(UpdateTriggerRequest updateTriggerRequest)
        {
            return this.DoRequestCommon<UpdateTriggerResponse>(updateTriggerRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Deletes the trigger.
        /// </summary>
        /// <returns>The trigger.</returns>
        /// <param name="deleteTriggerRequest">Delete trigger request.</param>
        public DeleteTriggerResponse DeleteTrigger(DeleteTriggerRequest deleteTriggerRequest)
        {
            return this.DoRequestCommon<DeleteTriggerResponse>(deleteTriggerRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Lists the triggers.
        /// </summary>
        /// <returns>The triggers.</returns>
        /// <param name="listTriggersRequest">List triggers request.</param>
        public ListTriggersResponse ListTriggers(ListTriggersRequest listTriggersRequest)
        {
            return this.DoRequestCommon<ListTriggersResponse>(listTriggersRequest.GenHttpRequest(Config));
        }
        #endregion trigger


        #region version
        /// <summary>
        /// Publishs the version.
        /// </summary>
        /// <returns>The version.</returns>
        /// <param name="publishVersionRequest">Publish version request.</param>
        public PublishVersionResponse PublishVersion(PublishVersionRequest publishVersionRequest)
        {
            return this.DoRequestCommon<PublishVersionResponse>(publishVersionRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Lists the versions.
        /// </summary>
        /// <returns>The versions.</returns>
        /// <param name="listVersionsRequest">List versions request.</param>
        public ListVersionsResponse ListVersions(ListVersionsRequest listVersionsRequest)
        {
            return this.DoRequestCommon<ListVersionsResponse>(listVersionsRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Deletes the version.
        /// </summary>
        /// <returns>The version.</returns>
        /// <param name="deleteVersionRequest">Delete version request.</param>
        public DeleteVersionResponse DeleteVersion(DeleteVersionRequest deleteVersionRequest)
        {
            return this.DoRequestCommon<DeleteVersionResponse>(deleteVersionRequest.GenHttpRequest(Config));
        }

        #endregion version

        #region alias
        /// <summary>
        /// Creates the alias.
        /// </summary>
        /// <returns>The alias.</returns>
        /// <param name="createAliasRequest">Create alias request.</param>
        public CreateAliasResponse CreateAlias(CreateAliasRequest createAliasRequest)
        {
            return this.DoRequestCommon<CreateAliasResponse>(createAliasRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <returns>The alias.</returns>
        /// <param name="getAliasRequest">Get alias request.</param>
        public GetAliasResponse GetAlias(GetAliasRequest getAliasRequest)
        {
            return this.DoRequestCommon<GetAliasResponse>(getAliasRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Updates the AL ias.
        /// </summary>
        /// <returns>The AL ias.</returns>
        /// <param name="updateAliasRequest">Update alias request.</param>
        public UpdateAliasResponse UpdateALias(UpdateAliasRequest updateAliasRequest)
        {
            return this.DoRequestCommon<UpdateAliasResponse>(updateAliasRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Deletes the alias.
        /// </summary>
        /// <returns>The alias.</returns>
        /// <param name="deleteAliasRequest">Delete alias request.</param>
        public DeleteAliasResponse DeleteAlias(DeleteAliasRequest deleteAliasRequest)
        {
            return this.DoRequestCommon<DeleteAliasResponse>(deleteAliasRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Lists the aliases.
        /// </summary>
        /// <returns>The aliases.</returns>
        /// <param name="listAliasesRequest">List aliases request.</param>
        public ListAliasesResponse ListAliases(ListAliasesRequest listAliasesRequest)
        {
            return this.DoRequestCommon<ListAliasesResponse>(listAliasesRequest.GenHttpRequest(Config));
        }
        #endregion alias


        #region customDomain
        /// <summary>
        /// Creates the custom domain.
        /// </summary>
        /// <returns>The custom domain.</returns>
        /// <param name="createCustomDomainRequest">Create custom domain request.</param>
        public CreateCustomDomainResponse CreateCustomDomain(CreateCustomDomainRequest createCustomDomainRequest)
        {
            return this.DoRequestCommon<CreateCustomDomainResponse>(createCustomDomainRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Gets the custom domain.
        /// </summary>
        /// <returns>The custom domain.</returns>
        /// <param name="getCustomDomainRequest">Get custom domain request.</param>
        public GetCustomDomainResponse GetCustomDomain(GetCustomDomainRequest getCustomDomainRequest)
        {
            return this.DoRequestCommon<GetCustomDomainResponse>(getCustomDomainRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Updates the custom domain.
        /// </summary>
        /// <returns>The custom domain.</returns>
        /// <param name="updateCustomDomainRequest">Update custom domain request.</param>
        public UpdateCustomDomainResponse UpdateCustomDomain(UpdateCustomDomainRequest updateCustomDomainRequest)
        {
            return this.DoRequestCommon<UpdateCustomDomainResponse>(updateCustomDomainRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Deletes the custom domain.
        /// </summary>
        /// <returns>The custom domain.</returns>
        /// <param name="deleteCustomDomainRequest">Delete custom domain request.</param>
        public DeleteCustomDomainResponse DeleteCustomDomain(DeleteCustomDomainRequest deleteCustomDomainRequest)
        {
            return this.DoRequestCommon<DeleteCustomDomainResponse>(deleteCustomDomainRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Lists the custom domains.
        /// </summary>
        /// <returns>The custom domains.</returns>
        /// <param name="listCustomDomainsRequest">List custom domains request.</param>
        public ListCustomDomainsResponse ListCustomDomains(ListCustomDomainsRequest listCustomDomainsRequest)
        {
            return this.DoRequestCommon<ListCustomDomainsResponse>(listCustomDomainsRequest.GenHttpRequest(Config));
        }
        #endregion customDomain

        #region Tag
        /// <summary>
        /// TagResource is an upsert operation. It always updates or adds tags on the given resource.
        /// </summary>
        /// <returns>requestId</returns>
        /// <param name="tagResourceRequest">tag resource request.</param>
        public TagResourceResponse TagResource(TagResourceRequest tagResourceRequest)
        {
            return this.DoRequestCommon<TagResourceResponse>(tagResourceRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Remove tag key-value pair from the given resource.
        /// </summary>
        /// <returns>requestId</returns>
        /// <param name="unTagResourceRequest">tag resource request.</param>
        public UntagResourceResponse UnTagResource(UntagResourceRequest unTagResourceRequest)
        {
            return this.DoRequestCommon<UntagResourceResponse>(unTagResourceRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Get all the tags on the given resource.
        /// </summary>
        /// <returns>dict</returns>
        /// <param name="getResourceTagsRequest">tag resource request.</param>
        public GetResourceTagsResponse GetResourceTags(GetResourceTagsRequest getResourceTagsRequest)
        {
            return this.DoRequestCommon<GetResourceTagsResponse>(getResourceTagsRequest.GenHttpRequest(Config));
        }
        #endregion Tag

        #region reservedCapacity
        /// <summary>
        /// Lists the reserved capacities.
        /// </summary>
        /// <returns>The reserved capacities.</returns>
        /// <param name="listReservedCapacitiesRequest">List reserved capacities request.</param>
        public ListReservedCapacitiesResponse ListReservedCapacities(ListReservedCapacitiesRequest listReservedCapacitiesRequest)
        {
            return this.DoRequestCommon<ListReservedCapacitiesResponse>(listReservedCapacitiesRequest.GenHttpRequest(Config));
        }
        #endregion reservedCapacity

        #region Provision
        /// <summary>
        /// put provision config.
        /// </summary>
        /// <returns>dict</returns>
        /// <param name="putProvisionConfigRequest">List reserved capacities request.</param>
        public PutProvisionConfigResponse PutProvisionConfig(PutProvisionConfigRequest putProvisionConfigRequest)
        {
            return this.DoRequestCommon<PutProvisionConfigResponse>(putProvisionConfigRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// get provision config.
        /// </summary>
        /// <returns>dict</returns>
        /// <param name="getProvisionConfigRequest">List reserved capacities request.</param>
        public GetProvisionConfigResponse GetProvisionConfig(GetProvisionConfigRequest getProvisionConfigRequest)
        {
            return this.DoRequestCommon<GetProvisionConfigResponse>(getProvisionConfigRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// list provision config.
        /// </summary>
        /// <returns>dict</returns>
        /// <param name="listProvisionConfigsRequest">List reserved capacities request.</param>
        public ListProvisionConfigsResponse ListProvisionConfigs(ListProvisionConfigsRequest listProvisionConfigsRequest)
        {
            return this.DoRequestCommon<ListProvisionConfigsResponse>(listProvisionConfigsRequest.GenHttpRequest(Config));
        }
        #endregion Provision

        #region function async config
        /// <summary>
        /// put function async config.
        /// </summary>
        /// <returns>dict</returns>
        /// <param name="putFunctionAsyncConfigRequest">Put function async config request.</param>
        public PutFunctionAsyncConfigResponse PutFunctionAsyncConfig(PutFunctionAsyncConfigRequest putFunctionAsyncConfigRequest)
        {
            return this.DoRequestCommon<PutFunctionAsyncConfigResponse>(putFunctionAsyncConfigRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// get function async config.
        /// </summary>
        /// <returns>dict</returns>
        /// <param name="getFunctionAsyncConfigRequest">Get function async config request.</param>
        public GetFunctionAsyncConfigResponse GetFunctionAsyncConfig(GetFunctionAsyncConfigRequest getFunctionAsyncConfigRequest)
        {
            return this.DoRequestCommon<GetFunctionAsyncConfigResponse>(getFunctionAsyncConfigRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// delete function async config.
        /// </summary>
        /// <param name="deleteFunctionAsyncConfigRequest">Delete function async config request.</param>
        public DeleteFunctionAsyncConfigResponse DeleteFunctionAsyncConfig(DeleteFunctionAsyncConfigRequest deleteFunctionAsyncConfigRequest)
        {
            return this.DoRequestCommon<DeleteFunctionAsyncConfigResponse>(deleteFunctionAsyncConfigRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// list function async configs.
        /// </summary>
        /// <returns>dict</returns>
        /// <param name="listFunctionAsyncConfigsRequest">List function async configs request.</param>
        public ListFunctionAsyncConfigsResponse ListFunctionAsyncConfigs(ListFunctionAsyncConfigsRequest listFunctionAsyncConfigsRequest)
        {
            return this.DoRequestCommon<ListFunctionAsyncConfigsResponse>(listFunctionAsyncConfigsRequest.GenHttpRequest(Config));
        }
        #endregion function async config

        #region instance exec
        /// <summary>
        /// Lists the instances.
        /// </summary>
        /// <returns>The instances.</returns>
        /// <param name="listFunctionsRequest">List instances request.</param>
        public ListInstancesResponse ListInstances(ListInstancesRequest lisInstancesRequest)
        {
            return this.DoRequestCommon<ListInstancesResponse>(lisInstancesRequest.GenHttpRequest(Config));
        }

        /// <summary>
        /// Lists the instances.
        /// </summary>
        /// <returns>The instances.</returns>
        /// <param name="listFunctionsRequest">List instances request.</param>
        public InstanceExecResponse InstanceExec(InstanceExecRequest instanceExecRequest, ExecCallback callback)
        {
            var webPath = Config.Endpoint + instanceExecRequest.GetPath() + "?" + instanceExecRequest.GetQueries();
            webPath = webPath.Replace("http", "ws");

            ClientWebSocket ws = new ClientWebSocket();

            Dictionary<string, string> headers = new Dictionary<string, string> {
                // { "host", this.Config.Host},
                { "date", DateTime.Now.ToUniversalTime().ToString("r")},
                { "user-agent", this.Config.UserAgent},
            };
            if (this.Config.SecurityToken != "")
                headers.Add("x-fc-security-token", this.Config.SecurityToken);
            headers.Add("authorization", instanceExecRequest.GetSign(headers));

            foreach (var header in headers)
                ws.Options.SetRequestHeader(header.Key, header.Value);

            return new InstanceExecResponse(ws, webPath, callback);
        }

        #endregion instance exec

    }
}
