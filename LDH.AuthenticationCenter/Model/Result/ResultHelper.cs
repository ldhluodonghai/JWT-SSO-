using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Result
{
    public class ResultHelper
    {
        public static ResultApi Success(dynamic data)
        {
            return new ResultApi() { IsSuccess = true, Result = data, Msg = "操作成功" };
        }
        public static ResultApi Error(string msg ) {
            return new ResultApi() {Msg=msg,IsSuccess=false,Result=null };

        }
    }
}
