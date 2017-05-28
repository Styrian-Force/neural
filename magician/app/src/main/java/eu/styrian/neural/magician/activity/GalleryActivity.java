package eu.styrian.neural.magician.activity;

import android.annotation.TargetApi;
import android.content.Context;
import android.content.CursorLoader;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Build;
import android.provider.DocumentsContract;
import android.provider.MediaStore;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;

import java.io.File;
import java.util.concurrent.TimeUnit;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import eu.styrian.neural.magician.R;
import eu.styrian.neural.magician.api.interfaces.ImageService;
import eu.styrian.neural.magician.api.interfaces.ImageTaskService;
import eu.styrian.neural.magician.api.models.ImageTask;
import eu.styrian.neural.magician.api.models.ImageTaskStatus;
import eu.styrian.neural.magician.api.models.Value;
import eu.styrian.neural.magician.api.request.ImageRequestFactory;
import eu.styrian.neural.magician.api.utils.ApiServiceFactory;
import okhttp3.MultipartBody;
import okhttp3.ResponseBody;
import retrofit2.Response;
import rx.Observable;
import rx.Subscriber;
import rx.android.schedulers.AndroidSchedulers;
import rx.schedulers.Schedulers;

public class GalleryActivity extends AppCompatActivity {

    private static final int SELECT_PICTURE = 1;

    private String selectedImagePath;

    @BindView(R.id.image_view)
    public ImageView imageView;

    @BindView(R.id.button_load)
    public Button buttonLoad;

    @BindView(R.id.button_send)
    public Button buttonSend;

    private ImageService _imageService;
    private ImageTaskService _imageTaskService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_gallery);
        ButterKnife.bind(this);

        this._imageService = ApiServiceFactory.getImageService();
        this._imageTaskService = ApiServiceFactory.getImageTaskService();
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == RESULT_OK) {
            if (requestCode == SELECT_PICTURE) {
                Uri selectedImageUri = data.getData();

                selectedImagePath = getFilePath(this, selectedImageUri);

                imageView.setImageURI(selectedImageUri);
                Log.d("", "ssss" + selectedImagePath);
            }
        }
    }

    @TargetApi(Build.VERSION_CODES.KITKAT)
    public static String getFilePath(Context context, Uri uri)
    {
        int currentApiVersion;
        try
        {
            currentApiVersion = android.os.Build.VERSION.SDK_INT;
        }
        catch(NumberFormatException e)
        {
            //API 3 will crash if SDK_INT is called
            currentApiVersion = 3;
        }
        if (currentApiVersion >= Build.VERSION_CODES.KITKAT)
        {
            String filePath = "";
            String wholeID = DocumentsContract.getDocumentId(uri);

            // Split at colon, use second item in the array
            String id = wholeID.split(":")[1];

            String[] column = {MediaStore.Images.Media.DATA};

            // where id is equal to
            String sel = MediaStore.Images.Media._ID + "=?";

            Cursor cursor = context.getContentResolver().query(MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                    column, sel, new String[]{id}, null);

            int columnIndex = cursor.getColumnIndex(column[0]);

            if (cursor.moveToFirst())
            {
                filePath = cursor.getString(columnIndex);
            }
            cursor.close();
            return filePath;
        }
        else if (currentApiVersion <= Build.VERSION_CODES.HONEYCOMB_MR2 && currentApiVersion >= Build.VERSION_CODES.HONEYCOMB)

        {
            String[] proj = {MediaStore.Images.Media.DATA};
            String result = null;
            CursorLoader cursorLoader = new CursorLoader(
                    context,
                    uri, proj, null, null, null);
            Cursor cursor = cursorLoader.loadInBackground();

            if (cursor != null)
            {
                int column_index =
                        cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
                cursor.moveToFirst();
                result = cursor.getString(column_index);
            }
            return result;
        }
        else
        {

            String[] proj = {MediaStore.Images.Media.DATA};
            Cursor cursor = context.getContentResolver().query(uri, proj, null, null, null);
            int column_index
                    = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
            cursor.moveToFirst();
            return cursor.getString(column_index);
        }
    }

    public String getPath1(Uri uri) {
        String[] projection = { MediaStore.Images.Media.DATA };
        Cursor cursor = getContentResolver().query(uri, projection, null, null, null);
        int column_index = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
        cursor.moveToFirst();
        int columnIndex = cursor.getColumnIndex(projection[0]);
        String filePath = cursor.getString(columnIndex);
        cursor.close();
        return cursor.getString(column_index);
    }

    @OnClick(R.id.button_load)
    public void loadImage(View view) {
        Log.d("asd", "BUTTON LOAD!");
        Intent intent = new Intent();
        intent.setType("image/*");
        intent.setAction(Intent.ACTION_GET_CONTENT);
        startActivityForResult(Intent.createChooser(intent, "Izberi fotografijo"), SELECT_PICTURE);
    }

    @OnClick(R.id.button_send)
    public void sendImage(View view) {
        Log.d("asd", "BUTTON SEND!" + this.selectedImagePath);

        this.buttonSend.setEnabled(false);
        if(this.selectedImagePath == null) {
            Log.d("ERROR1", "ni datoteke!!! ne more poslati");
            this.buttonSend.setEnabled(true);
            return;
        }

        File file = new File(this.selectedImagePath);

        if(!file.exists()) {
            Log.d("ERROR1", "ni datoteke!!! ne more poslati");
            this.buttonSend.setEnabled(true);
            return;
        }

        MultipartBody.Part imageFileBody = ImageRequestFactory.getInstance().generatePostRequest(file);

        /* Call<ResponseBody> call = imageService.post(imageFileBody);

        call.enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {

                Log.i("", "neki - success" + response.code());
                if (response.isSuccessful()) {
                    Log.i("", "neki - success" + response.body().contentLength());
                    Bitmap bitmap = BitmapFactory.decodeStream(response.body().byteStream());
                    imageView.setImageBitmap(bitmap);
                }
                buttonSend.setEnabled(true);
            }

            @Override
            public void onFailure(Call<ResponseBody> call, Throwable t) {
                Log.e("", " - fail" + t.getMessage());
            }
        });

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

        Observable<ImageTask> onImage = _imageService.post(imageFileBody);
        Log.i("", "neki - ");
        onImage.subscribeOn(Schedulers.newThread())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Subscriber<ImageTask>() {
                    @Override
                    public final void onCompleted() {
                    }

                    @Override
                    public final void onError(Throwable e) {
                        Log.d("error", e.getMessage());
                    }

                    @Override
                    public final void onNext(ImageTask imageTask) {
                        checkImageTaskStatus(imageTask);
                    }
                });
    }


    public void checkImageTaskStatus(final ImageTask imageTaskIn) {
        if(imageTaskIn == null) {
            Log.d(this.getClass().toString(), "ImageTask in null.");
            return;
        }
        
        Observable<ImageTask> onImageTask = _imageTaskService.getById(imageTaskIn.getJobId());

        onImageTask.delay(1, TimeUnit.SECONDS)
                .subscribeOn(Schedulers.newThread())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Subscriber<ImageTask>() {
                    @Override
                    public final void onCompleted() {
                    }

                    @Override
                    public final void onError(Throwable e) {
                        Log.d("error", e.getMessage());
                    }

                    @Override
                    public final void onNext(ImageTask imageTask) {
                        Log.d(this.getClass().toString(), "imageTask: " + imageTask.getJobId());

                        if(imageTask == null) {
                            Log.d(this.getClass().toString(), "ImageTask in null.");
                            return;
                        }

                        ImageTaskStatus status = ImageTaskStatus.parse(imageTask.getStatus());

                        if(status == ImageTaskStatus.FINISHED) {
                            getImage(imageTask);
                            return;
                        }
                        else if (status == ImageTaskStatus.ERROR) {
                            Log.d(this.getClass().toString(), "Error in image task.");
                            return;
                        }
                        else  {
                            Log.d(this.getClass().toString(), "CurrentStatus: " + imageTask.getStatus());
                            checkImageTaskStatus(imageTask);
                            return;
                        }

                    }
                });
    }

    private void getImage(ImageTask imageTask) {
        Log.d(this.getClass().toString(), "GET_IMAGE: " + imageTask.getJobId() + " " + imageTask.getStatus());

        Observable<Response<ResponseBody>> onImage = _imageService.getById(imageTask.getJobId());
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
    }

}
