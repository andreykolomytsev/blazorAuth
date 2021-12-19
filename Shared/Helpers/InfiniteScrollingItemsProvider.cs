using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AuthClient.Shared.Helpers
{
    /// <summary>
    /// Провайдер для бесконечной прокрутки
    /// </summary>
    public sealed class InfiniteScrollingItemsProviderRequest
    {
        public InfiniteScrollingItemsProviderRequest(int startIndex, CancellationToken cancellationToken)
        {
            StartIndex = startIndex;
            CancellationToken = cancellationToken;
        }

        public int StartIndex { get; }

        public CancellationToken CancellationToken { get; }
    }

    public delegate Task<IEnumerable<T>> InfiniteScrollingItemsProviderRequestDelegate<T>(InfiniteScrollingItemsProviderRequest context);
}
