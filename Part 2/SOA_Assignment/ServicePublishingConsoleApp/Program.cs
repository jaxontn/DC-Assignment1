
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicePublishingConsoleApp
{
    internal class Program
    {
        private static ServicePublishingConsoleAppInterface foob;
        private static int token = -1;

        


        static void Main(string[] args)
        {
            Console.WriteLine("---Service Publishing Console App---");

            //This is the actual host service system
            ServiceHost host;

            //This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();

            //Bind server to the implementation of DataServer
            host = new ServiceHost(typeof(ServicePublishingConsoleAppImplementation));

            String URL = "net.tcp://localhost:8100/ServicePublishingConsoleAppService";

            host.AddServiceEndpoint(typeof(ServicePublishingConsoleAppInterface), tcp, URL);


            //Amd open the host for business!
            host.Open();
            Console.WriteLine("System Online");

            ConsoleInput();


            Console.ReadLine(); //to stop auto close of the window

            host.Close();
        }


        public static void ConsoleInput()
        {
            //ASK USER IN CONSOLE REGARDING THE MINUTE TO CLEAR TOKEN-----------
            NetTcpBinding tcp = new NetTcpBinding();
            String URL = "net.tcp://localhost:8100/ServicePublishingConsoleAppService";
            ChannelFactory<ServicePublishingConsoleAppInterface> foobFactory = new ChannelFactory<ServicePublishingConsoleAppInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();


            Console.WriteLine("Enter (1) to Register, or enter (2) to Login:");

            int decision = 0;
            try
            {

                decision = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine("---PLEASE ENTER INTEGER ONLY, START AGAIN---");
                ConsoleInput();
            }

            switch (decision)
            {
                case 1:

                    if (Register().Equals("Successfully registered"))
                    {
                        Console.WriteLine("---THANK YOU FOR REGISTERING, PLEASE LOGIN NOW---");
                        token = Login();
                    }
                    else
                    {
                        Console.WriteLine("---YOU ARE REGISTERED, PLEASE LOGIN INSTEAD---");
                        token = Login();
                    }
                    
                    break;
                    
                case 2:

                    Console.WriteLine("---LOGIN PAGE---");
                    token = Login();
                    break;
                    
                default:
                    Console.WriteLine("Invalid input");
                    Console.WriteLine("---START AGAIN---");
                    ConsoleInput();
                    break;
            }

            AskPublishOrUnpublish(); //Time to ask if user want to publish or unpublish

   
            Console.ReadLine(); //to stop auto close of the window
            //END OF THE TOKEN MINUTE THIGN----------------------------
        }



        private static int Login()
        {
            Console.WriteLine("Enter your name:");
            String name2 = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            String password2 = Console.ReadLine();
            int result2 = foob.Login(name2, password2);
            Console.WriteLine("result: " + result2);

            if (result2 == -1)
            {
                Console.WriteLine("---!!!LOGIN FAILED, CREDENTIALS NO MATCH!!! PLEASE TRY AGAIN---");
                Login();
            }
            else
            {
                Console.WriteLine("---LOGIN SUCCESSFUL---");
                
            }
            
            return result2; //return the token
        }



        private static String Register()
        {
            Console.WriteLine("Enter your name:");
            String name = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            String password = Console.ReadLine();
            String result = foob.Register(name, password);
            Console.WriteLine("result: " + result);
            return result; //return the result whether registered or not.
        }


        private static void AskPublishOrUnpublish()
        {
            if (token != -1) //means valid token
            {
                Console.WriteLine("Enter (1) to Publish, or enter (2) to Unpublish:");

                int decision = 0;
                try
                {
                    decision = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException e)
                {
                    Console.WriteLine("---PLEASE ENTER INTEGER ONLY, START AGAIN---");
                    ConsoleInput();
                }


                //SWTICH CASE STATEMENT
                switch (decision)
                {
                    case 1:
                        Console.WriteLine("---PUBLISH PAGE---");
                        Publish();
                        AskPublishOrUnpublish();
                        break;

                    case 2:
                        Console.WriteLine("---UNPUBLISH PAGE---");
                        Unpublish();
                        AskPublishOrUnpublish();
                        break;

                    default:
                        Console.WriteLine("Invalid input");
                        Console.WriteLine("---START AGAIN---");
                        AskPublishOrUnpublish();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Hmmm...Something is wrong with your login token, please contact your developer :(");
            }
            
            
        }


        private static void Publish()
        {
            Console.WriteLine("Enter service name:");
            String name = Console.ReadLine();
            
            Console.WriteLine("Enter service description:");
            String description = Console.ReadLine();

            Console.WriteLine("Enter service API Endpoint:");
            String apiEndpoint = Console.ReadLine();
            
            Console.WriteLine("Enter number of operands:");
            String numOperands = Console.ReadLine();

            Console.WriteLine("Enter operand type:");
            String operandType = Console.ReadLine();


            DataIntermed di = foob.Publish(name, description, apiEndpoint, numOperands, operandType, token);

            if (di != null)
            {
                Console.WriteLine("---PUBLISH SUCCESSFUL---");
                Console.WriteLine("Service Name: " + di.name);
                Console.WriteLine("Service Description: " + di.desc);
                Console.WriteLine("Service API Endpoint: " + di.APIendpoint);
                Console.WriteLine("Service Number of Operands: " + di.operands);
                Console.WriteLine("Service Operand Type: " + di.operandType);
                Console.WriteLine("---With Login Token: " + token + "---");
            }
            else
            {
                Console.WriteLine("---PUBLISH FAILED---");
            }

        }



        private static void Unpublish()
        {
            Console.WriteLine("Enter serivce API Endpoint:");
            String apiEndpoint = Console.ReadLine();

            foob.Unpublish(apiEndpoint, token);

            Console.WriteLine("---Called Unpublised service---");
        }
    }
}
