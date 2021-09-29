package eventures.ui;

import android.annotation.SuppressLint;
import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;

import com.example.eventures.android.R;
import com.google.type.DateTime;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.Calendar;
import java.util.Date;

public class AddEventActivity extends AppCompatActivity {
    EditText editTextName, editTextPlace, editTextStartDate, editTextStartTime, editTextEndDate, editTextEndTime, editTextTickets, editTextPrice;
    String start, end;

    @SuppressLint("ClickableViewAccessibility")
    @RequiresApi(api = Build.VERSION_CODES.O)
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_event);

        editTextName = findViewById(R.id.editTextName);
        editTextName.requestFocus();
        editTextPlace = findViewById(R.id.editTextPlace);

        editTextStartDate = findViewById(R.id.editTextStartDate);
        editTextStartTime = findViewById(R.id.editTextStartTime);

        editTextEndDate = findViewById(R.id.editTextEndDate);
        editTextEndTime = findViewById(R.id.editTextEndTime);

        editTextTickets = findViewById(R.id.editTextTickets);
        editTextPrice = findViewById(R.id.editTextPrice);

        @SuppressLint("ClickableViewAccessibility")
        View.OnTouchListener touchListenerDateSelect = (v, event) -> {
            if (event.getAction() != MotionEvent.ACTION_DOWN)
                return false;

            // Create a date picker dialog and display it
            DatePickerDialog.OnDateSetListener onDateSetListener = (view, selectedYear, selectedMonth, selectedDay) -> {
                LocalDate selectedDate = LocalDate.of(selectedYear, selectedMonth+1, selectedDay);
                String selectedDateStr = selectedDate.format(DateTimeFormatter.ISO_LOCAL_DATE);
                ((EditText)v).setText(selectedDateStr);
            };

            final Calendar cldr = Calendar.getInstance();
            int currentDay = cldr.get(Calendar.DAY_OF_MONTH) + 1;
            int currentMonth = cldr.get(Calendar.MONTH);
            int currentYear = cldr.get(Calendar.YEAR);

            DatePickerDialog datePicker = new DatePickerDialog(AddEventActivity.this, onDateSetListener,
                    currentYear, currentMonth, currentDay);
            datePicker.show();

            return false;
        };

        editTextStartDate.setOnTouchListener(touchListenerDateSelect);
        editTextEndDate.setOnTouchListener(touchListenerDateSelect);

        View.OnTouchListener touchListenerTimeSelect = (v, event) -> {
            if (event.getAction() != MotionEvent.ACTION_DOWN)
                return false;

            // Create a time picker dialog and display it
            TimePickerDialog.OnTimeSetListener onTimeSetListener = (view, selectedHour, selectedMinutes) -> {
                LocalTime selectedTime = LocalTime.of(selectedHour, selectedMinutes);
                String selectedTimeStr = selectedTime.format(DateTimeFormatter.ISO_LOCAL_TIME);
                ((EditText)v).setText(selectedTimeStr);
            };

            final Calendar cldr = Calendar.getInstance();
            int hour = cldr.get(Calendar.HOUR_OF_DAY);
            int minutes = cldr.get(Calendar.MINUTE);

            TimePickerDialog timePicker = new TimePickerDialog(AddEventActivity.this, onTimeSetListener,
                    hour, minutes, true);
            timePicker.show();
            return false;
        };

        editTextStartTime.setOnTouchListener(touchListenerTimeSelect);
        editTextEndTime.setOnTouchListener(touchListenerTimeSelect);

        Button buttonCancel = findViewById(R.id.buttonCancel);
        buttonCancel.setOnClickListener(v -> {
            setResult(RESULT_CANCELED);
            finish();
        });

        Button buttonCreate = findViewById(R.id.buttonCreate);
        buttonCreate.setOnClickListener(v -> {
            start = editTextStartDate.getText().toString() + "T" + editTextStartTime.getText().toString();
            end = editTextEndDate.getText().toString() + "T" + editTextEndTime.getText().toString();
            String errors = CheckAllFields();
            if(errors == "") {
                Intent resultData = new Intent();
                resultData.putExtra("name", editTextName.getText().toString());
                resultData.putExtra("place", editTextPlace.getText().toString());
                resultData.putExtra("start", start);
                resultData.putExtra("end", end);
                resultData.putExtra("totalTickets", editTextTickets.getText().toString());
                resultData.putExtra("pricePerTicket", editTextPrice.getText().toString());
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

    @RequiresApi(api = Build.VERSION_CODES.O)
    private String CheckAllFields() {
        StringBuilder errors = new StringBuilder();

        // Name checks
        String name = editTextName.getText().toString();
        if (name.length() == 0) {
            errors.append("Name field is required.");
        }
        int minNameLength = 3;
        if (name.length() > 0 && name.length() < minNameLength) {
            errors.append(System.lineSeparator());
            errors.append("Name must be at least " + minNameLength + " characters long.");
        }

        // Place checks
        String place = editTextPlace.getText().toString();
        if (place.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Place field is required.");
        }
        int minPlaceLength = 3;
        if (place.length() > 0 && place.length() < minPlaceLength) {
            errors.append(System.lineSeparator());
            errors.append("Place must be at least " + minPlaceLength + " characters long.");
        }

        // Start date and time checks
        String startDate = editTextStartDate.getText().toString();
        if (startDate.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Start Date field is required.");
        }

        String startTime = editTextStartTime.getText().toString();
        if (startTime.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Start Time field is required.");
        }

        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
        Date startDateTime = new Date();
        try {
            startDateTime = sdf.parse(start);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        LocalDateTime now = LocalDateTime.now();
        try {
            if (startDateTime.before(sdf.parse(String.valueOf(now)))) {
                errors.append(System.lineSeparator());
                errors.append("Start Date must be in the future.");
            }
            else if(startDateTime.after(sdf.parse("2100-01-01T00:00:00.000"))) {
                errors.append(System.lineSeparator());
                errors.append("Start Date must be before the 2100 year.");
            }
        } catch (ParseException e) {
            e.printStackTrace();
        }

        // End date and time checks
        String endDate = editTextEndDate.getText().toString();
        if (endDate.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("End Date field is required.");
        }

        String endTime = editTextEndTime.getText().toString();
        if (endTime.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("End Time field is required.");
        }

        Date endDateTime = new Date();
        try {
            endDateTime = sdf.parse(end);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        if (endDateTime != null && endDateTime.before(startDateTime)) {
            errors.append(System.lineSeparator());
            errors.append("End Date must be after the Start Date.");
        }
        else {
            try {
                if(endDateTime != null &&  endDateTime.after(sdf.parse("2100-01-01T00:00:00.000"))) {
                    errors.append(System.lineSeparator());
                    errors.append("End Date must be before the 2100 year.");
                }
            } catch (ParseException e) {
                e.printStackTrace();
            }
        }

        // Tickets checks
        String ticketsStr = editTextTickets.getText().toString();
        if (ticketsStr.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Tickets field is required.");
        }
        else {
            int tickets = Integer.parseInt(editTextTickets.getText().toString());
            if (tickets < 0) {
                errors.append(System.lineSeparator());
                errors.append("Tickets must be a positive number.");
            }
            int maxTickets = 1000;
            if (tickets > maxTickets) {
                errors.append(System.lineSeparator());
                errors.append("Tickets must be less than " + maxTickets + ".");
            }
        }

        // Price checks
        String priceStr = editTextPrice.getText().toString();
        if (priceStr.length() == 0) {
            errors.append(System.lineSeparator());
            errors.append("Price field is required.");
        }
        else {
            double price = Double.parseDouble(editTextPrice.getText().toString());
            if (price < 0) {
                errors.append(System.lineSeparator());
                errors.append("Price must be a positive number.");
            }
            int maxPrice = 1000;
            if (price > maxPrice) {
                errors.append(System.lineSeparator());
                errors.append("Price must be less than " + maxPrice + ".");
            }
        }

        return  errors.toString();
    }
}
