package eu.styrian.neural.magician.api.models;

/**
 * Created by Gal on 28/05/2017.
 */


import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class CroppedImage {

    @SerializedName("id")
    @Expose
    private Integer id;
    @SerializedName("left")
    @Expose
    private Integer left;
    @SerializedName("top")
    @Expose
    private Integer top;
    @SerializedName("width")
    @Expose
    private Integer width;
    @SerializedName("heigth")
    @Expose
    private Integer heigth;

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getLeft() {
        return left;
    }

    public void setLeft(Integer left) {
        this.left = left;
    }

    public Integer getTop() {
        return top;
    }

    public void setTop(Integer top) {
        this.top = top;
    }

    public Integer getWidth() {
        return width;
    }

    public void setWidth(Integer width) {
        this.width = width;
    }

    public Integer getHeigth() {
        return heigth;
    }

    public void setHeigth(Integer heigth) {
        this.heigth = heigth;
    }

}