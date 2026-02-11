using System;

namespace Aplicacao.Utils
{
    public static class Base62Encoder
    {
        private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int Base = 62;
        private const int MinLength = 6; // Comprimento mínimo para URLs

        public static string Encode(long value, int minLength = MinLength)
        {
            if (minLength > 10) throw new ArgumentException("minLength cannot exceed 10");

            // Adiciona offset para evitar IDs muito pequenos e começar com strings mais longas
            value += (long)Math.Pow(Base, minLength - 1);

            if (value == 0)
            {
                return Base62Chars[0].ToString().PadLeft(minLength, Base62Chars[0]);
            }

            Span<char> buffer = stackalloc char[11];
            int index = buffer.Length;

            while (value > 0)
            {
                buffer[--index] = Base62Chars[(int)(value % Base)];
                value /= Base;
            }

            return new string(buffer.Slice(index));
        }

        public static long Decode(ReadOnlySpan<char> encoded)
        {
            long result = 0;
            long multiplier = 1;

            for (int i = encoded.Length - 1; i >= 0; i--)
            {
                int charValue = Base62Chars.IndexOf(encoded[i]);
                if (charValue == -1) throw new ArgumentException("Invalid Base62 character");
                
                result += charValue * multiplier;
                multiplier *= Base;
            }

            return result;
        }
    }
}