package eu.styrian.neural.magician.api.interfaces;

import java.util.List;

import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.Part;
import retrofit2.http.Path;

/**
 * Created by Gal on 13. 05. 2017.
 */

public interface ImageService {

    @GET("/api/images/{id}")
    Call<ResponseBody> getById(@Path("id")String id);

    @Multipart
    @POST("/api/images")
    Call<ResponseBody> post(@Part MultipartBody.Part file);

    @Multipart
    @POST("/api/images")
    Call<ResponseBody> post(
            @Part("description") RequestBody description,
            @Part MultipartBody.Part file
    );

}
