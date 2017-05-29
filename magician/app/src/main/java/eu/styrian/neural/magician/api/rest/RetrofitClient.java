package eu.styrian.neural.magician.api.rest;

import retrofit2.Retrofit;
import retrofit2.adapter.rxjava.RxJavaCallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;

public class RetrofitClient {
    //public static final String BASE_URL = "http://192.168.1.165";
    //public static final String BASE_URL = "http://jsonplaceholder.typicode.com";
    //public static final String BASE_URL = "http://95.87.159.189";
    public static final String BASE_URL = "http://192.168.1.165:5001";

    private static final Object lock = new Object();
    private static volatile Retrofit instance = null;

    public static Retrofit getInstance() {
        Retrofit retrofit = instance;

        if (retrofit == null) {
            synchronized (lock) {
                retrofit = instance;
                if (retrofit == null) {
                    RxJavaCallAdapterFactory rxAdapter = RxJavaCallAdapterFactory.create();
                    retrofit = new Retrofit.Builder()
                            .baseUrl(BASE_URL)
                            .addConverterFactory(GsonConverterFactory.create())
                            .addCallAdapterFactory(rxAdapter)
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