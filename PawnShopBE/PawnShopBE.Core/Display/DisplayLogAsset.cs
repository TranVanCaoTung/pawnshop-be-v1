using PawnShopBE.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Display
{
    public class DisplayLogAsset
    {
        public int logAssetId { get; set; }
        public int contractAssetId { get; set; }
        public string AssetName { get; set; }
        public string UserName { get; set; }
        public string WareHouseName { get; set; }
        public string? ImportImg { get; set; }
        public string? ExportImg { get; set; }
        public string? Description { get; set; }
 
    }
}
