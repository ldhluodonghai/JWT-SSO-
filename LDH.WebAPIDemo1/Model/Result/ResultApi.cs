using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDH.WebAPIDemo1el.Model.Result
{
    public class ResultApi
    {
        public bool IsSuccess { get; set; }
        public object? Result { get; set; }
        public string? Msg { get; set; }
    }
}
