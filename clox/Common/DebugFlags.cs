public static class DebugFlags
{
#if DEBUG
    public const bool PrintCode = true;
    public const bool TraceExecution = true;
#else
    public const bool PrintCode = false;
    public const bool TraceExecution = false;
#endif
}