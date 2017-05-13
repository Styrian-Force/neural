package eu.styrian.neural.magician.api.rest;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class RetrofitClient {
    public static final String BASE_URL = "http://212.235.179.173";

    private static final Object lock = new Object();
    private static volatile Retrofit instance = null;

    public static Retrofit getInstance() {
        Retrofit retrofit = instance;

        if (retrofit == null) {
            synchronized (lock) {
                retrofit = instance;
                if (retrofit == null) {
                    retrofit = new Retrofit.Builder()
                            .baseUrl(BASE_URL)
                            .addConverterFactory(GsonConverterFactory.create())
                            .build();
                    instance = retrofit;
                }
            }
        }
        return retrofit;
    }

    private RetrofitClient() {
    }
}