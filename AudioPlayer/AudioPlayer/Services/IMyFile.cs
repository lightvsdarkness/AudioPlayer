using System;
using System.Collections.Generic;
using System.Text;

namespace AudioPlayer.Services
{
    public interface IMyFile
    {
        IList<string> GetFileLocation();
    }
}
