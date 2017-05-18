package eu.styrian.neural.magician.api.request;

import java.io.File;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;

/**
 * Created by Gal on 13. 05. 2017.
 */

public class ImageRequestFactory {
    private static final Object lock = new Object();
    private static volatile ImageRequestFactory instance = null;

    private ImageRequestFactory() {
    }

    public static ImageRequestFactory getInstance() {
        ImageRequestFactory factory = instance;

        if (factory == null) {
            synchronized (lock) {
                factory = instance;
                if (factory == null) {
                    factory = new ImageRequestFactory();
                    instance = factory;
                }
            }
        }
        return factory;
    }

    public MultipartBody.Part generatePostRequest(File file) {
        RequestBody requestFile = RequestBody.create(MediaType.parse("multipart/form-data"), file);
        MultipartBody.Part imageFileBody = MultipartBody.Part.createFormData("uploaded_file", file.getName(), requestFile);

        return imageFileBody;
    }

}
