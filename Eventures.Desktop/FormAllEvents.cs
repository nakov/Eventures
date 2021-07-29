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
        private string username;
        private string email;
        private string password;
        private string confirmPassword;
        private string token;

        public FormAllEvents()
        {
            InitializeComponent();
        }

        private void EventBoardForm_Shown(object sender, EventArgs e)
        {
            var formConnect = new FormConnect();
            if (formConnect.ShowDialog() == DialogResult.OK)
            {
                this.apiBaseUrl = formConnect.ApiUrl;
                this.username = formConnect.Username;
                this.email = formConnect.Email;
                this.password = formConnect.Password;
                this.confirmPassword = formConnect.ConfirmPassword;
                LoadEvents();
            }
            else
            {
                this.Close();
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            LoadEvents();
        }

        private async void LoadEvents()
        {
            try
            {
                var restClient = new RestClient(this.apiBaseUrl); // { Timeout = 3000 };

                var registerRequest = new RestRequest("/users/register", Method.POST);
                registerRequest.AddJsonBody(
                    new { Username = $"{username}", 
                        Email = $"{email}", 
                        Password = $"{password}", 
                        ConfirmPassword = $"{confirmPassword}", 
                        FirstName = "Name", 
                        LastName = "Last" });

                var registerResponse = await restClient.ExecuteAsync(registerRequest);
                if (!registerResponse.IsSuccessful)
                    ShowError(registerResponse);

                var loginRequest = new RestRequest("/users/login", Method.POST);
                loginRequest.AddJsonBody(new { Username = $"{username}", Password = $"{password}" });

                var loginResponse = await restClient.ExecuteAsync(loginRequest);

                if (!loginResponse.IsSuccessful)
                    ShowError(loginResponse);

                var jsonResponse = new JsonDeserializer().Deserialize<LoginResponse>(loginResponse);
                token = jsonResponse.Token;

                var request = new RestRequest("/events", Method.GET);
                request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

                ShowMsg("Loading tasks ...");

                var response = await restClient.ExecuteAsync(request);
                if (response.IsSuccessful & response.StatusCode == HttpStatusCode.OK)
                {
                    // Visualize the returned events
                    var events = new JsonDeserializer().Deserialize<List<Event>>(response);
                    ShowSuccessMsg($"Search successful: {events.Count} events loaded.");
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
            if (formCreateEvent.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var restClient = new RestClient(this.apiBaseUrl) { Timeout = 3000 };
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
                    var response = await restClient.ExecuteAsync(request);
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
