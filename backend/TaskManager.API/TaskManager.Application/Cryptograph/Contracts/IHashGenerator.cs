﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.Cryptograph.Contracts
{
    public interface IHashGenerator
    {
        public Task<string> HashAsync(string plain);
    }
}
