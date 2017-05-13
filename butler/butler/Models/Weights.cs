
public class Weights
{
    public static Weights TINY_YOLO_VOC  = new Weights("cfg/tiny-yolo-voc.cfg", "weights/tiny-yolo-voc.weights");
    public static Weights YOLO_VOC  = new Weights("cfg/yolo-voc.cfg", "weights/yolo-voc.weights");
    public static Weights YOLO  = new Weights("cfg/yolo.cfg", "weights/yolo.weights");
    public string CfgPath;
    public string WeightsPath;

    public Weights(string cfg, string weights) {
        this.CfgPath = cfg;
        this.WeightsPath = weights;
    }
    
}