package eu.styrian.neural.magician.api.interfaces;

import java.util.List;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;

/**
 * Created by Gal on 13. 05. 2017.
 */

public interface ImageService {

    @GET("/api/images/{id}")
    Call<ResponseBody> getById(@Path("id")String id);

}
