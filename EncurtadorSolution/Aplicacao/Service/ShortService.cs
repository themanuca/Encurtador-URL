using Aplicacao.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacao.Service
{
    public class ShortService: IShortService
    {
        public ShortService() { }

        public Task<string> GenerateCode(string longUrl)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLongUrl(string shortCode)
        {
            throw new NotImplementedException();
        }
    }
}
