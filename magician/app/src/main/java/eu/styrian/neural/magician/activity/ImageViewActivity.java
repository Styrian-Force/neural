package eu.styrian.neural.magician.activity;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.ImageView;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import eu.styrian.neural.magician.R;

public class ImageViewActivity extends AppCompatActivity {

    @BindView(R.id.image_view2)
    public ImageView imageView;

    @BindView(R.id.button_load2)
    public Button buttonLoad;

    public Bitmap bitmap;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_image_view);
        ButterKnife.bind(this);
        Log.d(ImageViewActivity.class.toString(), "constructor");

        if (getIntent().hasExtra("bitmap")) {
            Bitmap bitmap = getIntent().getParcelableExtra("bitmap");
            imageView.setImageBitmap(bitmap);
            this.bitmap = bitmap;
        }
    }

    @OnClick(R.id.button_load2)
    public void clickToGallery() {
        //Intent i = new Intent(getApplicationContext(), GalleryActivity.class);
        //startActivity(i);
        Log.d(ImageViewActivity.class.toString(), "Button load click!");
    }
}
