using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System.Configuration;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using System.Net;
using System;

namespace UsingXRMToolKit
{
    class Program
    {
        private static OrganizationServiceProxy _serviceProxy;

        private static IOrganizationService _service;
        static void Main(string[] args)
        {
            /*var connectionString = ConfigurationManager.ConnectionStrings["Xrm"].ConnectionString;
            var crmConn = new CrmServiceClient(connectionString);

            using (_serviceProxy = crmConn.OrganizationServiceProxy)
            {

                _service = _serviceProxy;

                var reqWhoAmI = new WhoAmIRequest();

                var resp = (WhoAmIResponse)_service.Execute(reqWhoAmI);

                var buID = resp.OrganizationId.ToString();

                var userID = resp.UserId.ToString();
            }*/

            string connectionString = ConfigurationManager.ConnectionStrings["Xrm"].ConnectionString;
            CrmServiceClient conn = new CrmServiceClient(connectionString);

            OrganizationServiceProxy orgService = conn.OrganizationServiceProxy;

            // To impersonate, set the caller GUID
            orgService.CallerId = new Guid("67ab9cb6-96c3-4849-8cab-6c2135793f06");

            // Create Account

            Entity account = new Entity("account");
            account["name"] = "Satya Parida";
            Guid accountId = orgService.Create(account);

            // Who Am I Request

            // Guid userid = ((WhoAmIResponse)orgService.Execute(new WhoAmIRequest())).UserId;

            Console.ReadLine();
        }

    }

    // <summary>
    /// Insert records to Dynamics Entity
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    /*public string PostToCrm()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        CrmServiceClient service = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRMConnection"].ConnectionString);
        Entity leadRecord = new Entity("new_businesscardlead");


        leadRecord.Attributes.Add("new_name", "");
        leadRecord.Attributes.Add("new_company", "");
        leadRecord.Attributes.Add("new_title", "");
        leadRecord.Attributes.Add("new_citystatezip", "");
        leadRecord.Attributes.Add("new_email", "");
        leadRecord.Attributes.Add("new_website", "");
        leadRecord.Attributes.Add("new_facebook", "");
        leadRecord.Attributes.Add("new_twitter", "");
        leadRecord.Attributes.Add("new_phone", "");
        leadRecord.Attributes.Add("new_fax", "");
        leadRecord.Attributes.Add("new_cell", "");

        var guid = service.Create(leadRecord);
        string id = guid.ToString();
        string response = $"Lead has been created for: lead Id : {id}.";
        return response;
    }*/
}
