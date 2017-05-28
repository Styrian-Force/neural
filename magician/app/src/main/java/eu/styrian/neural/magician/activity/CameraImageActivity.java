package eu.styrian.neural.magician.activity;

import android.content.Intent;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;

import java.io.File;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import eu.styrian.neural.magician.R;

public class CameraImageActivity extends AppCompatActivity {

    @BindView(R.id.camera_image_view)
    public ImageView imageView;

    @BindView(R.id.button_accept_image)
    public Button buttonAccept;

    @BindView(R.id.button_reject_image)
    public Button buttonReject;

    private File imageFile;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_camera_image);
        ButterKnife.bind(this);

        Intent intent = getIntent();
        String imageFilePath = intent.getExtras().getString("imageFilePath");
        imageFile = new File(imageFilePath);
        Uri uri = Uri.fromFile(imageFile);
        imageView.setImageURI(uri);
    }

    @OnClick(R.id.button_accept_image)
    public void acceptImage(View view) {
        Log.d("asd", "BUTTON ACCEPT IMAGE!");
    }

    @OnClick(R.id.button_reject_image)
    public void rejectImage(View view) {
        Log.d("asd", "BUTTON REJECT IMAGE!");
        imageFile.delete();
        toCameryActivity();
    }

    public void toCameryActivity() {
        Intent i = new Intent(getApplicationContext(), CameraActivity.class);
        startActivity(i);
    }
}


