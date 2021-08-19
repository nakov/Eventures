package eventures.ui;

import android.annotation.SuppressLint;
import android.app.DatePickerDialog;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;

import com.example.eventures.android.R;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.Calendar;

public class AddEventActivity extends AppCompatActivity {
    @SuppressLint("ClickableViewAccessibility")
    @RequiresApi(api = Build.VERSION_CODES.O)
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_event);

        EditText editTextName = findViewById(R.id.editTextName);
        editTextName.requestFocus();
        EditText editTextPlace = findViewById(R.id.editTextPlace);
        EditText editTextStart = findViewById(R.id.editTextStart);
        EditText editTextEnd = findViewById(R.id.editTextEnd);
        EditText editTextTickets = findViewById(R.id.editTextTickets);
        EditText editTextPrice = findViewById(R.id.editTextPrice);

        @SuppressLint("ClickableViewAccessibility")
        View.OnTouchListener touchListenerDateSelect = (v, event) -> {
            if (event.getAction() != MotionEvent.ACTION_DOWN)
                return false;

            // Create a date picker dialog and display it
            DatePickerDialog.OnDateSetListener onDateSetListener = (view, selectedYear, selectedMonth, selectedDay) -> {
                LocalDateTime selectedDate = LocalDateTime.of(selectedYear, selectedMonth, selectedDay, 0, 0);
                String selectedDateStr = selectedDate.format(DateTimeFormatter.ISO_LOCAL_DATE_TIME);
                ((EditText)v).setText(selectedDateStr);
            };

            final Calendar cldr = Calendar.getInstance();
            int currentDay = cldr.get(Calendar.DAY_OF_MONTH);
            int currentMonth = cldr.get(Calendar.MONTH);
            int currentYear = cldr.get(Calendar.YEAR);

            DatePickerDialog datePicker = new DatePickerDialog(AddEventActivity.this, onDateSetListener,
                    currentYear, currentMonth, currentDay);
            datePicker.show();

            return false;
        };

        editTextStart.setOnTouchListener(touchListenerDateSelect);
        editTextEnd.setOnTouchListener(touchListenerDateSelect);

        Button buttonCancel = findViewById(R.id.buttonCancel);
        buttonCancel.setOnClickListener(v -> {
            setResult(RESULT_CANCELED);
            finish();
        });

        Button buttonCreate = findViewById(R.id.buttonCreate);
        buttonCreate.setOnClickListener(v -> {
            Intent resultData = new Intent();
            resultData.putExtra("name", editTextName.getText().toString());
            resultData.putExtra("place", editTextPlace.getText().toString());
            resultData.putExtra("start", editTextStart.getText().toString());
            resultData.putExtra("end", editTextEnd.getText().toString());
            resultData.putExtra("totalTickets", editTextTickets.getText().toString());
            resultData.putExtra("pricePerTicket", editTextPrice.getText().toString());
            setResult(RESULT_OK, resultData);
            finish();
        });
    }
}
