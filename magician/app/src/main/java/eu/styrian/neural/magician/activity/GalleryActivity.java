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
import android.support.v4.app.FragmentManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
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
import eu.styrian.neural.magician.api.models.Value;
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

public class GalleryActivity extends AppCompatActivity {

    private static final int SELECT_PICTURE = 1;

    private String selectedImagePath;

    @BindView(R.id.image_view)
    public ImageView imageView;

    @BindView(R.id.button_load)
    public Button buttonLoad;

    @BindView(R.id.button_send)
    public Button buttonSend;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_gallery);
        ButterKnife.bind(this);
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == RESULT_OK) {
            if (requestCode == SELECT_PICTURE) {
                Uri selectedImageUri = data.getData();

                selectedImagePath = FileUtil.getFilePath(this, selectedImageUri);

                imageView.setImageURI(selectedImageUri);
                Log.d("", "ssss" + selectedImagePath);
            }
        }
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
        Intent intent = new Intent(getApplicationContext(), ImageViewActivity.class);
        intent.putExtra("imagePath", this.selectedImagePath);
        startActivity(intent);
    }

    private void resetControls() {
        this.buttonSend.setEnabled(true);
    }


}
