using System;
using System.Web.Http;
using Newtonsoft.Json;

namespace ServiceProvider.Controllers
{
    [RoutePrefix("api/service")]
    public class ServiceController : ApiController
    {
       // private readonly string _path = $"C:\\Downloads\\SOA_Folder\\serviceresult.txt";

        
        [Route("addTwo/{firstNumber}/{secondNumber}")]
        [Route("addTwo")]
        [HttpGet]
        public String AddTwo(int firstNumber, int secondNumber)
        {
            //validate token first before returning the result.

            //returning a String JSON format back to caller
            return JSONResult(firstNumber + secondNumber);
        }
        
        [Route("addThree/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("addThree")]
        [HttpGet]
        public String AddThree(int firstNumber, int secondNumber, int thirdNumber)
        {
            //validate token first before returning the result.

            //returning a String JSON format back to caller
            return JSONResult(firstNumber + secondNumber + thirdNumber);
        }


        [Route("mulTwo/{firstNumber}/{secondNumber}")]
        [Route("mulTwo")]
        [HttpGet]
        public String MulTwo(int firstNumber, int secondNumber)
        {
            //validate token first before returning the result.

            //returning a String JSON format back to caller
            return JSONResult(firstNumber * secondNumber);
        }


        [Route("mulThree/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("mulThree")]
        [HttpGet]
        public String MulThree(int firstNumber, int secondNumber, int thirdNumber)
        {
            //validate token first before returning the result.

            //returning a String JSON format back to caller
            return JSONResult(firstNumber * secondNumber * thirdNumber);
        }



        private bool ValidateToken(String token)
        {
            ////validate token first when API is called?
            return true;
        }


        //for the JSON, write the output in JSON to a file.
        private String /*JsonResult*//*void*/ JSONResult(int result)
        {

            //Adding the result to a Result class that will be converted into Json.
            var theResult = new Result
            {
                ResultInt = result
            };

            //converting the Json into String, and returning the output in JSON.
            return JsonConvert.SerializeObject(theResult, Formatting.Indented);

        }
    }

    //for the JSON Result model
    public class Result
    {
        public int ResultInt { get; set; }
    }
}
