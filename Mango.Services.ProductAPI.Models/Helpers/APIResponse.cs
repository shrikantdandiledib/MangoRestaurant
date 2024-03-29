﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Services.ProductAPI.Models.Helpers
{
    public class APIResponse<T>
    {
        public bool Success { get; set; } = true;
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Exception { get; set; }
    }
}
