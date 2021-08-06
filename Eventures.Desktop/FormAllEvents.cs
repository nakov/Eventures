using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Drawing;
using System.Linq;
using Eventures.Desktop.Data;

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
            this.toolStripStatusLabel.TextChanged += ToolStripStatusLabel_TextChanged;
        }

        private void ToolStripStatusLabel_TextChanged(object sender, EventArgs e)
        {
            // Intentionally copy the "status label text" into the "accessibility text"
            // to allow the runtime UI inspectors to read the text
            this.toolStripStatusLabel.AccessibleName = toolStripStatusLabel.Text;
        }

        private void EventBoardForm_Shown(object sender, EventArgs e)
        {
            // Show the [Connect] form again and agin, until connected
            var connected = false;
            while (connected == false)
            {
                var formConnect = new FormConnect();
                if (formConnect.ShowDialog() != DialogResult.OK)
                {
                    this.Close();
                    break;
                } 
                connected = this.Connect(formConnect.ApiUrl);
            }
        }

        private bool Connect(string apiUrl)
        {
            try
            {
                // Try to connect to the Web API url
                this.apiBaseUrl = apiUrl;
                var homeRequest = new RestRequest("/", Method.GET);
                this.restClient = new RestClient(this.apiBaseUrl) { Timeout = 5000 };
                var homeResponse = this.restClient.Execute(homeRequest);
                if (!homeResponse.IsSuccessful)
                {
                    this.ShowError(homeResponse);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMsg(ex.Message);
                return false;
            }

            // Successfully connected to the Web API
            this.ShowSuccessMsg("Connected to the Web API.");
            return true;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            var formRegister = new FormRegister();
            if (formRegister.ShowDialog() != DialogResult.OK)
                return;

            this.Register(
                formRegister.Username,
                formRegister.Email,
                formRegister.Password,
                formRegister.ConfirmPassword,
                formRegister.FirstName,
                formRegister.LastName);
        }

        private async void Register(string username, string email,
            string password, string confirmPassword, string firstName, string lastName)
        {
            var registerRequest = new RestRequest("/users/register", Method.POST);
            registerRequest.AddJsonBody(
                new
                {
                    Username = $"{username}",
                    Email = $"{email}",
                    Password = $"{password}",
                    ConfirmPassword = $"{confirmPassword}",
                    FirstName = firstName,
                    LastName = lastName
                });

            try
            {
                var registerResponse = await this.restClient.ExecuteAsync(registerRequest);
                if (!registerResponse.IsSuccessful)
                {
                    this.ShowError(registerResponse);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMsg(ex.Message);
                return;
            }

            this.ShowSuccessMsg($"User `{username}` registered.");
            this.Login(username, password);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            var formLogin = new FormLogin();
            if (formLogin.ShowDialog() != DialogResult.OK)
                return;

            this.Login(
                formLogin.Username,
                formLogin.Password);
        }

        private async void Login(string username, string password)
        {
            var loginRequest = new RestRequest("/users/login", Method.POST);
            loginRequest.AddJsonBody(new { Username = $"{username}", Password = $"{password}" });

            try
            {
                var loginResponse = await this.restClient.ExecuteAsync(loginRequest);

                if (!loginResponse.IsSuccessful)
                {
                    this.ShowError(loginResponse);
                    return;
                }

                var jsonResponse = new JsonDeserializer().Deserialize<LoginResponse>(loginResponse);
                this.token = jsonResponse.Token;
            }
            catch (Exception ex)
            {
                ShowErrorMsg(ex.Message);
                return;
            }

            this.ShowSuccessMsg($"User `{username}` successfully logged-in.");
            this.LoadEvents();
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            this.LoadEvents();
        }

        private async void LoadEvents()
        {
            var request = new RestRequest("/events", Method.GET);
            request.AddParameter("Authorization",
                "Bearer " + this.token, ParameterType.HttpHeader);

            ShowMsg("Loading events ...");

            try
            {
                var response = await this.restClient.ExecuteAsync(request);
                if (!response.IsSuccessful)
                {
                    this.ShowError(response);
                    return;
                }

                // Visualize the returned events
                var events = new JsonDeserializer().Deserialize<List<Event>>(response);
                ShowSuccessMsg($"Load successful: {events.Count} events loaded.");
                DisplayEventsInListView(events);
            }
            catch (Exception ex)
            {
                ShowErrorMsg(ex.Message);
                return;
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

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            var formCreateEvent = new FormCreateEvent();
            if (formCreateEvent.ShowDialog() != DialogResult.OK)
                return;

            this.Create(formCreateEvent.EvName,
                formCreateEvent.Place,
                formCreateEvent.Start,
                formCreateEvent.End,
                formCreateEvent.TotalTickets,
                formCreateEvent.PricePerTicket);
        }

        private async void Create(string name, string place, DateTime start,
            DateTime end, int totalTickets, decimal pricePerTicket)
        {
            var request = new RestRequest("/events/create", Method.POST);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            request.AddJsonBody(new
            {
                name = name,
                place = place,
                start = start,
                end = end,
                totalTickets = totalTickets,
                pricePerTicket = pricePerTicket
            });
            ShowMsg($"Creating new event ...");
            var response = await this.restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                this.ShowError(response);
                return;
               
            }

            this.ShowSuccessMsg($"Event created.");
            this.LoadEvents();
        }

        private void ShowError(IRestResponse response)
        {
            if (string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                string errText = $"HTTP error `{response.StatusCode}`.";
                if (!string.IsNullOrWhiteSpace(response.Content))
                    errText += $" Details: {response.Content}";
                this.ShowErrorMsg(errText);
            }
            else
                this.ShowErrorMsg($"HTTP error `{response.ErrorMessage}`.");
        }

        private void ShowMsg(string msg)
        {
            this.toolStripStatusLabel.Text = msg;
            this.toolStripStatusLabel.ForeColor = SystemColors.ControlText;
            this.toolStripStatusLabel.BackColor = SystemColors.Control;
        }

        private void ShowSuccessMsg(string msg)
        {
            this.toolStripStatusLabel.Text = msg;
            this.toolStripStatusLabel.ForeColor = Color.White;
            this.toolStripStatusLabel.BackColor = Color.Green;
        }

        private void ShowErrorMsg(string errMsg)
        {
            this.toolStripStatusLabel.Text = $"Error: {errMsg}";
            this.toolStripStatusLabel.ForeColor = Color.White;
            this.toolStripStatusLabel.BackColor = Color.Red;
        }
    }
}
