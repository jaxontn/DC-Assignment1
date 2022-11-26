using Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Net;
//using System.Web;

namespace ServicePublishingConsoleApp
{
    internal class ServicePublishingConsoleAppImplementation : ServicePublishingConsoleAppInterface
    {
        private AuthenticatorInterface foob; //For the authenticator service

        //---FOR Publising and Unplublising functions (Registry)-------------------
        private static String URL = "http://localhost:51114/";
        RestSharp.RestClient client = new RestSharp.RestClient(URL);
        //----------------------------------------------------------------
        
        


        public ServicePublishingConsoleAppImplementation()
        {
            ChannelFactory<AuthenticatorInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            //Set the URL and crate the connection
            String theURL = "net.tcp://localhost:8200/AuthenticationService";
            foobFactory = new ChannelFactory<AuthenticatorInterface>(tcp, theURL);

            //Create the channel
            foob = foobFactory.CreateChannel();

        }

        public String Register(String name, String password) //connects to authenticator project
        {
            return foob.Register(name, password);
        }


        public int Login(String name, String password) //connects to authenticator project
        {
            return foob.Login(name, password);
        }


        //connects to registry project
        public DataIntermed Publish(String name, String description, String APIendpoint, String NumOfOperands, String operandType, int token)
        {
            // RestRequest request = new RestRequest("api/registry/publish/" + name + "/" + description + "/" + APIendpoint + "/" + NumOfOperands + "/" + operandType);

            String encodedAPIendpoint = WebUtility.UrlEncode(APIendpoint); //NEW
            DataIntermed di = new DataIntermed();
            di.name = name;
            di.desc = description;
            di.APIendpoint = encodedAPIendpoint.Replace('%', 'F');
            di.operands = NumOfOperands;
            di.operandType = operandType;

            RestRequest request = new RestRequest("api/registry/publish/");
            request.AddJsonBody(di); //serialize object to JSON format

            IRestResponse resp = client.Post(request);
            //do the request
            // IRestResponse resp = client.Get(request);

            //use JSON Deserializer to deserialize our object back to the class.
            //    DataIntermed result = JsonConvert.DeserializeObject<DataIntermed>(resp.Content);

            //String s = resp.Content;

            //DataIntermed result = JsonConvert.DeserializeObject<DataIntermed>(s);
            return di;
        }


        //connects to registry project
        public void Unpublish(String APIendpoint, int token)
        {
            //IMPORTANT: you need to encode the url before passed to the api url
            String encode = WebUtility.UrlEncode(APIendpoint);

            //must replace symbols in the encoded url string, otherwise unable to delete
            RestRequest request = new RestRequest("api/registry/unpublish/" + encode.Replace('%', 'F'));

            //do the request
            IRestResponse resp = client.Post(request); //the request return is not JSON

            //use JSON Deserializer to deserialize our object back to the class.
            //bool result = JsonConvert.DeserializeObject<bool>(resp.Content);

            //return result;
        }
    }
}
