package eu.styrian.neural.magician.api.interfaces;

import eu.styrian.neural.magician.api.models.ImageTask;
import okhttp3.ResponseBody;
import retrofit2.Response;
import retrofit2.http.GET;
import retrofit2.http.Path;
import rx.Observable;

/**
 * Created by Gal on 28/05/2017.
 */

public interface ImageTaskService {

    @GET("api/imageTasks/{id}")
    Observable<ImageTask> getById(@Path("id") String id);

}
