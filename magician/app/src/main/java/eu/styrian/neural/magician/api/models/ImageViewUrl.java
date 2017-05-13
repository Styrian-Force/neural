package eu.styrian.neural.magician.api.models;

import android.widget.ImageView;

/**
 * Created by Gal on 13. 05. 2017.
 */

public class ImageViewUrl {
    private ImageView imageView;
    private String url;

    public ImageView getImageView() {
        return imageView;
    }

    public void setImageView(ImageView imageView) {
        this.imageView = imageView;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }
}
