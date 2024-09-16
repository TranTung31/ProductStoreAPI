using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.DTOs.Common.ResponseNotification
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult()
        {
            IsSuccess = true;
        }

        public ApiSuccessResult(T result)
        {
            IsSuccess = true;
            Data = result;
        }

        public ApiSuccessResult(T result, string message)
        {
            IsSuccess = true;
            Message = message;
            Data = result;
        }

        public ApiSuccessResult(T result, string message, int count)
        {
            IsSuccess = true;
            Message = message;
            Data = result;
            DataTotal = count;
        }
    }
}
