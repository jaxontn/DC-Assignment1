using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicePublishingConsoleApp
{
    [ServiceContract]
    public interface ServicePublishingConsoleAppInterface
    {
        [OperationContract]
        String Register(String name, String password);

        [OperationContract]
        int Login(String name, String password);

        [OperationContract]
        DataIntermed Publish(String name, String description, String APIendpoint, String NumOfOperands, String operandType, int token);

        [OperationContract]
        void Unpublish(String APIendpoint, int token);
    }
}
