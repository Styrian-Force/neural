package eu.styrian.neural.magician.api.utils;

import eu.styrian.neural.magician.api.interfaces.ImageService;
import eu.styrian.neural.magician.api.interfaces.SOService;
import eu.styrian.neural.magician.api.interfaces.ValueService;
import eu.styrian.neural.magician.api.rest.RetrofitClient;

public class ApiUtils {

    public static final String BASE_URL_SO = "https://api.stackexchange.com/2.2/";
    public static final String BASE_URL = "http://212.235.179.173";

    public static SOService getSOService() {
        return RetrofitClient.getClient(BASE_URL_SO).create(SOService.class);
    }

    public static ValueService getValueService() {
        return RetrofitClient.getClient(BASE_URL).create(ValueService.class);
    }

    public static ImageService getImageService() {
        return RetrofitClient.getClient(BASE_URL).create(ImageService.class);
    }


}