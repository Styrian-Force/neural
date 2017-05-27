package eu.styrian.neural.magician.api.interfaces;


import java.util.List;

import eu.styrian.neural.magician.api.models.Post;
import retrofit2.http.GET;
import retrofit2.http.Path;
import rx.Observable;

/**
 * Created by Gal on 13. 05. 2017.
 */

public interface PostService {

    @GET("/posts/{id}")
    Observable<Post> getById(@Path("id") int id);

    @GET("/posts")
    Observable<List<Post>> getAll();

}

/*
    Observable<Post> post = ApiServiceFactory.getPostService().getById(1);

        post.subscribeOn(Schedulers.newThread())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribe(new Subscriber<Post>() {
@Override
public final void onCompleted() {
        }

@Override
public final void onError(Throwable e) {
        }

@Override
public final void onNext(Post post) {
        Log.d("success", post.getId() + " " + post.getUserId() + " " + post.getBody());
        }
        });
*/