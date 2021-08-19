package eventures.ui;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import com.example.eventures.android.R;

public class LoginActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        EditText editTextUsername = findViewById(R.id.editTextUsername);
        editTextUsername.requestFocus();
        EditText editTextPassword = findViewById(R.id.editTextPassword);

        Button buttonCancel = findViewById(R.id.buttonCancel);
        buttonCancel.setOnClickListener(v -> {
            setResult(RESULT_CANCELED);
            finish();
        });

        Button buttonLogin = findViewById(R.id.buttonConfirmRegister);
        buttonLogin.setOnClickListener(v -> {
            Intent resultData = new Intent();
            resultData.putExtra("username", editTextUsername.getText().toString());
            resultData.putExtra("password", editTextPassword.getText().toString());
            setResult(RESULT_OK, resultData);
            finish();
        });
    }
}
