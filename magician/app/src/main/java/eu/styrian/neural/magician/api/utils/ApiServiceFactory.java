package eu.styrian.neural.magician.api.utils;

import eu.styrian.neural.magician.api.interfaces.ImageService;
import eu.styrian.neural.magician.api.interfaces.ImageTaskService;
import eu.styrian.neural.magician.api.interfaces.PostService;
import eu.styrian.neural.magician.api.interfaces.ValueService;
import eu.styrian.neural.magician.api.models.ImageTask;
import eu.styrian.neural.magician.api.rest.RetrofitClient;

public class ApiServiceFactory {

    public static ValueService getValueService() {
        return RetrofitClient.getInstance().create(ValueService.class);
    }

    public static ImageService getImageService() {
        return RetrofitClient.getInstance().create(ImageService.class);
    }

    public static PostService getPostService() {
        return RetrofitClient.getInstance().create(PostService.class);
    }

    public static ImageTaskService getImageTaskService() {
        return RetrofitClient.getInstance().create(ImageTaskService.class);
    }


}