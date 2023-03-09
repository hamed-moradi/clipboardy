using Assets.Model.Base;
using Assets.Model.Binding;
using Assets.Model.View;
using Assets.Utility.Extension;
using Core.Application._App;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Services {
  public class ClipboardService: BaseService<Clipboard>, IClipboardService {
    #region

    public ClipboardService() {
    }
    #endregion

    public async Task<(List<ClipboardViewModel> List, int TotalCount, int PageCount)> GetPagingAsync(
      ClipboardGetBindingModel collection) {

      var query = Entity.Where(p => p.account_id == collection.AccountId);

      if(collection.TypeId.HasValue) {
        query = query.Where(p => p.type_id == collection.TypeId);
      }

      if(collection.TypeId.HasValue &&
        collection.TypeId == ContentTypeEnum.txt.ToInt() &&
        !string.IsNullOrWhiteSpace(collection.Content)) {

        var keywords = Convert.FromBase64String(collection.Content);
        query = query.Where(p => Encoding.UTF8.GetString(Convert.FromBase64String(p.content)).Contains(collection.Content));
      }

      if(collection.FromDate.HasValue && collection.ToDate.HasValue) {
        query = query.Where(p => p.inserted_at >= collection.FromDate && p.inserted_at <= collection.ToDate);
      }

      var totalCount = query.Count();
      if(totalCount > 0) {
        var pageCount = (int)Math.Ceiling((decimal)totalCount / collection.Take);

        query = query.OrderByField(collection.OrderBy, collection.Order.ToLower() == "asc");
        query = query.Skip(collection.Skip).Take(collection.Take);

        var result = _mapper.Map<List<ClipboardViewModel>>(await query.ToListAsync());
        return (result, totalCount, pageCount);
      }

      return (null, 0, 0);
    }
  }
}
