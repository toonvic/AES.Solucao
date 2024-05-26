﻿using System.Text;

namespace AES.Console;

public class AlgoritmoAes
{
    public static byte[,] MatrizSBox = new byte[16, 16]
    {
    {0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76},
    {0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0},
    {0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15},
    {0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75},
    {0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84},
    {0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf},
    {0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8},
    {0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2},
    {0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73},
    {0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb},
    {0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79},
    {0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08},
    {0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a},
    {0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e},
    {0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf},
    {0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16},
    };

    public static byte[,] MatrizL = new byte[,]
    {

     {0x00, 0x00, 0x19, 0x01, 0x32, 0x02, 0x1a, 0xC6, 0x4B, 0xC7, 0x1B, 0x68, 0x33, 0xEE, 0xDF, 0x03},
     {0x64, 0x04, 0xE0, 0x0E, 0x34, 0x8D, 0x81, 0xEF, 0x4C, 0x71, 0x08, 0xC8, 0xF8, 0x69, 0x1C, 0xC1},
     {0x7d, 0xC2, 0x1D, 0xB5, 0xF9, 0xB9, 0x27, 0x6A, 0x4D, 0xE4, 0xA6, 0x72, 0x9A, 0xC9, 0x09, 0x78},
     {0x65, 0x2F, 0x8A, 0x05, 0x21, 0x0F, 0xE1, 0x24, 0x12, 0xF0, 0x82, 0x45, 0x35, 0x93, 0xDA, 0x8E},
     {0x96, 0x8F, 0xDB, 0xBD, 0x36, 0xD0, 0xCE, 0x94, 0x13, 0x5C, 0xD2, 0xF1, 0x40, 0x46, 0x83, 0x38},
     {0x66, 0xDD, 0xFD, 0x30, 0xBF, 0x06, 0x8B, 0x62, 0xB3, 0x25, 0xE2, 0x98, 0x22, 0x88, 0x91, 0x10},
     {0x7E, 0x6E, 0x48, 0xC3, 0xA3, 0xB6, 0x1E, 0x42, 0x3A, 0x6B, 0x28, 0x54, 0xFA, 0x85, 0x3D, 0xBA},
     {0x2B, 0x79, 0x0A, 0x15, 0x9B, 0x9F, 0x5E, 0xCA, 0x4E, 0xD4, 0xAC, 0xE5, 0xF3, 0x73, 0xA7, 0x57},
     {0xAF, 0x58, 0xA8, 0x50, 0xF4, 0xEA, 0xD6, 0x74, 0x4F, 0xAE, 0xE9, 0xD5, 0xE7, 0xE6, 0xAD, 0xE8},
     {0x2C, 0xD7, 0x75, 0x7A, 0xEB, 0x16, 0x0B, 0xF5, 0x59, 0xCB, 0x5F, 0xB0, 0x9C, 0xA9, 0x51, 0xA0},
     {0x7F, 0x0C, 0xF6, 0x6F, 0x17, 0xC4, 0x49, 0xEC, 0xD8, 0x43, 0x1F, 0x2D, 0xA4, 0x76, 0x7B, 0xB7},
     {0xCC, 0xBB, 0x3E, 0x5A, 0xFB, 0x60, 0xB1, 0x86, 0x3B, 0x52, 0xA1, 0x6C, 0xAA, 0x55, 0x29, 0x9D},
     {0x97, 0xB2, 0x87, 0x90, 0x61, 0xBE, 0xDC, 0xFC, 0xBC, 0x95, 0xCF, 0xCD, 0x37, 0x3F, 0x5B, 0xD1},
     {0x53, 0x39, 0x84, 0x3C, 0x41, 0xA2, 0x6D, 0x47, 0x14, 0x2A, 0x9E, 0x5D, 0x56, 0xF2, 0xD3, 0xAB},
     {0x44, 0x11, 0x92, 0xD9, 0x23, 0x20, 0x2E, 0x89, 0xB4, 0x7C, 0xB8, 0x26, 0x77, 0x99, 0xE3, 0xA5},
     {0x67, 0x4A, 0xED, 0xDE, 0xC5, 0x31, 0xFE, 0x18, 0x0D, 0x63, 0x8C, 0x80, 0xC0, 0xF7, 0x70, 0x07},
    };

    public static byte[,] MatrizE = new byte[,]
    {
    { 0x01, 0x03, 0x05, 0x0F, 0x11, 0x33, 0x55, 0xFF, 0x1A, 0x2E, 0x72, 0x96, 0xA1, 0xF8, 0x13, 0x35},
    { 0x5F, 0xE1, 0x38, 0x48, 0xD8, 0x73, 0x95, 0xA4, 0xF7, 0x02, 0x06, 0x0A, 0x1E, 0x22, 0x66, 0xAA},
    { 0xE5, 0x34, 0x5C, 0xE4, 0x37, 0x59, 0xEB, 0x26, 0x6A, 0xBE, 0xD9, 0x70, 0x90, 0xAB, 0xE6, 0x31},
    { 0x53, 0xF5, 0x04, 0x0C, 0x14, 0x3C, 0x44, 0xCC, 0x4F, 0xD1, 0x68, 0xB8, 0xD3, 0x6E, 0xB2, 0xCD},
    { 0x4C, 0xD4, 0x67, 0xA9, 0xE0, 0x3B, 0x4D, 0xD7, 0x62, 0xA6, 0xF1, 0x08, 0x18, 0x28, 0x78, 0x88},
    { 0x83, 0x9E, 0xB9, 0xD0, 0x6B, 0xBD, 0xDC, 0x7F, 0x81, 0x98, 0xB3, 0xCE, 0x49, 0xDB, 0x76, 0x9A},
    { 0xB5, 0xC4, 0x57, 0xF9, 0x10, 0x30, 0x50, 0xF0, 0x0B, 0x1D, 0x27, 0x69, 0xBB, 0xD6, 0x61, 0xA3},
    { 0xFE, 0x19, 0x2B, 0x7D, 0x87, 0x92, 0xAD, 0xEC, 0x2F, 0x71, 0x93, 0xAE, 0xE9, 0x20, 0x60, 0xA0},
    { 0xFB, 0x16, 0x3A, 0x4E, 0xD2, 0x6D, 0xB7, 0xC2, 0x5D, 0xE7, 0x32, 0x56, 0xFA, 0x15, 0x3F, 0x41},
    { 0xC3, 0x5E, 0xE2, 0x3D, 0x47, 0xC9, 0x40, 0xC0, 0x5B, 0xED, 0x2C, 0x74, 0x9C, 0xBF, 0xDA, 0x75},
    { 0x9F, 0xBA, 0xD5, 0x64, 0xAC, 0xEF, 0x2A, 0x7E, 0x82, 0x9D, 0xBC, 0xDF, 0x7A, 0x8E, 0x89, 0x80},
    { 0x9B, 0xB6, 0xC1, 0x58, 0xE8, 0x23, 0x65, 0xAF, 0xEA, 0x25, 0x6F, 0xB1, 0xC8, 0x43, 0xC5, 0x54},
    { 0xFC, 0x1F, 0x21, 0x63, 0xA5, 0xF4, 0x07, 0x09, 0x1B, 0x2D, 0x77, 0x99, 0xB0, 0xCB, 0x46, 0xCA},
    { 0x45, 0xCF, 0x4A, 0xDE, 0x79, 0x8B, 0x86, 0x91, 0xA8, 0xE3, 0x3E, 0x42, 0xC6, 0x51, 0xF3, 0x0E},
    { 0x12, 0x36, 0x5A, 0xEE, 0x29, 0x7B, 0x8D, 0x8C, 0x8F, 0x8A, 0x85, 0x94, 0xA7, 0xF2, 0x0D, 0x17},
    { 0x39, 0x4B, 0xDD, 0x7C, 0x84, 0x97, 0xA2, 0xFD, 0x1C, 0x24, 0x6C, 0xB4, 0xC7, 0x52, 0xF6, 0x01},

    };

    public static byte[] RoundConstant = new byte[10]
    {
    0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1B, 0x36
    };

    private static StringBuilder _stringBuilder = new StringBuilder();

    public string ObterLog() =>
        _stringBuilder.ToString();

    public void AdicionarChave(string chave)
    {
        _stringBuilder.AppendLine("**** Chave ****");

        int colunas = 4;
        int contador = 0;

        foreach (char caractere in chave)
        {
            _stringBuilder.Append($"0x{Convert.ToByte(caractere):X2} ");

            contador++;
            if (contador == colunas)
            {
                _stringBuilder.AppendLine();
                contador = 0;
            }
        }
    }

    private void AdicionarLinhaTextoSimples(string texto)
    {
        _stringBuilder.AppendLine("**** Texto simples ****");
        byte[] bytes = Encoding.ASCII.GetBytes(texto);
        int count = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            _stringBuilder.Append($"0x{bytes[i]:X2} ");
            count++;
            if (count % 4 == 0)
            {
                _stringBuilder.AppendLine();
                count = 0;
            }
        }
    }

    private void AdicionarResultado(string nomeRound, byte[,] matrizEstado)
    {
        _stringBuilder.AppendLine($"**** {nomeRound} ****");
        for (int i = 0; i < matrizEstado.GetLength(0); i++)
        {
            for (int j = 0; j < matrizEstado.GetLength(1); j++)
            {
                if (j == 0)
                {
                    _stringBuilder.Append("0x");
                }
                _stringBuilder.Append($"{matrizEstado[j, i]:X2} ");
            }
            _stringBuilder.AppendLine();
        }
        _stringBuilder.AppendLine();
    }

    public string Criptografar(string entrada, string chave, string nomeArquivo)
    {
        var composicao = GerarComposicaoChave(chave);

        var matrizDeEstado = GerarMatrizDeEstado(composicao);

        var palavras = ObterPalavras(matrizDeEstado);

        var keySchedule = GerarKeySchedule(palavras);

        AdicionarLinhaTextoSimples(entrada);

        var blocos = PrepararBlocos(
             Encoding.ASCII.GetBytes(entrada)
        );

        return ExecutarCriptografia(blocos, keySchedule, nomeArquivo);
    }

    private static byte[] GerarComposicaoChave(string chave)
    {
        _stringBuilder.AppendLine("**** Chave ****");

        if (string.IsNullOrWhiteSpace(chave))
            throw new ArgumentException("A chave não pode ser nula ou vazia.");

        var partes = chave.Split(',');

        if (partes.Length != 16)
            throw new ArgumentException("Chave com tamanho inválido.");

        var composicao = new byte[16];

        for (int i = 0; i < partes.Length; i++)
        {
            if (!byte.TryParse(partes[i], out composicao[i]))
                throw new ArgumentException($"Valor inválido na posição {i}: '{partes[i]}'");
        }

        for (int i = 0; i < composicao.Length; i++)
        {
            _stringBuilder.Append($"0x{composicao[i]:X2} ");
            if ((i + 1) % 4 == 0 && i != 0) // Adiciona quebra de linha a cada 4 valores
                _stringBuilder.AppendLine();
        }

        _stringBuilder.AppendLine();

        return composicao;
    }


    private static byte[,] GerarMatrizDeEstado(byte[] composicao)
    {
        if (composicao == null || composicao.Length != 16)
            throw new ArgumentException("Composição deve conter exatamente 16 bytes.", nameof(composicao));

        var matrizDeEstado = new byte[4, 4];

        for (int i = 0; i < 16; i++)
        {
            matrizDeEstado[i % 4, i / 4] = composicao[i];
        }

        return matrizDeEstado;
    }

    public List<byte[]> ObterPalavras(byte[,] matrizDeEstado) =>
        Enumerable
        .Range(0, 4)
        .Select(j => Enumerable.Range(0, 4)
            .Select(i => matrizDeEstado[i, j])
            .ToArray()
        )
        .ToList();

    private IList<List<byte[]>> GerarKeySchedule(List<byte[]> expansaoChave)
    {
        var keySchedule = new List<List<byte[]>>(11) { expansaoChave };
        
        AdicionarChaveExpandida(expansaoChave);

        for (int i = 1; i < 11; i++)
        {
            _stringBuilder.AppendLine($"**** RoundKey={i} ****");

            var roundKeyAnterior = keySchedule[i - 1];
            var roundKeyNova = new List<byte[]>(4);

            var primeiraPalavraRoundKey = GerarPrimeiraPalavraRoundKey(roundKeyAnterior[0], roundKeyAnterior[3], i);
            roundKeyNova.Add(primeiraPalavraRoundKey);

            for (int j = 1; j < 4; j++)
            {
                var novaPalavra = ExecutarXorBytes(roundKeyAnterior[j], roundKeyNova[j - 1]);
                roundKeyNova.Add(novaPalavra);
            }

            keySchedule.Add(roundKeyNova);
        }

        return keySchedule;
    }

    private void AdicionarChaveExpandida(List<byte[]> expansaoChave)
    {
        _stringBuilder.Append("**** RoundKey=0 ****\n");

        foreach (var palavra in expansaoChave)
        {
            AdicionarLinhaChave(palavra);
        }

        _stringBuilder.AppendLine();
    }

    private void AdicionarLinhaChave(byte[] palavra)
    {
        for (int i = 0; i < palavra.Length; i++)
        {
            _stringBuilder.Append($"0x{palavra[i]:X2} ");
            if ((i + 1) % 4 == 0) // Adiciona quebra de linha a cada 4 valores
                _stringBuilder.AppendLine();
        }
    }

    private byte[] GerarPrimeiraPalavraRoundKey(byte[] primeiraPalavra, byte[] ultimaPalavra, int indexRoundKey)
    {
        _stringBuilder.AppendLine("   Etapas para geração da primeira word\n");
        _stringBuilder.AppendLine("   1) Cópia da última palavra da roundkey anterior: " + ByteArrayToString(ultimaPalavra));

        var rotWord = GerarRotWord(ultimaPalavra);

        var subWord = GerarSubWord(rotWord);

        var roundConstant = GerarRoundConstant(indexRoundKey);

        var resultadoXor = ExecutarXorBytes(subWord, roundConstant);

        return ExecutarXorBytes(primeiraPalavra, resultadoXor);
    }

    private byte[] GerarRotWord(byte[] palavra)
    {
        _stringBuilder.AppendLine("   2) Rotacionar os bytes desta palavra (RotWord): " + ByteArrayToString(palavra));

        var resultado = new byte[] { palavra[1], palavra[2], palavra[3], palavra[0] };

        _stringBuilder.AppendLine("      Resultado: " + ByteArrayToString(resultado));
        return resultado;
    }

    private byte[] GerarSubWord(byte[] rotWord)
    {
        _stringBuilder.AppendLine("   3) Substituir os bytes da palavra (SubWord): " + ByteArrayToString(rotWord));

        var resultado = new byte[rotWord.Length];

        for (int i = 0; i < rotWord.Length; i++)
        {
            byte valor = rotWord[i];
            byte linha = (byte)(valor >> 4);
            byte coluna = (byte)(valor & 0x0F);
            resultado[i] = MatrizSBox[linha, coluna];
        }

        _stringBuilder.AppendLine("      Resultado: " + ByteArrayToString(resultado));
        return resultado;
    }

    private byte[] GerarRoundConstant(int indexRoundKey)
    {
        _stringBuilder.AppendLine("   4) Gerar a RoundConstant: " + $"[0x{RoundConstant[indexRoundKey - 1]:X2} 0x00 0x00 0x00]");

        var resultado = new byte[] { RoundConstant[indexRoundKey - 1], 0, 0, 0 };

        _stringBuilder.AppendLine("      Resultado: " + ByteArrayToString(resultado));
        return resultado;
    }

    private byte[] ExecutarXorBytes(byte[] primeiroValor, byte[] segundoValor)
    {
        _stringBuilder.AppendLine("   5) XOR de (3) com (4): " + ByteArrayToString(primeiroValor) + " ^ " + ByteArrayToString(segundoValor));

        var resultado = new byte[primeiroValor.Length];

        for (int i = 0; i < primeiroValor.Length; i++)
        {
            resultado[i] = (byte)(primeiroValor[i] ^ segundoValor[i]);
        }

        _stringBuilder.AppendLine("      Resultado: " + ByteArrayToString(resultado) + "\n");
        return resultado;
    }

    public IList<byte[,]> PrepararBlocos(byte[] entrada)
    {
        int quantidadeBlocos = (entrada.Length + 15) / 16;
        var blocos = new List<byte[,]>(quantidadeBlocos);

        for (int i = 0; i < quantidadeBlocos; i++)
        {
            var bloco = new byte[4, 4];
            var blocoInicio = i * 16;

            for (int j = 0; j < 16; j++)
            {
                var index = blocoInicio + j;
                if (index < entrada.Length)
                {
                    bloco[j % 4, j / 4] = entrada[index];

                    continue;
                }

                bloco[j % 4, j / 4] = (byte)(16 - (entrada.Length % 16));
            }

            blocos.Add(bloco);
        }

        if (entrada.Length % 16 == 0)
        {
            blocos.Add(
                new byte[,]
                {
                { 16, 16, 16, 16 },
                { 16, 16, 16, 16 },
                { 16, 16, 16, 16 },
                { 16, 16, 16, 16 }
                }
            );
        }

        return blocos;
    }


    private string ExecutarCriptografia(IList<byte[,]> blocos, IList<List<byte[]>> keySchedule, string nomeArquivo)
    {
        var cifra = new List<byte>();

        foreach ((var bloco, var index) in blocos.Select((value, index) => (value, index)))
        {
            _stringBuilder.AppendLine($"**** AddRoundKey-Round 0 ****");
            _stringBuilder.AppendLine(ConverterMatrizParaString(bloco));

            var matrizResultado = ExecutarXorBytes(bloco, keySchedule[0]);

            for (int i = 1; i < 10; i++)
            {
                _stringBuilder.AppendLine($"\n**** SubBytes-Round {i} ****");
                _stringBuilder.AppendLine(ConverterMatrizParaString(matrizResultado));
                matrizResultado = GerarSubBytes(matrizResultado);

                _stringBuilder.AppendLine($"\n**** ShiftRows-Round {i} ****");
                _stringBuilder.AppendLine(ConverterMatrizParaString(matrizResultado));
                matrizResultado = ShiftRows(matrizResultado);

                _stringBuilder.AppendLine($"\n**** MixedColumns-Round {i} ****");
                _stringBuilder.AppendLine(ConverterMatrizParaString(matrizResultado));
                matrizResultado = MixColumns(matrizResultado);

                _stringBuilder.AppendLine($"\n**** addRoundKey-Round {i} ****");
                _stringBuilder.AppendLine(ConverterMatrizParaString(matrizResultado));
                matrizResultado = ExecutarXorBytes(matrizResultado, keySchedule[i]);
            }

            _stringBuilder.AppendLine($"\n**** SubBytes-Round 10 ****");
            _stringBuilder.AppendLine(ConverterMatrizParaString(matrizResultado));
            matrizResultado = GerarSubBytes(matrizResultado);

            _stringBuilder.AppendLine($"\n**** ShiftRows-Round 10 ****");
            _stringBuilder.AppendLine(ConverterMatrizParaString(matrizResultado));
            matrizResultado = ShiftRows(matrizResultado);

            _stringBuilder.AppendLine($"\n**** addRoundKey-Round 10 ****");
            _stringBuilder.AppendLine(ConverterMatrizParaString(matrizResultado));
            matrizResultado = ExecutarXorBytes(matrizResultado, keySchedule[10]);

            _stringBuilder.AppendLine($"\n**** Texto cifrado ****");
            _stringBuilder.AppendLine(ConverterMatrizParaString(matrizResultado));

            cifra.AddRange(ObterCifra(matrizResultado));
        }

        File.WriteAllBytes(nomeArquivo, cifra.ToArray());

        return Convert.ToBase64String(cifra.ToArray());
    }

    private string ConverterMatrizParaString(byte[,] matriz)
    {
        var stringBuilder = new StringBuilder();

        for (int i = 0; i < matriz.GetLength(0); i++)
        {
            for (int j = 0; j < matriz.GetLength(1); j++)
            {
                stringBuilder.Append($"0x{matriz[i, j]:X2} ");
            }
            stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine();

        return stringBuilder.ToString();
    }


    private byte[,] ExecutarXorBytes(byte[,] matrizEstado, IList<byte[]> roundKeyAtual)
    {
        var matrizEstadoResultado = new byte[4, 4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                byte byteDoMomento = (byte)(matrizEstado[j, i] ^ roundKeyAtual[i][j]);
                matrizEstadoResultado[j, i] = byteDoMomento;
            }
        }

        return matrizEstadoResultado;
    }

    private byte[,] GerarSubBytes(byte[,] matrizEstado)
    {
        byte[,] matrizEstadoResultante = new byte[4, 4];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                byte valor = matrizEstado[i, j];
                byte byteSubstituido = MatrizSBox[BitMaisSignificativo(valor), BitMenosSignificativo(valor)];
                matrizEstadoResultante[i, j] = byteSubstituido;
            }
        }

        return matrizEstadoResultante;
    }

    private byte[,] ShiftRows(byte[,] matrizEstado)
    {
        byte[,] matrizEstadoResultado = new byte[4, 4];

        for (int j = 0; j < 4; j++)
        {
            matrizEstadoResultado[0, j] = matrizEstado[0, j];
        }

        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                matrizEstadoResultado[i, j] = matrizEstado[i, (j + i) % 4];
            }
        }

        return matrizEstadoResultado;
    }

    private byte[,] MixColumns(byte[,] matrizEstado)
    {
        byte[,] matrizResultado = new byte[4, 4];

        // Matriz de multiplicação para MixColumns
        byte[,] multiplicacao = {
            { 2, 3, 1, 1 },
            { 1, 2, 3, 1 },
            { 1, 1, 2, 3 },
            { 3, 1, 1, 2 }
        };

        for (int coluna = 0; coluna < 4; coluna++)
        {
            // Extrair os bytes da coluna atual
            byte[] colunaAtual = new byte[4];
            for (int linha = 0; linha < 4; linha++)
            {
                colunaAtual[linha] = matrizEstado[linha, coluna];
            }

            // Aplicar a operação MixColumns à coluna atual
            for (int linha = 0; linha < 4; linha++)
            {
                byte resultado = 0;
                for (int i = 0; i < 4; i++)
                {
                    resultado ^= MultiplicacaoGalois(colunaAtual[i], multiplicacao[linha, i]);
                }
                matrizResultado[linha, coluna] = resultado;
            }
        }

        return matrizResultado;
    }


    private byte MultiplicacaoGalois(byte operacao, byte constanteTabela)
    {
        if (operacao == 0 || constanteTabela == 0)
            return 0;

        if (operacao == 1)
            return constanteTabela;

        if (constanteTabela == 1)
            return operacao;

        var resultadoOperacao = MatrizL[BitMaisSignificativo(operacao), BitMenosSignificativo(operacao)];

        var resultadoTabela = MatrizL[BitMaisSignificativo(constanteTabela), BitMenosSignificativo(constanteTabela)];

        var somaDosResultados = resultadoOperacao + resultadoTabela;

        if (somaDosResultados > 0xFF)
        {
            somaDosResultados -= 0xFF;
        }

        return MatrizE[BitMaisSignificativo((byte)somaDosResultados), BitMenosSignificativo((byte)somaDosResultados)];
    }

    private static int BitMaisSignificativo(byte valor) =>
        (valor >> 4) & 0x0F;

    private static int BitMenosSignificativo(byte valor) =>
        valor & 0x0F;

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

    private string ByteArrayToString(byte[] array)
    {
        var result = new StringBuilder();
        result.Append("[");
        for (int i = 0; i < array.Length; i++)
        {
            result.Append($"0x{array[i]:X2}");
            if (i < array.Length - 1)
                result.Append(" ");
        }
        result.Append("]");
        return result.ToString();
    }
}
