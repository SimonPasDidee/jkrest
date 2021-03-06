﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using RestSharp;

namespace RestClientJenkins
{
    class JenkinsClient
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string ApiToken { get; set; }

        public JenkinsClient(string url,string user,string token)
        {
            Url = url;
            Username = user;
            ApiToken = token;
        }

        public string GetSecurityCrumb()
        {
            string uri = $"{Url}/crumbIssuer/api/xml";// URL where the XML is available
            var client = new RestClient(uri); // define it as the actual client
            var request = new RestRequest(Method.GET);
            byte[] ua = Encoding.ASCII.GetBytes(Username + ":" + ApiToken); // Encoding username and token in base 64

            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(ua));// adding header to get the xml
            IRestResponse response = client.Execute(request);
            var document = XDocument.Parse(response.Content);// parsing the content of the response in a document

            var crumb = document.Root.Element("crumb").Value;// retrieve the content of the crumb only

            return crumb;
        }

        public void CreateEmptyPipelineJob(string pipelineName)
        {
            string uri = $"{Url}/createItem";
            var client = new RestClient(uri);
            var request = new RestRequest(Method.POST);
            byte[] ua = Encoding.ASCII.GetBytes(Username + ":" + ApiToken);
    
            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(ua));
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", $"name={pipelineName}&mode=org.jenkinsci.plugins.workflow.job.WorkflowJob&Jenkins-Crumb={GetSecurityCrumb()}&undefined=", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
        }

        public void BuildJob(string jobName)
        {
            string uri = $"{Url}/job/{jobName}/build";
            var client = new RestClient(uri);
            var request = new RestRequest(Method.POST);
            byte[] ua = Encoding.ASCII.GetBytes(Username + ":" + ApiToken);

            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(ua));
            request.AddHeader("Jenkins-Crumb", GetSecurityCrumb());

            IRestResponse response = client.Execute(request);
        }

        public void CopyJob(string from,string dest)
        {
            string uri = $"{Url}/createItem";
            var client = new RestClient(uri);
            var request = new RestRequest(Method.POST);
            byte[] ua = Encoding.ASCII.GetBytes(Username + ":" + ApiToken);

            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(ua));
            request.AddParameter("name", dest);
            request.AddParameter("mode", "copy");
            request.AddParameter("from", from);

            IRestResponse response = client.Execute(request);
        }

        //To revise
        public void AbortBuild(string jobName,string buildId)
        {
            string uri = $"{Url}/job/{jobName}/{buildId}/stop";
            var client = new RestClient(uri);
            var request = new RestRequest(Method.POST);
            byte[] ua = Encoding.ASCII.GetBytes(Username + ":" + ApiToken);

            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(ua));
            request.AddHeader("Jenkins-Crumb", GetSecurityCrumb());

            IRestResponse response = client.Execute(request);
        }

        //To revise
        public void ResumePausedPipeline(string jobName,string buildId,string inputId)
        {
            string uri = $"{Url}/job/{jobName}/{buildId}/wfapi/inputSubmit";
            var client = new RestClient(uri);
            var request = new RestRequest(Method.POST);
            byte[] ua = Encoding.ASCII.GetBytes(Username + ":" + ApiToken);

            request.AddParameter("inputId", inputId);

            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(ua));
            request.AddHeader("Jenkins-Crumb", GetSecurityCrumb());

            IRestResponse response = client.Execute(request);
        }

        //To revise
        public void AbortPausedPipeline(string jobName, string buildId, string inputId)
        {
            string uri = $"{Url}/job/{jobName}/{buildId}/input/{inputId}/abort";
            var client = new RestClient(uri);
            var request = new RestRequest(Method.POST);
            byte[] ua = Encoding.ASCII.GetBytes(Username + ":" + ApiToken);

            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(ua));
            request.AddHeader("Jenkins-Crumb", GetSecurityCrumb());

            IRestResponse response = client.Execute(request);

        }

        //To implement
        public void BuildJobWithParameters()
        {

        }

        //To implement
        public void GetPipelinesList()
        {

        }

        //To implement
        public void GetPipelineRuns(string jobName)
        {

        }

        public string GetBuildLog(string jobName,string buildId)
        {
            string uri = $"{Url}/job/{jobName}/{buildId}/logText/progressiveText";
            Console.WriteLine(uri);
            var client = new RestClient(uri);
            var request = new RestRequest(Method.GET);
            byte[] ua = Encoding.ASCII.GetBytes(Username + ":" + ApiToken);

            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(ua));

            request.AddParameter("start", "0");

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

    }

}
