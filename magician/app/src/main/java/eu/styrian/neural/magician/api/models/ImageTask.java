package eu.styrian.neural.magician.api.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class ImageTask {

    @SerializedName("status")
    @Expose
    private Integer status;
    @SerializedName("jobId")
    @Expose
    private String jobId;
    @SerializedName("croppedImages")
    @Expose
    private List<CroppedImage> croppedImages = null;
    @SerializedName("originalExtension")
    @Expose
    private String originalExtension;

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public String getJobId() {
        return jobId;
    }

    public void setJobId(String jobId) {
        this.jobId = jobId;
    }

    public List<CroppedImage> getCroppedImages() {
        return croppedImages;
    }

    public void setCroppedImages(List<CroppedImage> croppedImages) {
        this.croppedImages = croppedImages;
    }

    public String getOriginalExtension() {
        return originalExtension;
    }

    public void setOriginalExtension(String originalExtension) {
        this.originalExtension = originalExtension;
    }

}