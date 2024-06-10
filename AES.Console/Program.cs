using AES.Console;
using System.Security.Cryptography;
using System.Text;

Console.WriteLine("Insira o caminho + extensão do arquivo a ser criptografado:");
var entrada = Console.ReadLine();

Console.WriteLine("Insira a chave para ser usada na criptografia:");
var chave = Console.ReadLine();

Console.WriteLine("Insira o nome + extensão do arquivo de saída:");
var nomeArquivo = Console.ReadLine();

Console.WriteLine("");


var algoritmo = new AlgoritmoAes();

var criptografado = algoritmo.Criptografar(entrada, chave, nomeArquivo);
