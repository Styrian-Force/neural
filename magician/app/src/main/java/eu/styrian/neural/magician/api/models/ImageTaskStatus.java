package eu.styrian.neural.magician.api.models;

/**
 * Created by Gal on 28/05/2017.
 */

public class ImageTaskStatus {
    public static ImageTaskStatus UPLOADED = new ImageTaskStatus(0, "Uploaded.");
    public static ImageTaskStatus IN_DETECTOR_QUEUE = new ImageTaskStatus(1, "In Detector queue.");
    public static ImageTaskStatus IN_DETECTOR = new ImageTaskStatus(2, "In Detector.");
    public static ImageTaskStatus IN_IMAGE_QUEUE = new ImageTaskStatus(3, "In Artist queue.");
    public static ImageTaskStatus IN_ARTIST = new ImageTaskStatus(4, "In Artist.");
    public static ImageTaskStatus IN_MERGE_QUEUE = new ImageTaskStatus(5, "In merge queue.");
    public static ImageTaskStatus MERGING = new ImageTaskStatus(6, "Is merging.");
    public static ImageTaskStatus FINISHED = new ImageTaskStatus(7, "Finished!");
    public static ImageTaskStatus ERROR = new ImageTaskStatus(8, "Error on server.");


    private final int code;
    private final String message;

    private ImageTaskStatus(int code, String message) {
        this.code = code;
        this.message = message;
    }

    public static ImageTaskStatus parse(int code) {

        switch (code) {
            case 0:
                return UPLOADED;
            case 1:
                return IN_DETECTOR_QUEUE;
            case 2:
                return IN_DETECTOR;
            case 3:
                return IN_IMAGE_QUEUE;
            case 4:
                return IN_ARTIST;
            case 5:
                return IN_MERGE_QUEUE;
            case 6:
                return MERGING;
            case 7:
                return FINISHED;
            default:
                return ERROR;
        }
    }

    @Override
    public boolean equals(Object status) {
        if (status == null) {
            return false;
        }

        if (!(status instanceof ImageTaskStatus)) {
            return false;
        }

        return this.code == ((ImageTaskStatus) status).code;
    }

    public int getCode() {
        return this.code;
    }
    public String getMessage() { return this.message; }
}
