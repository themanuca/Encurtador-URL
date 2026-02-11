using Aplicacao.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacao.Interface
{
    public interface IShortService
    {
        Task<string>GetLongUrl(string urlDto);
        Task<UrlDTO>UrlCode(UrlDTO urlDto);
    }
}
