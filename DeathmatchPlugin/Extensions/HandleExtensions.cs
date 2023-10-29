using CounterStrikeSharp.API.Modules.Utils;

namespace DeathmatchPlugin.Extensions
{
    public static class HandleExtensions
    {
        public static T? GetValueOrNull<T>(this CHandle<T> handle) where T : class
        {
            return !handle.IsValid ? null : handle.Value;
        }
    }
}