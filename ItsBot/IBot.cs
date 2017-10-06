using System.Threading.Tasks;

namespace ItsBot
{
    // ToDo: Expand on this as its functionality is determined.

    /// <summary>
    /// Defines what a "bot" can publicly do.
    /// </summary>
    public interface IBot
    {
        Task RunOnceAsync();
    }
}
