using System.Collections.Generic;

namespace AxGrid.Text {
    public interface ITextRepository {
        Dictionary<string, object> Translations { get; }
    }
}