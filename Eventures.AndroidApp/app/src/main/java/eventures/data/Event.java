package eventures.data;

import android.os.Build;

import androidx.annotation.RequiresApi;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Event {
    private int id;
    private String name;
    private String place;
    private String start;
    private String end;
    private String totalTickets;
    private String pricePerTicket;
    private EventuresUser owner;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }


    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }


    public String getPlace() {
        return place;
    }

    public void setPlace(String place) {
        this.place = place;
    }


    public String getStart() {
        return start;
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    public LocalDateTime getStartAsLocalDateTime() {
        return LocalDateTime.parse(this.start, DateTimeFormatter.ISO_LOCAL_DATE_TIME);
    }

    public void setStart(String start) {
        this.start = start;
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    public void setStart(LocalDateTime startDateTime) {
        this.start = startDateTime.format(DateTimeFormatter.ISO_LOCAL_DATE_TIME);
    }


    public String getEnd() {
        return end;
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    public LocalDateTime getEndAsLocalDateTime() {
        return LocalDateTime.parse(this.end, DateTimeFormatter.ISO_LOCAL_DATE_TIME);
    }

    public void setEnd(String end) {
        this.end = end;
    }

    @RequiresApi(api = Build.VERSION_CODES.O)
    public void setEnd(LocalDateTime endDateTime) {
        this.end = endDateTime.format(DateTimeFormatter.ISO_LOCAL_DATE_TIME);
    }

    public String getTotalTickets() {
        return totalTickets;
    }

    public void setTotalTickets(String totalTickets) {
        this.totalTickets = totalTickets;
    }


    public String getPricePerTicket() {
        return pricePerTicket;
    }

    public void setPricePerTicket(String pricePerTicket) {
        this.pricePerTicket = pricePerTicket;
    }


    public EventuresUser getOwner() {
        return owner;
    }
}
