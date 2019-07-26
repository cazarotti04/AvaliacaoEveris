using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using AvaliacaoEverisAPI.Services.Validators;
using AvaliacaoEverisAPI.Models;

namespace AvaliacaoEverisAPI.Services
{
    public class ProdutoService
    {
        string connectionString = ConnectionDb.connectionString;
        ProdutoValidator validator = new ProdutoValidator();

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

        public Produto EditaProduto(Produto produto)
        {
            using (SqlConnection conexao = new SqlConnection(connectionString))
            {
                bool valido = false;
                string update = "UPDATE Produtos SET Empresa = @EMPRESA, Produto = @PRODUTO, Entrada = @ENTRADA, Saida = @SAIDA, Estoque = @ESTOQUE WHERE cdgProduto = " + produto.CDGPRODUTO;

                valido = validator.ProdutoExistente((int)produto.CDGPRODUTO);

                if(!valido)
                    throw new Exception("Erro ao buscar o produto no banco");

                valido = validator.ValidaProdutoNovo(produto);

                if (!valido)
                    throw new Exception("Erro ao validar as informações do seu produto. Verifique todos os campos");

                try
                {
                    conexao.Execute(update, new { EMPRESA = produto.EMPRESA, PRODUTO = produto.NOMEPRODUTO, ENTRADA = produto.ENTRADA, SAIDA = produto.SAIDA, ESTOQUE = produto.ESTOQUE });

                    return produto;
                }
                catch (Exception)
                {
                    throw new Exception("Houve um erro ao editar o produto");
                }
            }
        }

    }
}