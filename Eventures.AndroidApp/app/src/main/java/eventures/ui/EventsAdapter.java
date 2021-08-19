package eventures.ui;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import eventures.data.Event;
import com.example.eventures.android.R;

import java.util.List;

public class EventsAdapter extends
        RecyclerView.Adapter<EventsAdapter.ViewHolder>{

    private List<Event> events;

    public EventsAdapter(List<Event> events) {
        this.events = events;
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        Context context = parent.getContext();
        LayoutInflater inflater = LayoutInflater.from(context);

        // Inflate the custom layout
        View taskView = inflater.inflate(R.layout.fragment_event_data, parent, false);

        // Return a new holder instance
        ViewHolder viewHolder = new ViewHolder(taskView);
        return viewHolder;
    }

    // Populates data into the item through holder
    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        // Get the data model based on position
        Event event = this.events.get(position);

        // Set item views based on your views and data model
        holder.textViewTaskId.setText("" + event.getId());
        holder.textViewName.setText(event.getName());
        holder.textViewPlace.setText(event.getPlace());
        holder.textViewStart.setText("" + event.getStart());
        holder.textViewEnd.setText("" + event.getEnd());
        holder.textViewTickets.setText(event.getTotalTickets());
        holder.textViewPrice.setText(event.getPricePerTicket());
        holder.textViewOwner.setText(event.getOwner().getUsername());
    }

    @Override
    public int getItemCount() {
        return this.events.size();
    }

    // Provide a direct reference to each of the views within a data item
    // Used to cache the views within the item layout for fast access
    public class ViewHolder extends RecyclerView.ViewHolder {
        // Your holder should contain a member variable
        // for any view that will be set as you render a row
        public TextView textViewTaskId;
        public TextView textViewName;
        public TextView textViewPlace;
        public TextView textViewStart;
        public TextView textViewEnd;
        public TextView textViewTickets;
        public TextView textViewPrice;
        public TextView textViewOwner;

        // We also create a constructor that accepts the entire item row
        // and does the view lookups to find each subview
        public ViewHolder(View itemView) {
            // Stores the itemView in a public final member variable that can be used
            // to access the context from any ViewHolder instance.
            super(itemView);
            textViewTaskId = (TextView) itemView.findViewById(R.id.textViewEventId);
            textViewName = (TextView) itemView.findViewById(R.id.textViewName);
            textViewPlace = (TextView) itemView.findViewById(R.id.textViewPlace);
            textViewStart = (TextView) itemView.findViewById(R.id.textViewStart);
            textViewEnd = (TextView) itemView.findViewById(R.id.textViewEnd);
            textViewTickets = (TextView) itemView.findViewById(R.id.textViewTickets);
            textViewPrice = (TextView) itemView.findViewById(R.id.textViewPrice);
            textViewOwner = (TextView) itemView.findViewById(R.id.textViewOwner);
        }
    }
}
