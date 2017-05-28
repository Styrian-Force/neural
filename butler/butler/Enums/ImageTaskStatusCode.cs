
namespace Butler.Models
{
    public enum ImageTaskStatusCode
    {
        ImageUploaded,
        ImageInDetectorQueue,
        ImageInDetector,
        ImageInArtistQueue,
        ImageInArtist,
        ImageInMergeQueue,
        ImageMerging,
        ImageFinished,
        Error
    }
}


