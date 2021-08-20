package eventures.ui;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;

import androidx.appcompat.app.AppCompatActivity;

import com.example.eventures.android.R;
import eventures.data.Event;
import eventures.data.EventuresAPI;

import java.net.HttpURLConnection;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ActivityConnect extends AppCompatActivity {
    private String apiBaseUrl;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_connect);

        Button buttonConnect = findViewById(R.id.buttonConnect);
        buttonConnect.requestFocus();
        EditText editTextApiUrl = findViewById(R.id.editTextApiUrl);

        buttonConnect.setOnClickListener(v -> {
            this.apiBaseUrl = editTextApiUrl.getText().toString();
            if (!this.apiBaseUrl.endsWith("/"))
                this.apiBaseUrl += "/";

            try {
                Retrofit retrofit = new Retrofit.Builder()
                        .baseUrl(this.apiBaseUrl)
                        .addConverterFactory(GsonConverterFactory.create())
                        .build();
                EventuresAPI service = retrofit.create(EventuresAPI.class);

                Call<List<Event>> request;

                request = service.getEvents("Bearer ");
                try {
                    request.enqueue(new Callback<List<Event>>() {
                        @Override
                        public void onResponse(Call<List<Event>> call, Response<List<Event>> response) {
                            if (response.code() != HttpURLConnection.HTTP_UNAUTHORIZED) {
                                returnToActivityConnect();
                            }
                            else {
                                connect();
                                return;
                            }
                        }

                        @Override
                        public void onFailure(Call<List<Event>> call, Throwable t) {
                            returnToActivityConnect();
                        }
                    });
                }
                catch (Throwable tr) {
                    returnToActivityConnect();
                }

            } catch (Throwable t) {
                returnToActivityConnect();
            }
        });
    }

    private void connect() {
        EditText editTextApiUrl = findViewById(R.id.editTextApiUrl);
        String apiUrl = editTextApiUrl.getText().toString();
        Intent intent = new Intent(this, ActivityEvents.class);
        intent.putExtra("paramApiBaseUrl", apiUrl);
        startActivity(intent);
    }

    private void returnToActivityConnect() {
        Intent intent = new Intent(this, ActivityConnect.class);
        startActivity(intent);
    }
}
