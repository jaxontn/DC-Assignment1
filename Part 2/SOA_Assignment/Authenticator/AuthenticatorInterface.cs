using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator
{
    [ServiceContract]
    public interface AuthenticatorInterface
    {
        [OperationContract]
        String Register(String name, String password);
       
        [OperationContract]
        int Login(String name, String password);

        [OperationContract]
        String Validate(int token);

        [OperationContract]
        void SetTime(int minutes);
    }
}