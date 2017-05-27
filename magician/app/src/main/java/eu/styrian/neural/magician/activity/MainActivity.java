package eu.styrian.neural.magician.activity;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.ImageView;

import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;
import butterknife.OnClick;
import eu.styrian.neural.magician.R;
import eu.styrian.neural.magician.api.models.Post;
import eu.styrian.neural.magician.api.models.Value;
import eu.styrian.neural.magician.api.utils.ApiServiceFactory;
import rx.Observable;
import rx.Subscriber;
import rx.android.schedulers.AndroidSchedulers;
import rx.functions.Action1;
import rx.schedulers.Schedulers;

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
        //Intent i = new Intent(getApplicationContext(), CameraActivity.class);
        //startActivity(i);

        /*Observable<List<Post>> posts = ApiServiceFactory.getPostService().getAll();

        posts.subscribeOn(Schedulers.newThread())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Subscriber<List<Post>>() {
                    @Override
                    public final void onCompleted() {
                    }

                    @Override
                    public final void onError(Throwable e) {
                    }

                    @Override
                    public final void onNext(List<Post> values) {
                        Log.d("success", values.size() + " asd");
                    }
                });*/

        Observable<List<Value>> posts = ApiServiceFactory.getValueService().getAll();

        posts.subscribeOn(Schedulers.newThread())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Subscriber<List<Value>>() {
                    @Override
                    public final void onCompleted() {
                    }

                    @Override
                    public final void onError(Throwable e) {
                        Log.d("error", e.getMessage());
                    }

                    @Override
                    public final void onNext(List<Value> values) {
                        Log.d("success", values.size() + " asd");
                    }
                });

        Observable<Value> onValue = ApiServiceFactory.getValueService().getById(1);

        onValue.subscribeOn(Schedulers.newThread())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Subscriber<Value>() {
                    @Override
                    public final void onCompleted() {
                    }

                    @Override
                    public final void onError(Throwable e) {
                        Log.d("error", e.getMessage());
                    }

                    @Override
                    public final void onNext(Value value) {
                        Log.d("success", value.getCode() + " asd");
                    }
                });
    }

}
