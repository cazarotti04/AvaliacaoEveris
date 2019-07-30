using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using AvaliacaoEverisAPI.Services.Validators;
using AvaliacaoEverisAPI.Models;
using ClosedXML.Excel;
using Dapper.Contrib;
using System.IO;

namespace AvaliacaoEverisAPI.Services
{
    public class ProdutoService
    {
        //@brief Busca a connection string definida nos paraetros do projeto
        string connectionString = ConnectionDb.connectionString;

        //@brief Instancia a classe de validacao do produto
        ProdutoValidator validator = new ProdutoValidator();

        /**
        *   @brief Método para validar as informações do produto e inserilo no banco de dados
        *
        *   @param produto Objeto enviado, via controlador, que representa o novo produto cadastrado
        *
        *   @return Instancia do novo produto cadastrado
        */
        public Produto InsereProduto(Produto produto)
        {
            bool erro = false;

            erro = validator.ValidaProdutoNovo(produto);

            if (!erro)
                throw new Exception("Erro ao validar produto. Verifique se todos os campos são válidos");

            using(SqlConnection conexao = new SqlConnection(connectionString))
            {
                conexao.Open();

                var produtoFinal = new Produto();

                var insere = "INSERT INTO Produtos (Empresa, nomeProduto, Entrada, Saida, Estoque) Values (@EMPRESA, @NOMEPRODUTO, @ENTRADA, @SAIDA, @ESTOQUE)";
                var consulta = "SELECT * FROM Produtos WHERE nomeProduto = @PRODUTO";

                try
                {
                    conexao.Execute(insere, new { EMPRESA = produto.EMPRESA, NOMEPRODUTO = produto.NOMEPRODUTO, ENTRADA = produto.ENTRADA, SAIDA = produto.SAIDA, ESTOQUE = produto.ESTOQUE });
                }
                catch (Exception)
                {
                    throw new Exception("Erro ao inserir produto na tabela");
                }

                try
                {
                    produtoFinal = conexao.Query<Produto>(consulta, new { produto = produto.NOMEPRODUTO }).LastOrDefault();
                }
                catch (Exception)
                {
                    throw new Exception("Erro ao ler produto na tabela");
                }

                conexao.Close();

                if(!produtoFinal.Equals(new Produto()))
                    return produtoFinal;

                throw new Exception("Erro ao salvar o produto");
            }
        }

        /**
        *   @brief Método para buscar um produto no banco de dados pelo seu código
        *
        *   @param produto Número inteiro que representa o código do produto
        *
        *   @return Instancia do produto especificado
        */
        public Produto BuscaPorCodigo(int codigo)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                var produto = new Produto();
                string consulta = "SELECT * FROM Produtos WHERE cdgProduto = " + codigo;
                try
                {
                    produto = conexao.Query<Produto>(consulta).FirstOrDefault();

                    return produto;
                }
                catch (Exception)
                {

                    throw new Exception("Erro ao buscar produto por código");
                }
            }
        }

        /**
        *   @brief Método para buscar a lista de produtos cadastrados no banco de dados
        *
        *   @return List<Produto> retorna a lista de produtos existentes
        */
        public List<Produto> ListaProdutos()
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                string consulta = "SELECT * FROM Produtos";
                var lista = new List<Produto>();

                conexao.Open();

                try
                {
                    lista = conexao.Query<Produto>(consulta).ToList();
                }
                catch (Exception)
                {
                    throw new Exception("Erro ao ler a lista de produtos");
                }

                conexao.Close();

                return lista;
            }
        }

        /**
        *   @brief Método para remover um produto do banco de dados
        *
        *   @param cdgProduto Número inteiro que representa o código do produto a ser deletado
        *
        */
        public void RemoveProduto(int cdgProduto)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                bool existe = false;
                string deleta = "DELETE FROM Produtos WHERE cdgProduto = " + cdgProduto;

                conexao.Open();

                existe = validator.ProdutoExistente(cdgProduto);

                if (!existe)
                    throw new Exception("Produto não encontrado");

                try
                {
                    conexao.Execute(deleta);
                    conexao.Close();
                }
                catch (Exception)
                {
                    throw new Exception("Houve um erro ao deletar o produto selecionado");
                }
            }
        }

        /**
        *   @brief Método para validar as edições de um produto e salva-las no banco de dados
        *
        *   @param produto Objeto enviado, por meio do controlador, que representa produto a ser editado
        *   @param excel Valor booleano para especificar se a edição está sendo feita por um arquivo em excel ou não
        *
        *   @return Produto Objeto com suas devidas alterações
        */
        public Produto EditaProduto(Produto produto, bool excel = false)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                bool valido = false;
                string update = "UPDATE Produtos SET Empresa = @EMPRESA, nomeProduto = @NOMEPRODUTO, Entrada = @ENTRADA, Saida = @SAIDA, Estoque = @ESTOQUE WHERE cdgProduto = " + produto.CDGPRODUTO;

                valido = validator.ProdutoExistente((int)produto.CDGPRODUTO);

                if(!valido)
                    throw new Exception("Erro ao buscar o produto no banco");

                valido = validator.ValidaProdutoNovo(produto);

                if (!valido)
                    throw new Exception("Erro ao validar as informações do seu produto. Verifique todos os campos");

                try
                {
                    string consulta = "SELECT * FROM Produtos WHERE cdgProduto = " + produto.CDGPRODUTO ;

                    var existente = conexao.Query<Produto>(consulta).FirstOrDefault();

                    if (excel)
                    {
                        produto.ESTOQUE += existente.ESTOQUE;
                        produto.ENTRADA += existente.ENTRADA;
                        produto.SAIDA += existente.SAIDA;
                    }

                    conexao.Execute(update, new { EMPRESA = produto.EMPRESA, NOMEPRODUTO = produto.NOMEPRODUTO, ENTRADA = produto.ENTRADA, SAIDA = produto.SAIDA, ESTOQUE = produto.ESTOQUE });

                    return produto;
                }
                catch (Exception)
                {
                    throw new Exception("Houve um erro ao editar o produto");
                }
            }
        }

        /**
        *   @brief Método para realizar a leitura do arquivo em excel e estruturar seus dados afim de fazer inserções e alterações no banco de dados
        *
        *   @param arqBase64 Cadeia de caracteres que representa o arquivo em excel convertido para base64
        *
        */
        public void AtualizaExcel(/*string arqBase64*/)
        {
            //var bytes = Convert.FromBase64String(arqBase64);
            //var ms = new MemoryStream(bytes);
            //var wb = new XLWorkbook(ms);

            var wb = new XLWorkbook(@"C:/Caminho_do_Arquivo");

            var planilha = wb.Worksheet(1);

            var lista = new List<Produto>();

            var novoProduto = new Produto();

            int linha = 5;

            while(true)
            {
                novoProduto = new Produto();

                var teste = planilha.Cell("A" + linha.ToString()).Value.ToString();

                if (string.IsNullOrEmpty(teste))
                    break;

                novoProduto.EMPRESA = planilha.Cell("A" + linha.ToString()).Value.ToString();
                var cdg = planilha.Cell("B" + linha.ToString()).Value.ToString();
                novoProduto.CDGPRODUTO = Int32.Parse(cdg);
                novoProduto.NOMEPRODUTO = planilha.Cell("C" + linha.ToString()).Value.ToString();
                var entrada = planilha.Cell("D" + linha.ToString()).Value.ToString();
                var saida = planilha.Cell("E" + linha.ToString()).Value.ToString();
                var estoque = planilha.Cell("F" + linha.ToString()).Value.ToString();
                if (entrada != null && entrada != "")
                    novoProduto.ENTRADA = Int32.Parse(entrada);
                else
                    novoProduto.ENTRADA = 0;

                if (saida != null && saida != "")
                    novoProduto.SAIDA = Int32.Parse(saida);
                else
                    novoProduto.SAIDA = 0;

                if (estoque != null && estoque != "")
                    novoProduto.ESTOQUE = Int32.Parse(estoque);
                else
                    novoProduto.ESTOQUE = 0;

                lista.Add(novoProduto);

                linha++;

            }

            foreach (var item in lista)
            {
                try
                {
                    var x = validator.ProdutoExistente((int)item.CDGPRODUTO);

                    if (!x)
                        InsereProduto(item);
                    else
                        EditaProduto(item, true);
                }
                catch (Exception)
                {
                    throw new Exception("Erro ao atualizar estoque");
                }
            }
        }

    }
}