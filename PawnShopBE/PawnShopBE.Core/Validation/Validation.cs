using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnShopBE.Core.Validation
{
    public class Validation<T> 
    {
        public async Task<List<ValidationResult>> CheckValidation(T entity)
        {
            ValidationContext context = new ValidationContext(entity);
            // results - lưu danh sách ValidationResult, kết quả kiểm tra
            List<ValidationResult> results = new List<ValidationResult>();
            // thực hiện kiểm tra dữ liệu
            bool valid = Validator.TryValidateObject(entity, context, results, true);
            // In lỗi thông báo
            if (!valid)
            {
                return results;
            }
            return null;
        }

    }
}
