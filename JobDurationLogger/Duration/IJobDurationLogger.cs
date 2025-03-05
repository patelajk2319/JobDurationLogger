using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
    
    public interface IJobDurationLogger
    {
        public bool TookLongerThanExpected(double durationSeconds, int MinMinutes, int  MaxMinutes) ;

    }