package eu.styrian.neural.magician.api.interfaces;

import eu.styrian.neural.magician.api.models.Value;
import retrofit2.Call;
import retrofit2.http.GET;

/**
 * Created by Gal on 13. 05. 2017.
 */

public interface ValueService {

    @GET("/api/values/1")
    Call<Value> getAnswers();

}
