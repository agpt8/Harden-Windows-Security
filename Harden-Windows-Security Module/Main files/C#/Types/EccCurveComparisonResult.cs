using System;
using System.Collections.Generic;

#nullable enable

namespace HardenWindowsSecurity
{
    public class EccCurveComparisonResult
    {
        public bool AreCurvesCompliant { get; set; }
        public List<string>? CurrentEccCurves { get; set; }
    }
}