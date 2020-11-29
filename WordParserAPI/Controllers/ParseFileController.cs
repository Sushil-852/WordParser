using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;
namespace WordParserAPI.Controllers
{
    public class ParseFileController : ApiController
    {
       
        [AllowAnonymous]
        [HttpPost]
        [Route("ParseFile")]
        public HttpResponseMessage ParseFile(string fileURL)
        {
            try
            {
                string parsingFilePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/download/"), "samplingfile.txt");
                WebClient fileDownloadClient = new WebClient();
                fileDownloadClient.DownloadFile(fileURL, parsingFilePath);
                
                IDictionary<string, int> wordCountList =  ReadParsingFile(parsingFilePath);

                SaveParsedList(wordCountList);

                var sortedWordsList = (from entry in wordCountList orderby entry.Value descending select entry).Take(10).ToList();
                var jsonOutput = JsonConvert.SerializeObject(sortedWordsList);
                var response = Request.CreateResponse(HttpStatusCode.OK, "success");
                response.Content = new StringContent(jsonOutput, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Searchword")]
        public HttpResponseMessage Searchword(string searchWord)
        {
            try
            {

                var wordsDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/download/"), "parsedlist.txt")));
                string wordDefinition = String.Empty;
                if (wordsDictionary.ContainsKey(searchWord))
                {
                    wordDefinition = FetchDefinition(searchWord);
                    var response = Request.CreateResponse(HttpStatusCode.OK, "success");
                    response.Content = new StringContent(wordDefinition, Encoding.UTF8, "application/json");
                    return response;
                }
                    
                else
                {
                    var response = Request.CreateResponse(HttpStatusCode.OK, "Success");
                    response.Content = new StringContent("[{\"Success\":\"0\"},{\"Message\":\"No Definition found\"}]", Encoding.UTF8, "application/json");
                    return response;
                }
                   
                
            }
            catch (Exception ex)
            {
                var response = Request.CreateResponse(HttpStatusCode.OK, "Failure");
                response.Content = new StringContent("[{\"Success\":\"0\"},{\"Message\":\"Cannot process your request at the moment\"}]", Encoding.UTF8, "application/json");
                return response;
            }
        }

        private string FetchDefinition(string word)
        {

            WebRequest request = WebRequest.Create((ConfigurationManager.AppSettings["owlAPIURL"] + word));
            request.Method = "GET";
            request.Headers.Add("Authorization", "Token " + ConfigurationManager.AppSettings["accessToken"]);


            var httpResponse = (HttpWebResponse)request.GetResponse();
            string jsonResult = "";
            using (var reader = new StreamReader(httpResponse.GetResponseStream()))
            {

                jsonResult = reader.ReadToEnd(); 
                
            }
            return jsonResult;

        }

        private void SaveParsedList(IDictionary<string, int> wordCountList)
        {
            File.WriteAllText(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/download/"), "parsedlist.txt"), JsonConvert.SerializeObject(wordCountList));
        }

    
        private IDictionary<string, int> ReadParsingFile(string parsingFilePath)
        {
            try {
                FileParser parsingFileObj = new FileParser();
                return parsingFileObj.WordCount(parsingFilePath);
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}
