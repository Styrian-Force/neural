package eu.styrian.neural.magician.util;

import android.content.Context;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by matej on 28.5.2017.
 */

public class FileUtil {

    public static File createTemporaryFile (String fileName, String extensionName, Context context) {
        File outputDir = context.getCacheDir(); // context being the Activity pointer
        File outputFile = null;
        try {
            String timeStamp = new SimpleDateFormat("yyyy.MM.dd.HH.mm.ss").format(new Date());
            outputFile = File.createTempFile("picture" + timeStamp, "jpg", outputDir);
        } catch (IOException e) {
            e.printStackTrace();
        }

        return outputFile;
    }

}
