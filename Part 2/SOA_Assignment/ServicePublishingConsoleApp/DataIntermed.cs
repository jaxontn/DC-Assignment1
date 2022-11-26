using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePublishingConsoleApp
{
    //We are using these object as templates for the .NET JSON serializers,
    //so we dont have to do any work with the JSON itself.
    public class DataIntermed
    {
        public String name;
        public String desc; //description
        public String APIendpoint;
        public String operands; //number of operands
        public String operandType;

    }
}
