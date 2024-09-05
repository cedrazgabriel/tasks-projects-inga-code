using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Cryptograph.Contracts;

namespace TaskManager.Infrastructure.Cryptograph
{
    public class HashCompare : IHashCompare
    {
        public async Task<bool> CompareAsync(string plain, string hash)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(plain, hash));
        }
    }
}
