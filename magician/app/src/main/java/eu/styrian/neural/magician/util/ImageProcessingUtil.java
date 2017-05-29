package eu.styrian.neural.magician.util;

import android.graphics.Bitmap;

/**
 * Created by Gal on 28/05/2017.
 */

public class ImageProcessingUtil {

    public static Bitmap resizeBitmap(Bitmap bitmap, int width, int height) {

        Bitmap resized = Bitmap.createScaledBitmap(bitmap, width, height, true);

        return resized;
    }

}
