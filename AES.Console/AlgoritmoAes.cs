using System.Globalization;
using System.Text;

namespace AES.Console;

public class AlgoritmoAes
{
    private byte[,] MatrizDeEstado { get; set; } = new byte[4,4];

    public void Criptografar(string entradaUsuario)
    {
        var entrada = Encoding.ASCII.GetBytes(entradaUsuario);

        var chave = new byte[] { 20, 1, 94, 33, 199, 0, 48, 9, 31, 94, 112, 40, 59, 30, 100, 248 };

        if (chave.Length != 16)
            throw new ArgumentException("Chave com tamanho inválido.");

        for (int col = 0; col < 4; col++)
        {
            for (int row = 0; row < 4; row++)
            {
                MatrizDeEstado[row, col] = chave[col * 4 + row];
            }
        }

        var palavras = ObterPalavrasMatrizDeEstado();
        var keySchedule = GerarKeySchedule(palavras);
        var blocos = PrepararBlocos(entrada);

        //Acho que aqui não está funcionando
        ExecutarCriptografia(blocos, keySchedule, "./testando.txt");
    }

    public List<byte[]> ObterPalavrasMatrizDeEstado() =>
        Enumerable
        .Range(0, 4)
        .Select(j => Enumerable.Range(0, 4)
            .Select(i => MatrizDeEstado[i, j])
            .ToArray()
        )
        .ToList();


    private IList<List<byte[]>> GerarKeySchedule(List<byte[]> expansaoChave)
    {
        var keySchedule = new List<List<byte[]>>(11) { expansaoChave };

        for (int i = 1; i < 11; i++)
        {
            var roundKeyAnterior = keySchedule[i - 1];

            var primeiraPalavra = roundKeyAnterior[0];
            var ultimaPalavra = roundKeyAnterior[3];

            var primeiraPalavraRoundKey = GerarPrimeiraPalavraRoundKey(primeiraPalavra, ultimaPalavra, i);
            var roundKeyNova = new List<byte[]> { primeiraPalavraRoundKey };

            for (int j = 0; j < 3; j++)
            {
                roundKeyNova.Add(ExecutarXorBytes(roundKeyAnterior[j + 1], roundKeyNova[j]));
            }

            keySchedule.Add(roundKeyNova);
        }

        return keySchedule;
    }

    private byte[] GerarPrimeiraPalavraRoundKey(byte[] primeiraPalavra, byte[] ultimaPalavra, int indexRoundKey)
    {
        var rotWord = GerarRotWord(ultimaPalavra);

        var subWord = GerarSubWord(rotWord);

        var roundConstant = GerarRoundConstant(indexRoundKey);

        var resultadoXor = ExecutarXorBytes(subWord, roundConstant);

        return ExecutarXorBytes(primeiraPalavra, resultadoXor);
    }

    private byte[] GerarRotWord(byte[] palavra) =>
        new byte[] { palavra[1], palavra[2], palavra[3], palavra[0] };

    private byte[] GerarSubWord(byte[] rotWord) =>
        rotWord.Select(b =>
        {
            var highNibble = (b >> 4) & 0x0F;
            var lowNibble = b & 0x0F;
            return Constantes.MatrizSBox[highNibble, lowNibble];
        }).ToArray();

    private byte[] GerarRoundConstant(int indexRoundKey) =>
        new byte[] { Constantes.RoundConstant[indexRoundKey - 1], 0, 0, 0 };

    private byte[] ExecutarXorBytes(byte[] primeiroValor, byte[] segundoValor) =>
        primeiroValor.Zip(segundoValor, (x, y) => (byte)(x ^ y)).ToArray();

    private IList<byte[,]> PrepararBlocos(byte[] entrada)
    {
        var blocos = new List<byte[,]>();

        int quantidadeBlocos = (int)Math.Ceiling((double)entrada.Length / 16);
        int bytesRestantes = 16 - (entrada.Length % 16);

        for (int i = 0; i < quantidadeBlocos; i++)
        {
            var bloco = new byte[4, 4];
            int inicio = i * 16;

            for (int j = 0; j < 16; j++)
            {
                if (inicio + j < entrada.Length)
                {
                    bloco[j % 4, j / 4] = entrada[inicio + j];
                }
                else
                {
                    bloco[j % 4, j / 4] = (byte)bytesRestantes;
                }
            }

            blocos.Add(bloco);
        }

        return blocos;
    }

    private string ExecutarCriptografia(IList<byte[,]> blocos, IList<List<byte[]>> keySchedule, string nomeArquivoDeSaida)
    {
        var matrizResultado = new byte[4, 4];
        var cifra = new List<byte>();
        foreach (var x in blocos.Select((value, index) => new { value, index }))
        {
            var bloco = x.value;
            //Console.WriteLine($"************************ Criptografando Bloco #{x.index + 1} ************************");
            matrizResultado = XorBytes(bloco, keySchedule[0]);
            for (int i = 1; i < 10; i++)
            {
                matrizResultado = SubBytes(matrizResultado);
                matrizResultado = ShiftRows(matrizResultado);
                matrizResultado = MixColumns(matrizResultado);
                matrizResultado = XorBytes(matrizResultado, keySchedule[i]);
            }

            matrizResultado = SubBytes(matrizResultado);
            matrizResultado = ShiftRows(matrizResultado);
            matrizResultado = XorBytes(matrizResultado, keySchedule[10]);
            //Console.WriteLine($"************************ Fim criptografia Bloco #{x.index + 1} ************************");

            cifra.AddRange(ObterCifra(matrizResultado));
        }

        var pathArquivoCriptografado = "./texto.txt";

        using (var file = File.Create(pathArquivoCriptografado))
        {
            file.Write(cifra.ToArray());
            file.Flush();
            file.Close();
        }

        return pathArquivoCriptografado;
    }

    private byte[,] XorBytes(byte[,] matrizEstado, IList<byte[]> roundKeyAtual)
    {
        var matrizEstadoResultado = new byte[4, 4];
        for (var i = 0; i < matrizEstado.GetLength(0); i++)
        {
            for (int j = 0; j < matrizEstado.GetLength(1); j++)
            {
                var wordMomentoKeySchedule = roundKeyAtual[i];
                var primeiroElemento = matrizEstado[j, i];
                var segundoElemento = wordMomentoKeySchedule[j];
                var byteDoMomento = (byte)(primeiroElemento ^ segundoElemento);

                matrizEstadoResultado[j, i] = byteDoMomento;
            }
        }

        return matrizEstadoResultado;
    }

    private byte[,] SubBytes(byte[,] matrizEstado)
    {
        var matrizEstadoResultante = new byte[4, 4];
        for (int i = 0; i < matrizEstado.GetLength(0); i++)
        {
            for (int j = 0; j < matrizEstado.GetLength(1); j++)
            {
                var byteIteradoDaMatrizEstado = matrizEstado[i, j];
                var (intQuatroBitsMaisSignificativos, intQuatroBitsMenosSignificativos) = ObterBitMaisEMenosSignificativo(byteIteradoDaMatrizEstado);
                var byteParaSubstituir = Constantes.MatrizSBox[intQuatroBitsMaisSignificativos, intQuatroBitsMenosSignificativos];

                matrizEstadoResultante[i, j] = byteParaSubstituir;
            }
        }

        return matrizEstadoResultante;
    }

    private byte[] SubWord(byte[] rotWord)
    {
        Span<byte> subWord = new byte[4];
        for (int i = 0; i < rotWord.Length; i++)
        {
            var byteIteradoDaRotWord = rotWord[i];
            (var intQuatroBitsMaisSignificativos, var intQuatroBitsMenosSignificativos) = ObterBitMaisEMenosSignificativo(byteIteradoDaRotWord);

            var byteParaSubstituir = Constantes.MatrizSBox[intQuatroBitsMaisSignificativos, intQuatroBitsMenosSignificativos];
            subWord[i] = byteParaSubstituir;
        }

        var subWordArray = subWord.ToArray();
        return subWordArray;
    }

    private byte[,] ShiftRows(byte[,] matrizEstado)
    {
        byte[,] matrizEstadoResultado = (byte[,])matrizEstado.Clone();

        for (int i = 1; i < matrizEstadoResultado.GetLength(0); i++)
        {
            var linhaAtual = new byte[4];
            for (int j = 0; j < linhaAtual.Length; j++)
            {
                linhaAtual[j] = matrizEstadoResultado[i, j];
            }

            int quantidadeShifts = i;
            for (int j = quantidadeShifts; j > 0; j--)
            {
                linhaAtual = RotWord(linhaAtual);
            }

            for (int k = 0; k < linhaAtual.Length; k++)
            {
                matrizEstadoResultado[i, k] = linhaAtual[k];
            }
        }

        return matrizEstadoResultado;
    }

    private byte[] RotWord(byte[] word)
    {
        var resultado = new byte[4];
        word.CopyTo(resultado, 0);

        var primeiroByte = word[0];
        for (int i = 0; i < word.Length - 1; i++)
        {
            resultado[i] = word[i + 1];
        }

        resultado[word.Length - 1] = primeiroByte;
        return resultado;
    }

    private byte[,] MixColumns(byte[,] matrizEstado)
    {
        byte[,] matrizResultado = new byte[matrizEstado.GetLength(0), matrizEstado.GetLength(1)];

        for (int i = 0; i < matrizEstado.GetLength(0); i++)
        {
            var r1 = matrizEstado[0, i];
            var r2 = matrizEstado[1, i];
            var r3 = matrizEstado[2, i];
            var r4 = matrizEstado[3, i];

            matrizResultado[0, i] = (byte)(MultiplicacaoGalois(r1, 2) ^
                                           MultiplicacaoGalois(r2, 3) ^
                                           MultiplicacaoGalois(r3, 1) ^
                                           MultiplicacaoGalois(r4, 1));


            matrizResultado[1, i] = (byte)(MultiplicacaoGalois(r1, 1) ^
                                           MultiplicacaoGalois(r2, 2) ^
                                           MultiplicacaoGalois(r3, 3) ^
                                           MultiplicacaoGalois(r4, 1));

            matrizResultado[2, i] = (byte)(MultiplicacaoGalois(r1, 1) ^
                                           MultiplicacaoGalois(r2, 1) ^
                                           MultiplicacaoGalois(r3, 2) ^
                                           MultiplicacaoGalois(r4, 3));

            matrizResultado[3, i] = (byte)(MultiplicacaoGalois(r1, 3) ^
                                           MultiplicacaoGalois(r2, 1) ^
                                           MultiplicacaoGalois(r3, 1) ^
                                           MultiplicacaoGalois(r4, 2));
        }

        return matrizResultado;
    }

    private byte MultiplicacaoGalois(byte operacaoMomento, byte constanteDaTabelaMultiplicacao)
    {
        if (operacaoMomento == 0 || constanteDaTabelaMultiplicacao == 0)
            return 0;

        if (operacaoMomento == 1)
            return constanteDaTabelaMultiplicacao;

        if (constanteDaTabelaMultiplicacao == 1)
            return operacaoMomento;

        var (bitMaisSignificativoOperacaoMomento, bitMenosSignificativoOperacaoMomento) = ObterBitMaisEMenosSignificativo(operacaoMomento);
        var resultadoTabelaLParaOperacaoMomento = Constantes.MatrizL[bitMaisSignificativoOperacaoMomento, bitMenosSignificativoOperacaoMomento];

        var (bitMaisSignificativoConstanteDaTabelaMultiplicacao, bitMenosSignificativoConstanteDaTabelaMultiplicacao) = ObterBitMaisEMenosSignificativo(constanteDaTabelaMultiplicacao);
        var resultadoTabelaLParaConstanteDaTabelaMultiplicacao = Constantes.MatrizL[bitMaisSignificativoConstanteDaTabelaMultiplicacao, bitMenosSignificativoConstanteDaTabelaMultiplicacao];

        var somaDosResultados = Convert.ToInt32(resultadoTabelaLParaOperacaoMomento) + Convert.ToInt32(resultadoTabelaLParaConstanteDaTabelaMultiplicacao);
        if (somaDosResultados > 0xFF)
        {
            somaDosResultados = somaDosResultados - 0xFF;
        }

        var somaDosResultadosBitMaisMenosSignificativo = ObterBitMaisEMenosSignificativo((byte)somaDosResultados);
        return Constantes.MatrizE[somaDosResultadosBitMaisMenosSignificativo.BitMaisSignificativo, somaDosResultadosBitMaisMenosSignificativo.BitMenosSignificativo];
    }

    private (int BitMaisSignificativo, int BitMenosSignificativo) ObterBitMaisEMenosSignificativo(byte byteParaObterBitMaisEMenosSignificativo)
    {
        Span<byte> spanComByte = new byte[] { byteParaObterBitMaisEMenosSignificativo };
        var byteHexa = Convert.ToHexString(spanComByte);

        var intQuatroBitsMaisSignificativos = int.Parse(byteHexa[0].ToString(), NumberStyles.HexNumber);
        var intQuatroBitsMenosSignificativos = int.Parse(byteHexa[1].ToString(), NumberStyles.HexNumber);

        return (intQuatroBitsMaisSignificativos, intQuatroBitsMenosSignificativos);
    }

    private byte[] ObterCifra(byte[,] matrizEstado)
    {
        var byteCifraSaida = new List<byte>(16);

        for (int i = 0; i < matrizEstado.GetLength(0); i++)
        {
            var dimensao = new byte[4];
            for (int j = 0; j < matrizEstado.GetLength(1); j++)
            {
                dimensao[j] = matrizEstado[j, i];
            }

            byteCifraSaida.AddRange(dimensao);
        }

        return byteCifraSaida.ToArray();
    }
}
