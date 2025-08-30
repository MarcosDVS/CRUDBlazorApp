﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared;

public class ApiResponse<T>
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public T? Data { get; set; }
}
