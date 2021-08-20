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
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_connect);

        Button buttonConnect = findViewById(R.id.buttonConfirmConnect);
        buttonConnect.requestFocus();
        EditText editTextApiUrl = findViewById(R.id.editTextApiUrl);

        buttonConnect.setOnClickListener(v -> {
            Intent resultData = new Intent();
            resultData.putExtra("paramApiBaseUrl", editTextApiUrl.getText().toString());
            setResult(RESULT_OK, resultData);
            finish();
        });
    }
}
