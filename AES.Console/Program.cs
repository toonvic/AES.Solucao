using AES.Console;
using System.Security.Cryptography;
using System.Text;

Console.WriteLine("Insira um texto a ser criptografado:");
var entrada = Console.ReadLine();

Console.WriteLine("Insira a chave para ser usada na criptografia:");
var chave = Console.ReadLine();

Console.WriteLine("Insira o nome do arquivo:");
var nomeArquivo = Console.ReadLine();

Console.WriteLine("");

var decifrado = "";

var algoritmo = new AlgoritmoAes();

var textoCriptografado = algoritmo.Criptografar(entrada, chave, nomeArquivo);

Console.WriteLine("Criptografado: " + textoCriptografado);

var composicao = chave
                   .Split(',')
                   .Select(
                       w => Convert.ToByte(w)
                   )
                   .ToList();

using (Aes aesAlg = Aes.Create())
{
    aesAlg.Key = composicao.ToArray();
    aesAlg.Mode = CipherMode.ECB;
    aesAlg.Padding = PaddingMode.PKCS7;

    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

    byte[] textoBytes = Encoding.UTF8.GetBytes(entrada);

    byte[] textoCifradoBytes = encryptor.TransformFinalBlock(textoBytes, 0, textoBytes.Length);

    var cifrado = Convert.ToBase64String(textoCifradoBytes);

    Console.WriteLine("Cifrado por lib: " + cifrado);
}

using (Aes aesAlg = Aes.Create())
{
    aesAlg.Key = composicao.ToArray();
    aesAlg.Mode = CipherMode.ECB;
    aesAlg.Padding = PaddingMode.PKCS7;

    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

    byte[] textoCifradoBytes = Convert.FromBase64String(textoCriptografado);

    byte[] textoDescriptografadoBytes = decryptor.TransformFinalBlock(textoCifradoBytes, 0, textoCifradoBytes.Length);

    decifrado = Encoding.UTF8.GetString(textoDescriptografadoBytes);

    Console.WriteLine("Decifrado por lib: " + decifrado);
}
