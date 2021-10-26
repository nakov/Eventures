package eventures.ui;

import static eventures.data.Constants.MinPasswordLength;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;

import com.example.eventures.android.R;

public class RegisterActivity extends AppCompatActivity {
    EditText editTextUserName, editTextEmail, editTextPassword, editTextConfirmPassword, editTextFirstName, editTextLastName;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        editTextUserName = findViewById(R.id.editTextUsername);
        editTextUserName.requestFocus();
        editTextEmail = findViewById(R.id.editTextEmail);
        editTextPassword = findViewById(R.id.editTextPassword);
        editTextConfirmPassword = findViewById(R.id.editTextConfirmPassword);
        editTextFirstName = findViewById(R.id.editTextFirstName);
        editTextLastName = findViewById(R.id.editTextLastName);

        Button buttonCancel = findViewById(R.id.buttonCancel);
        buttonCancel.setOnClickListener(v -> {
            setResult(RESULT_CANCELED);
            finish();
        });

        Button buttonLogin = findViewById(R.id.buttonConfirmRegister);
        buttonLogin.setOnClickListener(v -> {
            String errors = CheckAllFields();
            if(errors == "") {
                Intent resultData = new Intent();
                resultData.putExtra("username", editTextUserName.getText().toString());
                resultData.putExtra("email", editTextEmail.getText().toString());
                resultData.putExtra("password", editTextPassword.getText().toString());
                resultData.putExtra("confirmPassword", editTextConfirmPassword.getText().toString());
                resultData.putExtra("firstName", editTextFirstName.getText().toString());
                resultData.putExtra("lastName", editTextLastName.getText().toString());
                setResult(RESULT_OK, resultData);
                finish();
            }
            else {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.setMessage(errors)
                        .setCancelable(false)
                        .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                            }
                        });
                //Creating dialog box
                AlertDialog alert = builder.create();
                //Setting the title manually
                alert.setTitle("Errors");
                alert.show();
            }
        });
    }

    private String CheckAllFields() {
        StringBuilder errors = new StringBuilder();
        String username = editTextUserName.getText().toString();
        if (username.length() == 0) {
            errors.append("Username field is required.");
        }

        String email = editTextEmail.getText().toString();
        if (email.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Email field is required.");
        }
        else {
            var result = isValidEmailAddress(email);
            if(!result)
            {
                errors.append(System.lineSeparator());
                errors.append("Email field must have a valid email address.");
            }
        }

        String firstName = editTextFirstName.getText().toString();
        if (firstName.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("First Name field is required.");
        }

        String lastName = editTextLastName.getText().toString();
        if (lastName.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Last Name field is required.");
        }

        String password = editTextPassword.getText().toString();
        if (password.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Password field is required.");
        }

        if (password.length() > 0 && password.length() < MinPasswordLength) {
            errors.append(System.lineSeparator());
            errors.append("Password must be at least " + MinPasswordLength + " characters long.");
        }

        String confirmPassword = editTextConfirmPassword.getText().toString();
        if (confirmPassword.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Confirm Password field is required.");
        }

        if(confirmPassword.length() > 0 && confirmPassword.compareTo(password) != 0) {
            errors.append(System.lineSeparator());
            errors.append("Confirm Password and Password don't match.");
        }

        return  errors.toString();
    }

    private boolean isValidEmailAddress(String email) {
        String ePattern = "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$";
        java.util.regex.Pattern p = java.util.regex.Pattern.compile(ePattern);
        java.util.regex.Matcher m = p.matcher(email);
        return m.matches();
    }
}
