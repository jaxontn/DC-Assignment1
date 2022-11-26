using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Security.Policy;
using System.IO;

namespace Authenticator
{
    internal class Program
    {
        private static AuthenticatorInterface foob;

        //path for registered user
        private static String _path = $"C:\\Downloads\\registered.txt";

        //path for tokens of users that logged in.
        private static String _tokenPath = $"C:\\Downloads\\token.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("---Authentication Service---");
            //This is the actual host service system
            ServiceHost host;
            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            //Bind server to the implementation of DataServer
            host = new ServiceHost(typeof(AuthenticatorImplementation));
            /*Present the publicly accessible interface to the client. 0.0.0.0 tells .net to
            accept on any interface. :8200 means this will use port 8200. AuthenticationService is a name for the
            actual service, this can be any string.*/

            String URL = "net.tcp://localhost:8200/AuthenticationService";
            host.AddServiceEndpoint(typeof(AuthenticatorInterface), tcp, URL);
            //And open the host for business!
            host.Open();
            Console.WriteLine("System Online");
            //Console.ReadLine();



            ConsoleInput();


            

            //Don't forget to close the host after you're done!
            host.Close();
        }

        public static void ConsoleInput()
        {
            //ASK USER IN CONSOLE REGARDING THE MINUTE TO CLEAR TOKEN-----------
            NetTcpBinding tcp = new NetTcpBinding();
            String URL = "net.tcp://localhost:8200/AuthenticationService";
            ChannelFactory<AuthenticatorInterface> foobFactory = new ChannelFactory<AuthenticatorInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();


            Console.WriteLine("ENTER NUMBER OF MINUTE TO CLEAR TOKEN:");
            int minute = Convert.ToInt32(Console.ReadLine());

            foob.SetTime(minute);
            Console.WriteLine(minute + " minutes SET");

            CreateFile(_path);
            CreateFile(_tokenPath);
            Test(); //FOR TESTING

            Console.ReadLine(); //to stop auto close of the window
            //END OF THE TOKEN MINUTE THIGN-------------------------------------
        }


        //JUST FOR TESTING
        public static void Test()
        {
            Console.WriteLine("Register: " + foob.Register("test", "test123"));
            int token = foob.Login("test", "test123");
            Console.WriteLine("Login: " + token);
            Console.WriteLine("Validate: " + foob.Validate(token));
        }


        private static void CreateFile(String thePath)
        {
            TextWriter tw = new StreamWriter(thePath, true);
            tw.WriteLine(""); //insert data
            tw.Close(); //close the file
        }
    }
}
