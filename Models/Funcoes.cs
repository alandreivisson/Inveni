using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Inveni.Models {
    public class Funcoes {
        public static string Criptografar(string texto) {
            using (SHA512 sha512 = new SHA512Managed())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hashBytes = sha512.ComputeHash(bytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public static string Hash(string textoLimpo) {
            HashAlgorithm algoritmo = HashAlgorithm.Create("SHA-512");
            if (algoritmo == null)
            {
                throw new ArgumentException("Hash nãó válida!");
            }
            else
            {
                byte[] hash = algoritmo.ComputeHash(Encoding.UTF8.GetBytes(textoLimpo));
                return Convert.ToBase64String(hash);
            }
        }
        public static string EncodeId(int id) {
            // Converte o ID para bytes e codifica para base64
            byte[] idBytes = BitConverter.GetBytes(id);
            string idCodificado = Convert.ToBase64String(idBytes);

            // Remove caracteres especiais da string codificada
            idCodificado = idCodificado.TrimEnd('=');

            return idCodificado;
        }

        public static int DecodeId(string idCodificado) {
            // Adiciona os caracteres especiais removidos na codificação
            while (idCodificado.Length % 4 != 0)
            {
                idCodificado += "=";
            }

            // Decodifica a string base64 para bytes e converte para int
            byte[] idBytes = Convert.FromBase64String(idCodificado);
            int idDecodificado = BitConverter.ToInt32(idBytes, 0);

            return idDecodificado;
        }
    }
}
