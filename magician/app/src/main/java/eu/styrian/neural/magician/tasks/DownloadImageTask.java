package eu.styrian.neural.magician.tasks;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.AsyncTask;
import android.renderscript.ScriptGroup;
import android.util.Log;
import android.widget.ImageView;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

import eu.styrian.neural.magician.api.interfaces.ImageService;
import eu.styrian.neural.magician.api.models.ImageViewUrl;
import eu.styrian.neural.magician.api.request.ImageRequestFactory;
import eu.styrian.neural.magician.api.utils.ApiUtils;
import eu.styrian.neural.magician.api.utils.RequestBodyUtil;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
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
        imageService = ApiUtils.getImageService();
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