package eu.styrian.neural.magician.api.utils;

import eu.styrian.neural.magician.api.interfaces.SOService;
import eu.styrian.neural.magician.api.rest.RetrofitClient;

public class ApiUtils {

    public static final String BASE_URL = "https://api.stackexchange.com/2.2/";

    public static SOService getSOService() {
        return RetrofitClient.getClient(BASE_URL).create(SOService.class);
    }
}