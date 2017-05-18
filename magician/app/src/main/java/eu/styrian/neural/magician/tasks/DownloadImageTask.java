package eu.styrian.neural.magician.tasks;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.ImageView;

import java.io.File;

import eu.styrian.neural.magician.api.interfaces.ImageService;
import eu.styrian.neural.magician.api.models.ImageViewUrl;
import eu.styrian.neural.magician.api.request.ImageRequestFactory;
import eu.styrian.neural.magician.api.utils.ApiServiceFactory;
import okhttp3.MultipartBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by Gal on 13. 05. 2017.
 */

public class DownloadImageTask extends AsyncTask<ImageViewUrl, Void, Bitmap> {

    public ImageService imageService;
    public ImageRequestFactory factory;

    public DownloadImageTask() {
        imageService = ApiServiceFactory.getImageService();
        factory = ImageRequestFactory.getInstance();
    }

    ImageView imageView;

    @Override
    protected Bitmap doInBackground(ImageViewUrl... imageViewUrls) {
        int count = imageViewUrls.length;
        if (count < 1) {
            return null;
        }

        ImageViewUrl imageViewUrl = imageViewUrls[0];
        this.imageView = imageViewUrl.getImageView();

        // https://github.com/iPaulPro/aFileChooser/blob/master/aFileChooser/src/com/ipaulpro/afilechooser/utils/FileUtils.java
        // use the FileUtils to get the actual file by uri
        // File file = FileUtils.getFile(this, fileUri);
        File file = new File("/mnt/sdcard/person.jpg");

        MultipartBody.Part imageFileBody = factory.generatePostRequest(file);

        Call<ResponseBody> call = imageService.post(imageFileBody);

        call.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {

                Log.i("", "neki - success" + response.code());
                if (response.isSuccessful()) {
                    Log.i("", "neki - success" + response.body().contentLength());
                    Bitmap bitmap = BitmapFactory.decodeStream(response.body().byteStream());
                    imageView.setImageBitmap(bitmap);
                }
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                Log.e("", " - fail" + t.getMessage());
            }
        });


        //Log.i("TAG", "eagle: " + bitmap.getWidth());

        return null;
    }

}