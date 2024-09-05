using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.Cryptograph.Contracts;
    public interface IHashCompare
    {
        Task<bool> CompareAsync(string plain, string hash);
    }

