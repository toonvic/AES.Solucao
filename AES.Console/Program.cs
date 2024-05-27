using AES.Console;
using System.Text;


Console.WriteLine("Insira um texto a ser criptografado:");
var entrada = Console.ReadLine();

Console.WriteLine("Insira a chave para ser usada na criptografia:");
var chave = Console.ReadLine();

Console.WriteLine("Insira o nome do arquivo:");
var nomeArquivo = Console.ReadLine();

Console.WriteLine("");

var algoritmo = new AlgoritmoAes();

var textoCriptografado = algoritmo.Criptografar(entrada, chave, nomeArquivo);

File.WriteAllBytes("./log.txt", Encoding.UTF8.GetBytes(algoritmo.ObterLog()));

Console.WriteLine("Criptografado: " + textoCriptografado);