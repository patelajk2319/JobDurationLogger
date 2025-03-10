using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
    public interface ILogProcessor
    {
        public List<ErrorModel> ProcessLogFile(string filePath);
    }