using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Registry.Controllers
{
    [RoutePrefix("api/registry")]
    public class RegistryController : ApiController
    {
        private String filePath = $"C:\\Downloads\\registry.txt";
        private List<DataIntermed> dataList;


        [Route("publish/{di}")]
        [Route("publish")]
        [HttpPost]
        public String Publish(DataIntermed di)
        {            
            //1. search if the file has been created.
            CheckSetup();

            //get JSON from the file
            GetJSONfromFile();

            //add the new service to the list
            dataList.Add(di);

            //write the list to the file
            UpdateFile();
            
            //save to a file 
            return JsonConvert.SerializeObject(di);
        }


        [Route("search/{description}")]
        [Route("search")]
        [HttpGet]
        public List<DataIntermed>/*String*/ Search(String description)
        {
            //1. check if setup is done
            CheckSetup();

            //2. obtain the file
            GetJSONfromFile();

            //3. search through dataList for desc
            List<DataIntermed> result = new List<DataIntermed>();
            foreach (DataIntermed di in dataList)
            {
                if (di.desc.Contains(description))
                {
                    result.Add(di);
                }
            }


            //return JsonConvert.SerializeObject(result);
            return result;
        }


        [Route("search/name/{name}")]
        [Route("search")]
        [HttpGet]
        public List<DataIntermed>/*String*/ SearchName(String name)
        {
            //1. check if setup is done
            CheckSetup();

            //2. obtain the file
            GetJSONfromFile();

            //3. search through dataList for desc
            List<DataIntermed> result = new List<DataIntermed>();
            foreach (DataIntermed di in dataList)
            {
                if (di.name.Equals(name))
                {
                    result.Add(di);
                }
            }


            //return JsonConvert.SerializeObject(result);
            return result;
        }


        [Route("allservice")]
        [HttpGet]
        public List<DataIntermed>/*String*/ AllService()
        {
            //1. check if setup is done
            CheckSetup();
            
            //2. obtain the file
            GetJSONfromFile();

            //3. Return all services in the registry
            //return JsonConvert.SerializeObject(dataList);
            return dataList;

        }


        [Route("unpublish/{serviceEndpoint}")]
        [Route("unpublish")]
        [HttpPost]
        public bool Unpublish(String serviceEndpoint)
        {
            bool removed = false;

            //IMPORTANT: decode the encoded URL
           // String decodedURL = WebUtility.UrlDecode(serviceEndpoint);


            //1. check if setup is done
            CheckSetup();

            //2. obtain the file
            GetJSONfromFile();

            //3. search through dataList for API endpoint
            foreach (DataIntermed di in dataList)
            {
                if (di.APIendpoint.Equals(/*decodedURL*/serviceEndpoint))
                {
                    dataList.Remove(di);//remove the service
                    UpdateFile(); //update the file
                    return removed = true;
                }
            }

            return removed;
        }

        
        //PRIVATE METHODS-------------------------------------------------

        //NAME: initialSetup
        //IMPORT: none
        //EXPORT: none
        //ASSERTION: initial setup for the registry
        private void CheckSetup()
        {
            //if file doesnt exist, create it.
            if (!File.Exists(filePath))
            {
                //create the file
                File.Create(filePath).Close();
            }
        }



        //NAME: GetJSONfromFile
        //IMPORT: none
        //EXPORT: none
        //ASSERTION: get the JSON from the file and convert it to a list
        private void GetJSONfromFile()
        {
            var jsonData = File.ReadAllText(filePath);

            //deserialize JSON from file or create a new list
            dataList = JsonConvert.DeserializeObject<List<DataIntermed>>(jsonData) ?? new List<DataIntermed>();
        }


        
        //NAME: UpdateFile
        //IMPORT: none
        //EXPORT: none
        //ASSERTION: update the text file
        private void UpdateFile()
        {
            //serialize JSON to a string and then write string to a file
            File.WriteAllText(filePath, JsonConvert.SerializeObject(dataList));
        }
    
    
    }
}
