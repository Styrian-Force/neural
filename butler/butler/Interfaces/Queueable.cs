

namespace Butler.Interfaces
{
    public interface Queueable<T> {
        void AddToQueue(T item);
    }
}