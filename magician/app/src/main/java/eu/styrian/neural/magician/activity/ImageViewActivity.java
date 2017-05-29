package eu.styrian.neural.magician.activity;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.provider.MediaStore;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.Toast;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.InputStream;
import java.util.concurrent.TimeUnit;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import eu.styrian.neural.magician.R;
import eu.styrian.neural.magician.activity.fragment.MagicianSpinnerDialog;
import eu.styrian.neural.magician.api.interfaces.ImageService;
import eu.styrian.neural.magician.api.interfaces.ImageTaskService;
import eu.styrian.neural.magician.api.models.ImageTask;
import eu.styrian.neural.magician.api.models.ImageTaskStatus;
import eu.styrian.neural.magician.api.request.ImageRequestFactory;
import eu.styrian.neural.magician.api.utils.ApiServiceFactory;
import eu.styrian.neural.magician.util.FileUtil;
import eu.styrian.neural.magician.util.ImageProcessingUtil;
import okhttp3.MultipartBody;
import okhttp3.ResponseBody;
import retrofit2.Response;
import rx.Observable;
import rx.Subscriber;
import rx.android.schedulers.AndroidSchedulers;
import rx.schedulers.Schedulers;

public class ImageViewActivity extends AppCompatActivity {

    @BindView(R.id.image_view2)
    public ImageView imageView;

    @BindView(R.id.button_save)
    public Button buttonSave;

    private ImageService _imageService;
    private ImageTaskService _imageTaskService;

    private MagicianSpinnerDialog _dialog;
    private Bitmap bitmap_merged;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_image_view);
        ButterKnife.bind(this);
        Log.d(ImageViewActivity.class.toString(), "constructor");

        this._imageService = ApiServiceFactory.getImageService();
        this._imageTaskService = ApiServiceFactory.getImageTaskService();

        this._dialog = new MagicianSpinnerDialog();

        Intent intent = getIntent();
        if(intent.hasExtra("imagePath")) {
            String imagePath = intent.getStringExtra("imagePath");
            Log.d("asd", imagePath);
            sendToButler(imagePath);
        }
    }

    @OnClick(R.id.button_save)
    public void clickSaveToGallery() {
        MediaStore.Images.Media.insertImage(getContentResolver(), bitmap_merged, "Styrian neural image", "yolocaust");
        Toast.makeText(imageView.getContext(), "Slika shranjena", Toast.LENGTH_SHORT).show();
        Log.d(ImageViewActivity.class.toString(), "Button load click!");
    }

    private void sendToButler(String imagePath) {
        _dialog.show(getSupportFragmentManager(), "some_tag");
        Log.d("asd", "BUTTON SEND!" + imagePath);

        if (imagePath == null) {
            Log.d("ERROR1", "ni datoteke!!! ne more poslati");
            resetControls();
            return;
        }

        File file = new File(imagePath);

        if (!file.exists()) {
            Log.d("ERROR1", "ni datoteke!!! ne more poslati");
            resetControls();
            return;
        }

        MultipartBody.Part imageFileBody = ImageRequestFactory.getInstance().generatePostRequest(file);

        Observable<ImageTask> onImage = _imageService.post(imageFileBody);
        //_dialog.setMessage("Uploading.");

        onImage.subscribeOn(Schedulers.newThread())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Subscriber<ImageTask>() {
                    @Override
                    public final void onCompleted() {
                    }

                    @Override
                    public final void onError(Throwable e) {
                        Log.d("error", e.getMessage());
                        resetControls();
                    }

                    @Override
                    public final void onNext(ImageTask imageTask) {
                        ImageTaskStatus status = ImageTaskStatus.parse(imageTask.getStatus());

                        _dialog.setMessageAsync(status.getMessage());
                        checkImageTaskStatus(imageTask);
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
                        resetControls();
                    }

                    @Override
                    public final void onNext(Response<ResponseBody> response) {
                        Log.d("success", response.body() + " asd");
                        Log.i("", "neki - success" + response.code());
                        if (response.isSuccessful()) {
                            Log.i("", "neki - success" + response.body().contentLength());
                            InputStream inputStream = response.body().byteStream();
                            bitmap_merged = BitmapFactory.decodeStream(inputStream);

                            imageView.setImageBitmap(bitmap_merged);
                            resetControls();
                        }
                    }
                });
    }

    private Bitmap getBitmap(File file) {
        FileInputStream fis = null;
        try {
            fis = new FileInputStream(file);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
            return null;
        }

        Bitmap bitmap = BitmapFactory.decodeStream(fis);
        return bitmap;
    }

    public void checkImageTaskStatus(final ImageTask imageTaskIn) {
        if (imageTaskIn == null) {
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
                        resetControls();
                    }

                    @Override
                    public final void onNext(ImageTask imageTask) {
                        Log.d(this.getClass().toString(), "imageTask: " + imageTask.getJobId());

                        if (imageTask == null) {
                            Log.d(this.getClass().toString(), "ImageTask in null.");
                            resetControls();
                            return;
                        }

                        ImageTaskStatus status = ImageTaskStatus.parse(imageTask.getStatus());

                        _dialog.setMessageAsync(status.getMessage());

                        if (status == ImageTaskStatus.FINISHED) {
                            getImage(imageTask);
                            return;
                        } else if (status == ImageTaskStatus.ERROR) {
                            Log.d(this.getClass().toString(), "Error in image task.");
                            resetControls();
                            return;
                        } else {
                            Log.d(this.getClass().toString(), "CurrentStatus: " + imageTask.getStatus());
                            checkImageTaskStatus(imageTask);
                            return;
                        }

                    }
                });
    }

    private void resetControls() {
        this._dialog.dismiss();
    }
}
