using Authenticator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthenticatorInterface foob; //For the authenticator service

        //---FOR Display all services and search services functions (Registry)-------------------
        private static String URL = "http://localhost:51114/";
        RestClient client = new RestClient(URL);
        //----------------------------------------------------------------

        



        const int LOGIN = 10;
        const int REGISTER = 20;
        const int NOT_DECIDED_YET = 0;

        private static int decision;

        private int loginToken;

        private static List<DataIntermed> allServiceList;
        private static List<DataIntermed> searchedServiceList;
        private static List<DataIntermed> currentSelectedServiceList;

        private DataIntermed currentServiceData;

        private static String selectedService;



        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<AuthenticatorInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            //Set the URL and crate the connection
            String theURL = "net.tcp://localhost:8200/AuthenticationService";
            foobFactory = new ChannelFactory<AuthenticatorInterface>(tcp, theURL);

            //Create the channel
            foob = foobFactory.CreateChannel();

            decision = NOT_DECIDED_YET;

            login_username.Visibility = Visibility.Hidden;
            login_password.Visibility = Visibility.Hidden;

            register_username.Visibility = Visibility.Hidden;
            register_password.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            decision = LOGIN;

            login_username.Visibility = Visibility.Visible;
            login_password.Visibility = Visibility.Visible;
            register_username.Visibility = Visibility.Hidden;
            register_password.Visibility = Visibility.Hidden;
        }

        private void register_btn_Click(object sender, RoutedEventArgs e)
        {
            decision = REGISTER;

            login_username.Visibility = Visibility.Hidden;
            login_password.Visibility = Visibility.Hidden;
            register_username.Visibility = Visibility.Visible;
            register_password.Visibility = Visibility.Visible;
        }

        private void proceed_btn_Click(object sender, RoutedEventArgs e)
        {
            switch(decision)
            {
                case LOGIN:
                    int token = foob.Login(login_username.Text, login_password.Text);
                    loginToken = token;
                    
                    if (loginToken == -1)
                    {
                        MessageBox.Show("Login failed");
                    }
                    else
                    {
                        MessageBox.Show("Login successful");
                    }
                    break;
                    
                    
                case REGISTER:
                    String result = foob.Register(register_username.Text, register_password.Text);
                    if (result.Equals("Successfully registered"))
                    {
                        MessageBox.Show("---THANK YOU FOR REGISTERING, PLEASE LOGIN NOW---");
                    }
                    else
                    {
                        MessageBox.Show("---YOU ARE REGISTERED, PLEASE LOGIN INSTEAD---");
                    }
                    break;
                    
                    
                default:
                    MessageBox.Show("Please select an option, either LOGIN or REGISTER");
                    break;
            }
        }

        private void showall_btn_Click(object sender, RoutedEventArgs e)
        {
            //clear first after every button click
            AllServiceList.Items.Clear();

            RestRequest request = new RestRequest("api/registry/allservice");
            RestResponse resp = client.Get(request);

            allServiceList = JsonConvert.DeserializeObject<List<DataIntermed>>(resp.Content);

            //for each in the list, add the name
            foreach (DataIntermed data in allServiceList)
            {
                AllServiceList.Items.Add(data.name);
            }

        }

        private void showsearchservice_btn_Click(object sender, RoutedEventArgs e)
        {
            //clear first after every button click
            SearchServiceList.Items.Clear();

            RestRequest request = new RestRequest("api/registry/search/" + search_service.Text);
            RestResponse resp = client.Get(request);

            
            searchedServiceList = JsonConvert.DeserializeObject<List<DataIntermed>>(resp.Content);

            //for all in addServiceList, add the name to the listbox
            foreach (DataIntermed data in searchedServiceList)
            {
                SearchServiceList.Items.Add(data.name);
            }

        }

        private void AllServiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllServiceList.SelectedItem != null)
            {
                selectedService = AllServiceList.SelectedItem.ToString();
                selected_service.Text = "Selected service: " + selectedService;
            }
            else
            {
                selected_service.Text = "Selected service: ";
            }
        
            


            RestRequest request = new RestRequest("api/registry/search/name/" + selectedService);
            RestResponse resp = client.Get(request);

            currentSelectedServiceList = JsonConvert.DeserializeObject<List<DataIntermed>>(resp.Content);

            //get the first one all data
            DataIntermed data = currentSelectedServiceList[0];
            currentServiceData = data;

            //update the GUI boxes
             updateGUIBoxes();
        }

        private void SearchServiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchServiceList.SelectedItem != null)
            {
                selectedService = SearchServiceList.SelectedItem.ToString();
                selected_service.Text = "Selected service: " + selectedService;
            }
            else
            {
                selected_service.Text = "Selected service: ";
            }


            RestRequest request = new RestRequest("api/registry/search/name/" + selectedService);
            RestResponse resp = client.Get(request);

            currentSelectedServiceList = JsonConvert.DeserializeObject<List<DataIntermed>>(resp.Content);

            //get the first one all data
            DataIntermed data = currentSelectedServiceList[0];
            currentServiceData = data;

            //update the GUI Boxes;
            updateGUIBoxes();
        }


           private void updateGUIBoxes()
           {
               //find out how many boxes to display
               int numBoxes = Int32.Parse(currentServiceData.operands);

               switch (numBoxes)
               {
                   case 1:

                       inputOne.Visibility = Visibility.Visible;
                    
                       inputTwo.Visibility = Visibility.Hidden;
                       inputThree.Visibility = Visibility.Hidden;
                       inputFour.Visibility = Visibility.Hidden;
                       inputFive.Visibility = Visibility.Hidden;

                       break;

                   case 2:

                       inputOne.Visibility = Visibility.Visible;
                       inputTwo.Visibility = Visibility.Visible;

                       inputThree.Visibility = Visibility.Hidden;
                       inputFour.Visibility = Visibility.Hidden;
                       inputFive.Visibility = Visibility.Hidden;

                       break;

                   case 3:

                       inputOne.Visibility = Visibility.Visible;
                       inputTwo.Visibility = Visibility.Visible;
                       inputThree.Visibility = Visibility.Visible;

                       inputFour.Visibility = Visibility.Hidden;
                       inputFive.Visibility = Visibility.Hidden;

                       break;

                   case 4:

                        inputOne.Visibility = Visibility.Visible;
                        inputTwo.Visibility = Visibility.Visible;
                        inputThree.Visibility = Visibility.Visible;
                        inputFour.Visibility = Visibility.Visible;
                    
                        inputFive.Visibility = Visibility.Hidden;

                       break;

                   case 5:

                        inputOne.Visibility = Visibility.Visible;
                        inputTwo.Visibility = Visibility.Visible;
                        inputThree.Visibility = Visibility.Visible;
                        inputFour.Visibility = Visibility.Visible;
                        inputFive.Visibility = Visibility.Visible;

                    break;

                   default:

                       break;

               }            
           }


        private void calculate_btn_Click(object sender, RoutedEventArgs e)
        {
            int numBoxes = 0;
            String apiEndpoint = "";
            if (currentServiceData != null)
            {
                //find out how many boxes to display
                numBoxes = Int32.Parse(currentServiceData.operands);
                apiEndpoint = currentServiceData.APIendpoint;

                //replace 'F' to '%'
                apiEndpoint = apiEndpoint.Replace('F', '%');
                apiEndpoint = WebUtility.HtmlDecode(apiEndpoint);
                apiEndpoint = apiEndpoint.Replace("%3A", ":");
                apiEndpoint = apiEndpoint.Replace("%2%", "/");
                MessageBox.Show(apiEndpoint);

                //---FOR geeting the serivces (Service Providr)---------------
                RestClient serviceClient = new RestClient(apiEndpoint);
                //------------------------------------------------------------


                //Make sure the inputs are valid
                switch (numBoxes)
                {
                    case 1:

                        if (!String.IsNullOrEmpty(inputOne.Text))
                        {
                            RestRequest request = new RestRequest("/" + inputOne.Text);
                            RestResponse resp = serviceClient.Get(request);

                            String result = JsonConvert.DeserializeObject<String>(resp.Content); //in JSON String

                            dynamic data = JObject.Parse(result); //Parsing the JSON string data to get the data.

                            MessageBox.Show("Result: " + data.ResultInt);
                        }
                        else
                        {
                            MessageBox.Show("Please input Integer");
                        }
                        break;

                    case 2:

                        if (!String.IsNullOrEmpty(inputOne.Text) && !String.IsNullOrEmpty(inputTwo.Text))
                        {
                            RestRequest request = new RestRequest("/" + inputOne.Text + "/" + inputTwo.Text);
                            RestResponse resp = serviceClient.Get(request);

                            String result = JsonConvert.DeserializeObject<String>(resp.Content); //in JSON String

                            dynamic data = JObject.Parse(result); //Parsing the JSON string data to get the data.

                            MessageBox.Show("Result: " + data.ResultInt);
                        }
                        else
                        {
                            MessageBox.Show("Please input Integer");
                        }
                        break;

                    case 3:

                        if (!String.IsNullOrEmpty(inputOne.Text) && !String.IsNullOrEmpty(inputTwo.Text) && !String.IsNullOrEmpty(inputThree.Text))
                        {
                            RestRequest request = new RestRequest("/" + inputOne.Text + "/" + inputTwo.Text + "/" + inputThree.Text);
                            RestResponse resp = serviceClient.Get(request);

                            String result = JsonConvert.DeserializeObject<String>(resp.Content); //in JSON String


                            dynamic data = JObject.Parse(result); //Parsing the JSON string data to get the data.

                            MessageBox.Show("Result: " + data.ResultInt);
                        }
                        else
                        {
                            MessageBox.Show("Please input Integer");
                        }
                        break;

                    case 4:

                        if (!String.IsNullOrEmpty(inputOne.Text) && !String.IsNullOrEmpty(inputTwo.Text) && !String.IsNullOrEmpty(inputThree.Text) && !String.IsNullOrEmpty(inputFour.Text))
                        {
                            RestRequest request = new RestRequest("/" + inputOne.Text + "/" + inputTwo.Text + "/" + inputThree.Text + "/" + inputFour.Text);
                            RestResponse resp = serviceClient.Get(request);

                            String result = JsonConvert.DeserializeObject<String>(resp.Content); //in JSON String


                            dynamic data = JObject.Parse(result); //Parsing the JSON string data to get the data.

                            MessageBox.Show("Result: " + data.ResultInt);
                        }
                        else
                        {
                            MessageBox.Show("Please input Integer");
                        }
                        break;

                    case 5:

                        if (!String.IsNullOrEmpty(inputOne.Text) && !String.IsNullOrEmpty(inputTwo.Text) && !String.IsNullOrEmpty(inputThree.Text) && !String.IsNullOrEmpty(inputFour.Text) && !String.IsNullOrEmpty(inputFive.Text))
                        {
                            RestRequest request = new RestRequest("/" + inputOne.Text + "/" + inputTwo.Text + "/" + inputThree.Text + "/" + inputFour.Text + "/" + inputFive.Text);
                            RestResponse resp = serviceClient.Get(request);

                            String result = JsonConvert.DeserializeObject<String>(resp.Content); //in JSON String


                            dynamic data = JObject.Parse(result); //Parsing the JSON string data to get the data.

                            MessageBox.Show("Result: " + data.ResultInt);
                        }
                        else
                        {
                            MessageBox.Show("Please input Integer");
                        }
                        break;
                }

            }
            else 
            {
                MessageBox.Show("Please select a service first");
            }    
        }
    }
        
    
}
