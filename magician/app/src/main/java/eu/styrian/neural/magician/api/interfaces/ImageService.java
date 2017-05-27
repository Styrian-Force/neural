package eu.styrian.neural.magician.api.interfaces;


import okhttp3.MultipartBody;
import okhttp3.ResponseBody;
import retrofit2.Response;
import retrofit2.http.GET;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.Part;
import retrofit2.http.Path;
import rx.Observable;

/**
 * Created by Gal on 13. 05. 2017.
 */

public interface ImageService {

    @GET("api/images/{id}")
    Observable<Response<ResponseBody>> getById(@Path("id") String id);

    @Multipart
    @POST("api/images")
    Observable<Response> post(@Part MultipartBody.Part file);

}

/* EXAMPLE
    Observable<Response<ResponseBody>> onImage = imageService.getById("2017-05-15_16-37-09.770");
        Log.i("", "neki - ");
                onImage.subscribeOn(Schedulers.newThread())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Subscriber<Response<ResponseBody>>() {
@Override
public final void onCompleted() {
        }

@Override
public final void onError(Throwable e) {
        Log.d("error", e.getMessage());
        }

@Override
public final void onNext(Response<ResponseBody> response) {
        Log.d("success", response.body() + " asd");
        Log.i("", "neki - success" + response.code());
        if (response.isSuccessful()) {
        Log.i("", "neki - success" + response.body().contentLength());
        Bitmap bitmap = BitmapFactory.decodeStream(response.body().byteStream());
        imageView.setImageBitmap(bitmap);
        }
        buttonSend.setEnabled(true);
        }
        });
*/