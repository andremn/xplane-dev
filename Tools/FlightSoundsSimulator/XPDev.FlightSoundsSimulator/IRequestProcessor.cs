using System.Threading.Tasks;

namespace XPDev.FlightSoundsSimulator
{
    public interface IRequestProcessor<TParameter>
    {
        Task ProcessRequestAsync(TParameter parameter);
    }

    public interface IRequestProcessor<TResult, TParameter>
    {
        Task<TResult> ProcessRequestAsync(TParameter parameter);
    }
}