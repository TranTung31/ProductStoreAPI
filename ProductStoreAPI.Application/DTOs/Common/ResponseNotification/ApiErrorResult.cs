using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.DTOs.Common.ResponseNotification
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public ApiErrorResult()
        {
            IsSuccess = false;
        }

        public ApiErrorResult(string message)
        {
            IsSuccess = false;
            Message = message;
        }
    }
}
