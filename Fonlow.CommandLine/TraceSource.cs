using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Fonlow.CommandLineGui
{
    public sealed class AppTraceSource : TraceSource
    {
        private AppTraceSource() : base("Application")
        {
            
        }

        public static AppTraceSource Instance { get { return Nested.instance; } }

        private static class Nested
        {
            static Nested()
            {
            }

            internal static readonly AppTraceSource instance = new AppTraceSource();
        }
    }


    public static class TraceSourceExtension
    {
        public static void TraceWarning(this TraceSource traceSource, string message)
        {
            traceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }

        public static void TraceWarning(this TraceSource traceSource, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        public static void TraceError(this TraceSource traceSource, string message)
        {
            traceSource.TraceEvent(TraceEventType.Error, 0, message);
        }

        public static void TraceError(this TraceSource traceSource, string format, params object[] args)
        {
            traceSource.TraceEvent(TraceEventType.Error, 0, format, args);
        }
    }

}
