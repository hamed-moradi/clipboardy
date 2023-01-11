using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
  public class ClipboardService: IClipboardService {
    #region

    public ClipboardService() {
    }
    #endregion

    public async Task<ClipboardResult> FirstAsync(ClipboardGetFirstSchema clipboard) {
      return null;
    }

    public async Task<IEnumerable<ClipboardResult>> PagingAsync(ClipboardGetPagingSchema clipboard) {
      return null;
    }

    public async Task<int> AddAsync(ClipboardAddSchema clipboard) {
      return 0;
    }
  }
}
