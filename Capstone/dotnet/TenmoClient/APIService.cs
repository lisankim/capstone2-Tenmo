﻿using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System;
using TenmoClient.Data;


namespace TenmoClient
{
    public class APIService
    {
        private readonly string API_BASE_URL = "";
        private readonly IRestClient client = new RestClient();
        //AccountController accountController = new AccountController();

        public APIService(string api_url)
        {
            API_BASE_URL = api_url;
        }


        public decimal GetBalance()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "account");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<decimal> response = client.Get<decimal>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }

            return response.Data; // return the Account? 
        }

        // View your past transfers
        public List<Transfer> GetListOfTransfers()
        {
            RestRequest request = new RestRequest(API_BASE_URL + "transfer/all");
            client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }

            return response.Data;
        }
    


        private void ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("Error occurred - unable to reach server.");
            }
            else if (!response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Authorization Require. Please Log In");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    Console.WriteLine("You do not have permissions to do that");
                }
                else
                {
                    Console.WriteLine("Error occurred - received non-success response: " + (int)response.StatusCode);
                }
            }
        }




    }
}
