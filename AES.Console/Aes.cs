using System.Globalization;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace AES.Console;

public class Aes
{
    public void Criptografar()
    {
        var conteudo = "DESENVOLVIMENTO!!";
        var chaveEntrada = "20,1,94,33,199,0,48,9,31,94,112,40,59,30,100,248";

        var conteudoEmBytes = Encoding.ASCII.GetBytes(conteudo);

        var blocos = GerarBlocos(conteudoEmBytes);
    }

    public List<byte[,]> GerarBlocos(byte[] entrada)
    {
        // Calcular o número de blocos necessários
        int quantidadeBlocos = (entrada.Length + 15) / 16;

        var blocos = new List<byte[,]>(quantidadeBlocos);

        // Iterar sobre os blocos
        for (int i = 0; i < quantidadeBlocos; i++)
        {
            var novoBloco = new byte[4, 4];

            // Preencher o bloco atual
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    int indexEntrada = i * 16 + j + k * 4;

                    if (indexEntrada < entrada.Length)
                    {
                        novoBloco[k, j] = entrada[indexEntrada];
                    }
                    else
                    {
                        // Preencher com o valor de padding
                        novoBloco[k, j] = (byte)(16 - entrada.Length % 16);
                    }
                }
            }

            // Adicionar o bloco à lista
            blocos.Add(novoBloco);
        }

        return blocos;
    }

    private void ConverterStringEntradaParaComposicaoEmByte(string entrada)
    {
        try
        {
            Composicao = entrada.Split(',')
                .Select(w => Convert.ToByte(w))
                .ToList();

            if (Composicao.Count != 16)
            {
                throw new ArgumentException("Chave precisa ter 16 bytes.");
            }

            var hex = Convert.ToHexString(Composicao.ToArray());

            //Console.WriteLine($"Chave de entrada - HEX: {hex}");
        }
        catch (Exception e)
        {
            //Console.WriteLine($"Entrada inválida. {e.Message}");
            throw;
        }
    }

    public IEnumerable<byte[]> ObterTodasPalavras()
    {
        var palavras = new List<byte[]>(4);

        for (int j = 0; j < 4; j++)
        {
            var word = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                word[i] = Palavras[i, j];
            }
            palavras.Add(word);
        }
        return palavras;
    }
}
