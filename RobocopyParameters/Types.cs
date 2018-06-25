using System;
using System.ComponentModel;

namespace Fonlow.CommandLineGui.Robocopy
{
    [Flags]
    [TypeConverter(typeof(FlaggedEnumConverter<CopyFlags>))]
    public enum CopyFlags : int
    {
        None = 0, D = 1, A = 2, T = 4, S = 8, O = 16, U = 32
    }

    /// <summary>
    /// R – Read only A – Archive S – System
    /// H – Hidden C – Compressed N – Not content indexed
    /// E – Encrypted T – Temporary O - Offline
    /// </summary>
    [Flags]
    [TypeConverter(typeof(FlaggedEnumConverter<Rashcneto>))]
    public enum Rashcneto : int
    {
        None = 0, R = 1, A = 2, S = 4, H = 8, C = 16, N = 32, E = 64, T = 128, O = 256
    }

}
