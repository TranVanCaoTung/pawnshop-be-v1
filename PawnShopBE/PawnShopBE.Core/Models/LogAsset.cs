﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Models
{
    public class LogAsset
    {
        public int logAssetId { get; set; }
        public int contractAssetId { get; set; }
        public string UserName { get; set; }
        public string WareHouseName{get;set; }
        public string? ImportImg { get;set; }
        public string? ExportImg { get; set; }
        public string? Description { get; set;}
        public DateTime CreateDate { get; set; }
        [JsonIgnore]
        public virtual ContractAsset? ContractAsset { get; set; }
    }
}
