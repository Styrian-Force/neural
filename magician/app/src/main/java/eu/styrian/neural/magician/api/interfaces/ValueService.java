package eu.styrian.neural.magician.api.interfaces;

import java.util.List;

import eu.styrian.neural.magician.api.models.Value;
import retrofit2.http.GET;
import retrofit2.http.Path;
import rx.Observable;

/**
 * Created by Gal on 13. 05. 2017.
 */

public interface ValueService {

    @GET("api/values/{id}")
    Observable<Value> getById(@Path("id") int id);

    @GET("api/values")
    Observable<List<Value>> getAll();
}
