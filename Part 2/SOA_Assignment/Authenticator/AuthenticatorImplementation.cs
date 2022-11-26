using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Authenticator
{
    internal class AuthenticatorImplementation : AuthenticatorInterface
    {
        //path for registered user
        private String _path = $"C:\\Downloads\\registered.txt";

        //path for tokens of users that logged in.
        private String _tokenPath = $"C:\\Downloads\\token.txt";

        //thread for the minutes to clear token
       // private Thread threadOne;


        //NAME: Register
        //IMPORTS: name (String), password (String)
        //EXPORTS: message (String)
        //ASSERTIONS: Handles registrations, check if its already registered.
        public String Register(String name, String password)
        {
            String message = "Already registered";
            if(!IsRegistered(name, password)) //if not registered, save new data to file
            {
                TextWriter tw = new StreamWriter(_path, true); //append data to file, no overwrite
                tw.WriteLine(name + "," + password); //insert data
                tw.Close(); //close the file
                message = "Successfully registered";
            }
            return message;
        }

        
        
        //NAME: Login
        //IMPORTS: name (String), password (String)
        //EXPORTS: token (Integer)
        //ASSERTIONS: Handles the login, creates the token if user is registered,
        //            Returns the token to the caller.
        public int Login(String name, String password)
        {
            Random rdm = new Random();
            int token = -1; //means not registered.

            if (IsRegistered(name, password)) //check if already registered
            {
                token = rdm.Next(1000, 2000); //create a random integer token
                SaveToken(token); //save token to a file
            }
            return token;
        }

        


        //NAME: Validate 
        //IMPORTS: token (Integer)
        //EXPORTS: message (String)
        //ASSERTIONS: Checks whether the token is already generated.
        public String Validate(int token)
        {
            String message = "Not validated";

            if (HasToken(token))
            {
                message = "Validated";
            }

            return message;
        }



        //NAME: SetTime
        //IMPORTS: minutes (Integer)
        //EXPORTS: none
        //ASSERTIONS: This will allow the console to state the time taken to clear the
        //            token file for every cycle.
        public void SetTime(int minutes)
        {
            //threadOne = Thread.CurrentThread;

        }



        //INTERNAL METHOD--------------------------------------------------------------------
        //NAME: ClearToken
        //IMPROTS: 
        //EXPORTS:
        //ASSERTIONS: It will constantly clear the token file every 'x' minutes.
        internal void ClearToken(int minutes)
        {
            //inifinite loop
            while (true)
            { 
                //clear the file after minutes
            }
        }




        //PRIVATE METHODS--------------------------------------------------------------------
        private bool IsRegistered(String name, String password)
        {
            bool registered = false;

            StreamReader reader = File.OpenText(_path);
            String line;


            while ((line = reader.ReadLine()) != null)
            {
                String[] data = line.Split(',');
                if (data[0].Equals(name) && data[1].Equals(password)) //if match
                {
                    registered = true; //say it is registered.
                }
            }

            reader.Close();

            return registered;
        }

        private bool SaveToken(int token)
        {
            bool done = false;

            TextWriter tw = new StreamWriter(_tokenPath, true); //append data to file, no overwrite
            tw.WriteLine(token); //insert data
            tw.Close(); //close the file
            done = true;

            return done;
        }


        private bool HasToken(int token)
        {
            bool hasToken = false;

            StreamReader reader = File.OpenText(_tokenPath);
            String line;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Equals(token.ToString())) //if match
                {
                    hasToken = true; //say has the token
                }
            }

            reader.Close();
            return hasToken;
        }


        
        //-----------------------------------------------------------------------
    }
}
