﻿using Assets.Model.Base;
using System;

namespace Core.Domain.StoredProcedure.Result {
    public class AccountProfileResult: IStoredProcResult {
        public int? Id { get; set; }
        public int? AccountId { get; set; }
        public int? TypeId { get; set; }
        public string LinkedId { get; set; }
        public Status? StatusId { get; set; }
    }
}
