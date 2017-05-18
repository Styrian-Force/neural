package eu.styrian.neural.magician.activity;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.ImageView;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import eu.styrian.neural.magician.R;

public class MainActivity extends AppCompatActivity {

    @BindView(R.id.button_gallery)
    public Button buttonGallery;

    @BindView(R.id.image_view)
    public ImageView imageView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        ButterKnife.bind(this);
    }

    @OnClick(R.id.button_gallery)
    public void clickToGallery() {
        Intent i = new Intent(getApplicationContext(), GalleryActivity.class);
        startActivity(i);
    }

    @OnClick(R.id.button_camera)
    public void clickToCamera() {
        Log.d("TAG", "GOTO CAMERA");
        Intent i = new Intent(getApplicationContext(), CameraActivity.class);
        startActivity(i);
    }

}
