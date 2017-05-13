
namespace Butler.Config
{
    public static class ButlerConfig
    {
        public static string NEURAL_GIT_DIR = "/home/administrator/dev/git/neural/";
        public static string DATABASE_DIR = "/home/administrator/dev/neural/database/";
        public static string DETECTOR_SUBDIR = "detector/";
        public static string ARTIST_SUBDIR = "artist/";

        public static string ORIGINAL_FILENAME = "input";
        public static string DETECTOR_OUTPUT = "detector_output";
        public static string TRANSPARENT_OUTPUT = "transparent_output";
        public static string DETECTOR_OUTPUT_WITH_EXT = DETECTOR_OUTPUT + ".png";
        public static string MERGED_OUTPUT = "merged_output.png";
    }
}