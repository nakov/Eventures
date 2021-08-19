package eventures.data;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.Headers;
import retrofit2.http.POST;

public interface EventuresAPI {
    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("events")
    Call<List<Event>> getEvents(@Header("Authorization") String auth);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @POST("events/create")
    Call<EventReponse> create(@Body Event event, @Header("Authorization") String auth);

    @POST("users/login")
    Call<LoginResponse> login(@Body UserLoginModel user);

    @POST("users/register")
    Call<EventReponse> register(@Body EventuresUser user);
}
