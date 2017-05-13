package eu.styrian.neural.magician.api.interfaces;

import java.util.List;

import eu.styrian.neural.magician.api.models.Value;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;

/**
 * Created by Gal on 13. 05. 2017.
 */

public interface ValueService {

    @GET("/api/values/{id}")
    Call<Value> getById(@Path("id") int id);

    @GET("/api/values")
    Call<List<Value>> get();

}
