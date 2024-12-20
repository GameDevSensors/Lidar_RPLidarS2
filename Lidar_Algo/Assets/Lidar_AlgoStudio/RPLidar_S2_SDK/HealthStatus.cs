﻿using System;
using System.Collections.Generic;

namespace RPLidar
{
    /// <summary>
    /// Health status
    /// </summary>
    public enum HealthStatus : byte
    {
        Good = 0,
        Warning = 1,
        Error = 2,
        Unknown = 0xFF
    }
}
