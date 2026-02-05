using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacao.Interface
{
    public interface IShortService
    {
        Task<string>GenerateCode(string longUrl);
        Task<string>GetLongUrl(string shortCode);
    }
}
