package eu.styrian.neural.magician.activity;

import android.content.Intent;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.ImageView;

import java.io.File;

import butterknife.BindView;
import butterknife.ButterKnife;
import eu.styrian.neural.magician.R;

public class CameraImageActivity extends AppCompatActivity {

    @BindView(R.id.camera_image_view)
    public ImageView imageView;

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
}


