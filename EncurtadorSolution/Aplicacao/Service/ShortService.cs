using Aplicacao.DTO;
using Aplicacao.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace Aplicacao.Service
{
    public class ShortService: IShortService
    {
        private static long _currentId = 0; // Em produção, use banco de dados com AUTO_INCREMENT

        // Thread-safe storage para mapear shortcode -> longUrl
        private static readonly ConcurrentDictionary<string, string> _urlStorage = new();

        // Thread-safe storage para evitar duplicação de URLs (cache)
        private static readonly ConcurrentDictionary<string, string> _reverseStorage = new();

        public ShortService() { }

        public Task<string> GetLongUrl(string shortcode)
        {
            if (string.IsNullOrEmpty(shortcode))
            {
                throw new ArgumentNullException(nameof(shortcode), "Shortcode cannot be null or empty");
            }

            if (_urlStorage.TryGetValue(shortcode, out var longUrl))
            {
                return Task.FromResult(longUrl);
            }

            throw new KeyNotFoundException($"Shortcode '{shortcode}' not found");
        }

        public async Task<UrlDTO> UrlCode(UrlDTO urlDto)
        {
            if(urlDto == null || string.IsNullOrEmpty(urlDto.LongUrl))
            {
                throw new ArgumentNullException("urlDto or LongUrl is null");
            }

            // Verifica se a URL já foi encurtada (evita duplicação)
            if (_reverseStorage.TryGetValue(urlDto.LongUrl, out var existingShortcode))
            {
                return new UrlDTO 
                { 
                    Shortcode = existingShortcode,
                    LongUrl = urlDto.LongUrl
                };
            }

            try
            {
                // Gera ID sequencial thread-safe
                long sequentialId = Interlocked.Increment(ref _currentId);

                // Converte para Base62 com mínimo de 6 caracteres
                string shortcode = Utils.Base62Encoder.Encode(sequentialId, minLength: 6);

                // Armazena o mapeamento
                _urlStorage[shortcode] = urlDto.LongUrl;
                _reverseStorage[urlDto.LongUrl] = shortcode;

                return new UrlDTO 
                { 
                    Shortcode = shortcode,
                    LongUrl = urlDto.LongUrl
                };
            }
            catch(Exception ex)
            {
                throw new Exception("Error generating shortcode", ex);
            }
        }

        // Método auxiliar para obter estatísticas (útil para performance testing)
        public int GetTotalUrls()
        {
            return _urlStorage.Count;
        }
    }
}
