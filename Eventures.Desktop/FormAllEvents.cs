using System;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Drawing;
using System.Linq;
using Eventures.Data;

namespace Eventures_Desktop
{
    public partial class FormAllEvents : Form
    {
        private string apiBaseUrl;
        private string token;
        private RestClient restClient;

        public FormAllEvents()
        {
            InitializeComponent();
        }

        private void EventBoardForm_Shown(object sender, EventArgs e)
        {
            // Show the [Connect] form again and agin, until connected
            while (true)
            {
                var formConnect = new FormConnect();
                if (formConnect.ShowDialog() != DialogResult.OK)
                    this.Close();

                // Try to connect to the Web API url
                try
                {
                    this.apiBaseUrl = formConnect.ApiUrl;
                    var homeRequest = new RestRequest("/", Method.GET);
                    this.restClient = new RestClient(this.apiBaseUrl) { Timeout = 10000 };
                    var homeResponse = this.restClient.Execute(homeRequest);
                    if (homeResponse.IsSuccessful)
                    {
                        // Successfully connected to the Web API
                        this.ShowSuccessMsg("Connected to the Web API.");
                        return;
                    }
                    else
                    {
                        ShowError(homeResponse);
                    }
                } 
                catch (Exception ex)
                {
                    ShowErrorMsg(ex.Message);
                }
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            var formRegister = new FormRegister();
            if (formRegister.ShowDialog() != DialogResult.OK)
                return;

            this.Register(formRegister.Username, formRegister.Email, formRegister.Password, formRegister.ConfirmPassword /* firstName, lastName */);
        }

        private async void Register(string username, string email, 
            string password, string confirmPassword)
        {
            var registerRequest = new RestRequest("/users/register", Method.POST);
            registerRequest.AddJsonBody(
                new
                {
                    Username = $"{username}",
                    Email = $"{email}",
                    Password = $"{password}",
                    ConfirmPassword = $"{confirmPassword}",
                    FirstName = "Name", // TODO: add "First Name" in the register form ...
                    LastName = "Last"   // TODO: add "Last Name" in the register form ...
                });

            var registerResponse = await this.restClient.ExecuteAsync(registerRequest);
            if (!registerResponse.IsSuccessful)
            {
                ShowError(registerResponse);
                return;
            }

            this.ShowSuccessMsg($"User `{username}` registered.");

            this.Login(username, password);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            // Show the login form
            var username = "nakov";
            var password = "parola1";

            this.Login(username, password);
        }

        private async void Login(string username, string password)
        {
            var loginRequest = new RestRequest("/users/login", Method.POST);
            loginRequest.AddJsonBody(new { Username = $"{username}", Password = $"{password}" });

            var loginResponse = await this.restClient.ExecuteAsync(loginRequest);

            if (!loginResponse.IsSuccessful)
            {
                ShowError(loginResponse);
                return;
            }

            var jsonResponse = new JsonDeserializer().Deserialize<LoginResponse>(loginResponse);
            this.token = jsonResponse.Token;

            this.ShowSuccessMsg($"User `{username}` successfully logged-in.");

            this.LoadEvents();
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            LoadEvents();
        }

        private async void LoadEvents()
        {
            try
            {
                var request = new RestRequest("/events", Method.GET);
                request.AddParameter("Authorization", 
                    "Bearer " + this.token, ParameterType.HttpHeader);

                ShowMsg("Loading events ...");

                var response = await this.restClient.ExecuteAsync(request);
                if (response.IsSuccessful & response.StatusCode == HttpStatusCode.OK)
                {
                    // Visualize the returned events
                    var events = new JsonDeserializer().Deserialize<List<Event>>(response);
                    ShowSuccessMsg($"Load successful: {events.Count} events loaded.");
                    DisplayEventsInListView(events);
                }
                else
                    ShowError(response);
            }
            catch (Exception ex)
            {
                ShowErrorMsg(ex.Message);
            }
        }

        private void DisplayEventsInListView(List<Event> events)
        {
            this.listViewEvents.Clear();

            // Create column headers
            var headers = new ColumnHeader[] {
                new ColumnHeader { Text = "Id", Width = 50 },
                new ColumnHeader { Text = "Name", Width = 100 },
                new ColumnHeader { Text = "Place", Width = 100 },
                new ColumnHeader { Text = "Start", Width = 200 },
                new ColumnHeader { Text = "End", Width = 200 },
                new ColumnHeader { Text = "Tickets", Width = 100 },
                new ColumnHeader { Text = "Price", Width = 100 },
                new ColumnHeader { Text = "Owner", Width = 100 }
            };
            this.listViewEvents.Columns.AddRange(headers);

            // Add items and groups to the ListView control
            var groups = new Dictionary<string, ListViewGroup>();
            foreach (var ev in events)
            {
                var item = new ListViewItem(new string[] {
                    "" + ev.Id, 
                    ev.Name, 
                    ev.Place, 
                    ev.Start.ToString(),
                    ev.End.ToString(),
                    ev.TotalTickets.ToString(),
                    ev.PricePerTicket.ToString(),
                    ev.Owner.UserName
                });
                this.listViewEvents.Items.Add(item);
            }

            var sortedGroups = groups.Values.OrderBy(g => (int)g.Tag).ToArray();
            this.listViewEvents.Groups.AddRange(sortedGroups);
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            var formCreateEvent = new FormCreateEvent();
            if (formCreateEvent.ShowDialog() != DialogResult.OK)
                return;

            // TODO: extract in a separate method
            try
            {
                var request = new RestRequest("/events/create", Method.POST);
                request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
                request.AddJsonBody(new
                {
                    name = formCreateEvent.EvName,
                    place = formCreateEvent.Place,
                    start = formCreateEvent.Start,
                    end = formCreateEvent.End,
                    totalTickets = formCreateEvent.TotalTickets,
                    pricePerTicket = formCreateEvent.PricePerTicket
                });
                ShowMsg($"Creating new event ...");
                var response = await this.restClient.ExecuteAsync(request);
                if (response.IsSuccessful & response.StatusCode == HttpStatusCode.Created)
                {
                    ShowSuccessMsg($"Event created.");
                    LoadEvents();
                }
                else
                    ShowError(response);
            }
            catch (Exception ex)
            {
                ShowErrorMsg(ex.Message);
            }
        }

        private void ShowError(IRestResponse response)
        {
            if (string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                string errText = $"HTTP error `{response.StatusCode}`.";
                if (!string.IsNullOrWhiteSpace(response.Content))
                    errText += $" Details: {response.Content}";
                ShowErrorMsg(errText);
            }
            else
                ShowErrorMsg($"HTTP error `{response.ErrorMessage}`.");
        }

        private void ShowMsg(string msg)
        {
            toolStripStatusLabel.Text = msg;
            toolStripStatusLabel.ForeColor = SystemColors.ControlText;
            toolStripStatusLabel.BackColor = SystemColors.Control;
        }

        private void ShowSuccessMsg(string msg)
        {
            toolStripStatusLabel.Text = msg;
            toolStripStatusLabel.ForeColor = Color.White;
            toolStripStatusLabel.BackColor = Color.Green;
        }

        private void ShowErrorMsg(string errMsg)
        {
            toolStripStatusLabel.Text = $"Error: {errMsg}";
            toolStripStatusLabel.ForeColor = Color.White;
            toolStripStatusLabel.BackColor = Color.Red;
        }
    }
}
