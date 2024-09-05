using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Cryptograph.Contracts;

namespace TaskManager.Infrastructure.Cryptograph
{
    public class HashGenerator : IHashGenerator
    {
        public async Task<string> HashAsync(string plain)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.HashString(plain));
        }
    }
}
