using System;
using System.Collections.Generic;

namespace AxGrid.Text {
    public interface ITextRepository {
        Dictionary<string, string> Translations { get; }
        string Get(string key, string def = null);
    }
}