package eu.styrian.neural.magician.api.interfaces;


import okhttp3.MultipartBody;
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
    Call<ResponseBody> getById(@Path("id") String id);

    @Multipart
    @POST("/api/images")
    Call<ResponseBody> post(@Part MultipartBody.Part file);

}

// EXAMPLE
//String imageUrl = "http://www.eagles.org/wp-content/uploads/2015/10/donate-box-eagle.jpg";
//ImageViewUrl imageViewUrl = new ImageViewUrl();
//imageViewUrl.setImageView(imageView);
//imageViewUrl.setUrl(imageUrl);
//new DownloadImageTask().execute(imageViewUrl);

//Call<ResponseBody> call = imageService.getById("2017-05-13_16-56-16.439");

        /*call.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                if (response.isSuccessful()) {
                    if (response.body() != null) {
                        // display the image data in a ImageView or save it
                        Bitmap bitmap = BitmapFactory.decodeStream(response.body().byteStream());
                        //ivCaptcha.setImageBitmap(bm);
                        imageView.setImageBitmap(bitmap);
                        Log.d("TAG", "WIDTH: " + bitmap.getWidth());
                    } else {
                        Log.d("TAG", "WIDTH: " + "fail1");
                    }
                } else {
                    Log.d("TAG", "WIDTH: " + "fail2");
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                // TODO
            }
        });*/
// }

//public void loadAnswers() {
//        valueService.getById(2).enqueue(new Callback<Value>() {
//@Override
//public void onResponse(Call<Value> call, Response<Value> response) {
//
//        if(response.isSuccessful()) {
//        List<Value> values = new ArrayList<>();
//        values.add(response.body());
//        //valueAdapter.updateAnswers(values);
//
//        //valueAdapter.updateAnswers(response.body());
//        Log.d("MainActivity", "posts loaded from API");
//        }else {
//        int statusCode  = response.code();
//        // handle request errors depending on status code
//        Log.d("MainActivity", "posts loaded from API statusCode" + statusCode);
//        }
//        }
//
//@Override
//public void onFailure(Call<Value> call, Throwable t) {
//        Log.d("MainActivity", "error loading from API");
//
//        }
//        });
//        }