using Microsoft.AspNetCore.Http;

namespace Catalogo.Domain.Utils
{
    public static class ConverterImagem
    {
        public static byte[] ConverterMemoryStream(IFormFile? imagem)
        {
            if (imagem == null || imagem.Length == 0)
                return [];

            using var ms = new MemoryStream();
            imagem.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
