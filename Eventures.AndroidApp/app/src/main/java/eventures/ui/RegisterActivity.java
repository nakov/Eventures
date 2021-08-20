package eventures.ui;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import com.example.eventures.android.R;

public class RegisterActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        EditText editTextUserName = findViewById(R.id.editTextUsername);
        editTextUserName.requestFocus();
        EditText editTextEmail = findViewById(R.id.editTextEmail);
        EditText editTextPassword = findViewById(R.id.editTextPassword);
        EditText editTextConfirmPassword = findViewById(R.id.editTextConfirmPassword);
        EditText editTextFirstName = findViewById(R.id.editTextFirstName);
        EditText editTextLastName = findViewById(R.id.editTextLastName);

        Button buttonCancel = findViewById(R.id.buttonCancel);
        buttonCancel.setOnClickListener(v -> {
            setResult(RESULT_CANCELED);
            finish();
        });

        Button buttonLogin = findViewById(R.id.buttonConfirmRegister);
        buttonLogin.setOnClickListener(v -> {
            Intent resultData = new Intent();
            resultData.putExtra("username", editTextUserName.getText().toString());
            resultData.putExtra("email", editTextEmail.getText().toString());
            resultData.putExtra("password", editTextPassword.getText().toString());
            resultData.putExtra("confirmPassword", editTextConfirmPassword.getText().toString());
            resultData.putExtra("firstName", editTextFirstName.getText().toString());
            resultData.putExtra("lastName", editTextLastName.getText().toString());
            setResult(RESULT_OK, resultData);
            finish();
        });
    }
}
