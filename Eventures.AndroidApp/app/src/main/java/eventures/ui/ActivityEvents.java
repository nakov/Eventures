package eventures.ui;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import eventures.data.Event;
import eventures.data.EventReponse;
import eventures.data.EventuresAPI;
import eventures.data.EventuresUser;
import eventures.data.LoginResponse;
import com.example.eventures.android.R;
import eventures.data.UserLoginModel;

import java.net.HttpURLConnection;
import java.util.List;
import java.util.concurrent.TimeUnit;

import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ActivityEvents extends AppCompatActivity {
    private static final int REQUEST_CODE_CREATE_TASK = 1001;
    private static final int REQUEST_CODE_LOGIN = 1002;
    private static final int REQUEST_CODE_REGISTER = 1003;
    private OkHttpClient client;
    private String apiBaseUrl;
    private TextView textViewStatus;
    private String token = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_events);

        client = new OkHttpClient.Builder()
                .connectTimeout(30, TimeUnit.SECONDS)
                .writeTimeout(10, TimeUnit.SECONDS)
                .readTimeout(30, TimeUnit.SECONDS)
                .build();

        this.apiBaseUrl = this.getIntent().getStringExtra("paramApiBaseUrl");

        this.textViewStatus = findViewById(R.id.textViewStatus);

        Button buttonLogin = findViewById(R.id.buttonLogin);
        buttonLogin.setOnClickListener(v -> {
            //hideSoftKeyboard(this);
            Intent intent = new Intent(this, LoginActivity.class);
            startActivityForResult(intent, REQUEST_CODE_LOGIN);
        });

        Button buttonRegister = findViewById(R.id.buttonRegister);
        buttonRegister.setOnClickListener(v -> {
            //hideSoftKeyboard(this);
            Intent intent = new Intent(this, RegisterActivity.class);
            startActivityForResult(intent, REQUEST_CODE_REGISTER);
        });

        Button buttonAdd = findViewById(R.id.buttonAdd);
        buttonAdd.setOnClickListener(v -> {
            //hideSoftKeyboard(this);
            Intent intent = new Intent(this, AddEventActivity.class);
            startActivityForResult(intent, REQUEST_CODE_CREATE_TASK);
        });

        Button buttonReload = findViewById(R.id.buttonReload);
        buttonReload.setOnClickListener(v -> {
            //hideSoftKeyboard(this);
            getEvents();
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == REQUEST_CODE_CREATE_TASK && resultCode == RESULT_OK) {
            String name = data.getStringExtra("name");
            String place = data.getStringExtra("place");
            String start = data.getStringExtra("start");
            String end = data.getStringExtra("end");
            String tickets = data.getStringExtra("totalTickets");
            String price = data.getStringExtra("pricePerTicket");
            createEvent(name, place, start, end, tickets, price);
        }
        if (requestCode == REQUEST_CODE_LOGIN && resultCode == RESULT_OK) {
            String username = data.getStringExtra("username");
            String password = data.getStringExtra("password");
            authorize(username, password);
        }
        if (requestCode == REQUEST_CODE_REGISTER && resultCode == RESULT_OK) {
            String userName = data.getStringExtra("username");
            String email = data.getStringExtra("email");
            String password = data.getStringExtra("password");
            String confirmPassword = data.getStringExtra("confirmPassword");
            String firstName = data.getStringExtra("firstName");
            String lastName = data.getStringExtra("lastName");
            registerUser(userName, email, password, confirmPassword, firstName, lastName);
        }
    }

    private void authorize(String username, String password) {
        showStatusMsg("Authorization ...");

        Retrofit retrofit = new Retrofit.Builder()
                .baseUrl(this.apiBaseUrl)
                .addConverterFactory(GsonConverterFactory.create())
                .client(client)
                .build();
        EventuresAPI service = retrofit.create(EventuresAPI.class);

        try {
            Call<LoginResponse> loginRequest;

            UserLoginModel user = new UserLoginModel();
            user.setUsername(username);
            user.setPassword(password);
            loginRequest = service.login(user);
            loginRequest.enqueue(new Callback<LoginResponse>() {
                @Override
                public void onResponse(Call<LoginResponse> call, Response<LoginResponse> response) {
                    if (response.code() != HttpURLConnection.HTTP_OK) {
                        showErrorMsg("Error. HTTP code: " + response.message());
                        return;
                    }

                    token = response.body().getToken();
                    getEvents();
                }

                @Override
                public void onFailure(Call<LoginResponse> call, Throwable t) {
                    showErrorMsg(t.getMessage());
                }
            });
        } catch (Throwable t) {
            showErrorMsg(t.getMessage());
        }
    }

    private void getEvents() {
        showStatusMsg("Loading events ...");

        try {
            Retrofit retrofit = new Retrofit.Builder()
                    .baseUrl(this.apiBaseUrl)
                    .addConverterFactory(GsonConverterFactory.create())
                    .client(client)
                    .build();
            EventuresAPI service = retrofit.create(EventuresAPI.class);

            Call<List<Event>> request;

            request = service.getEvents("Bearer " + token);
            request.enqueue(new Callback<List<Event>>() {
                @Override
                public void onResponse(Call<List<Event>> call, Response<List<Event>> response) {
                    if (response.code() != HttpURLConnection.HTTP_OK) {
                        showErrorMsg("Error. HTTP code: " + response.code() + ": " + response.message());
                        return;
                    }
                    displayEvents(response.body());
                }

                @Override
                public void onFailure(Call<List<Event>> call, Throwable t) {
                    showErrorMsg(t.getMessage());
                }
            });
        } catch (Throwable t) {
            showErrorMsg(t.getMessage());
        }
    }

    private void displayEvents(List<Event> events) {
        showSuccessMsg("Events found: " + events.size());

        // Lookup the recyclerview in activity layout
        RecyclerView recyclerViewEvents =
                (RecyclerView) findViewById(R.id.recyclerViewEvents);

        EventsAdapter eventsAdapter = new EventsAdapter(events);
        // Attach the adapter to the RecyclerView to populate items
        recyclerViewEvents.setAdapter(eventsAdapter);
        // Set layout manager to position the items
        recyclerViewEvents.setLayoutManager(new LinearLayoutManager(this));
    }

    private void registerUser(String userName, String email, String password, String confirmPassword, String firstName, String lastName) {
        showStatusMsg("Registering the new user...");

        try {
            Retrofit retrofit = new Retrofit.Builder()
                    .baseUrl(this.apiBaseUrl)
                    .addConverterFactory(GsonConverterFactory.create())
                    .client(client)
                    .build();
            EventuresAPI service = retrofit.create(EventuresAPI.class);

            EventuresUser user = new EventuresUser();
            user.setUsername(userName);
            user.setEmail(email);
            user.setPassword(password);
            user.setConfirmPassword(confirmPassword);
            user.setFirstName(firstName);
            user.setLastName(lastName);

            Call<EventReponse> request = service.register(user);
            request.enqueue(new Callback<EventReponse>() {
                @Override
                public void onResponse(Call<EventReponse> call, Response<EventReponse> response) {
                    if (response.code() != HttpURLConnection.HTTP_OK) {
                        showErrorMsg("Error. HTTP code: " + response.code() + ": " + response.message());
                        return;
                    }
                    showSuccessMsg("New user registered.");
                    authorize(userName, password);
                }

                @Override
                public void onFailure(Call<EventReponse> call, Throwable t) {
                    showErrorMsg(t.getMessage());
                }
            });
        } catch (Throwable t) {
            showErrorMsg(t.getMessage());
        }
    }

    private void createEvent(String name, String place, String start, String end, String tickets, String price) {
        showStatusMsg("Creating new event ...");
        try {
            Retrofit retrofit = new Retrofit.Builder()
                    .baseUrl(this.apiBaseUrl)
                    .addConverterFactory(GsonConverterFactory.create())
                    .client(client)
                    .build();
            EventuresAPI service = retrofit.create(EventuresAPI.class);

            Event event = new Event();
            event.setName(name);
            event.setPlace(place);
            event.setStart(start);
            event.setEnd(end);
            event.setTotalTickets(tickets);
            event.setPricePerTicket(price);

            Call<EventReponse> request = service.create(event, "Bearer " + token);
            request.enqueue(new Callback<EventReponse>() {
                @Override
                public void onResponse(Call<EventReponse> call, Response<EventReponse> response) {
                    if (response.code() != HttpURLConnection.HTTP_CREATED) {
                        showErrorMsg("Error. HTTP code: " + response.code() + ": " + response.message());
                        return;
                    }
                    showSuccessMsg("New event created.");
                    getEvents();
                }

                @Override
                public void onFailure(Call<EventReponse> call, Throwable t) {
                    showErrorMsg(t.getMessage());
                }
            });
        } catch (Throwable t) {
            showErrorMsg(t.getMessage());
        }
    }

    private void showStatusMsg(String msg) {
        textViewStatus.setText(msg);
        textViewStatus.setBackgroundResource(R.color.backgroundStatus);
    }

    private void showSuccessMsg(String msg) {
        textViewStatus.setText(msg);
        textViewStatus.setBackgroundResource(R.color.backgroundSuccess);
    }

    private void showErrorMsg(String errMsg) {
        textViewStatus.setText(errMsg);
        textViewStatus.setBackgroundResource(R.color.backgroundError);
    }

    private void hideSoftKeyboard(Activity activity) {
        InputMethodManager inputMethodManager =
                (InputMethodManager)activity.getSystemService(Activity.INPUT_METHOD_SERVICE);
        inputMethodManager.hideSoftInputFromWindow(activity.getCurrentFocus().getWindowToken(), 0);
    }
}
