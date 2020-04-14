using Core.Domain.StoredProcedure.Result;
using Core.Domain.StoredProcedure.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services {
    public class ClipboardService: IClipboardService {
        #region
        private readonly IStoredProcedureService _storedProcedure;

        public ClipboardService(
            IStoredProcedureService storedProcedure) {

            _storedProcedure = storedProcedure;
        }
        #endregion

        public async Task<ClipboardResult> FirstAsync(ClipboardGetFirstSchema clipboard) {
            var result = await _storedProcedure.QueryFirstAsync<ClipboardGetFirstSchema, ClipboardResult>(clipboard);
            return result;
        }

        public async Task<IEnumerable<ClipboardResult>> PagingAsync(ClipboardGetPagingSchema clipboard) {
            var result = await _storedProcedure.QueryAsync<ClipboardGetPagingSchema, ClipboardResult>(clipboard);
            return result;
        }

        public async Task AddAsync(ClipboardAddSchema clipboard) {
            await _storedProcedure.ExecuteAsync(clipboard);
        }

        //public async Task UpdateAsync(clipboardUpdateSchema clipboard) {
        //    await _storedProcedure.ExecuteAsync(clipboard);
        //}
    }
}
