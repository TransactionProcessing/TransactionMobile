using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace TransactionProcessor.Mobile.BusinessLogic.Common;

/// <summary>
/// Provides a single shared factory for building MemoryCacheEntryOptions with consistent
/// absolute-expiry settings across all request handlers.
/// </summary>
internal static class CacheEntryOptionsFactory
{
    /// <summary>
    /// Creates a <see cref="MemoryCacheEntryOptions"/> with an absolute expiry of
    /// <paramref name="minutes"/> minutes from now (UTC), pinned to the cache with
    /// NeverRemove priority, and backed by a matching cancellation-token eviction signal.
    /// </summary>
    internal static MemoryCacheEntryOptions WithAbsoluteExpiry(int minutes)
    {
        DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddMinutes(minutes);
        CancellationChangeToken expirationToken =
            new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromMinutes(minutes)).Token);

        return new MemoryCacheEntryOptions()
            .SetPriority(CacheItemPriority.NeverRemove)
            .SetAbsoluteExpiration(expirationTime)
            .AddExpirationToken(expirationToken);
    }
}
