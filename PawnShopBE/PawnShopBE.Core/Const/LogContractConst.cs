using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Const
{
    public enum LogContractConst
    {
        // Tạo mới hợp đồng
        CREATE_CONTRACT = 1,
        // Trễ hạn đóng tiền lãi
        INTEREST_NOT_PAID = 2,
        // Đã đóng tiền lãi
        INTEREST_PAID = 3,
        // Hợp đồng đã đóng
        CLOSE_CONTRACT = 4,
        // Tiền lãi còn nợ
        INTEREST_DEBT = 5
    }
}
