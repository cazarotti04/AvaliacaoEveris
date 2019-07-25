using System;
using System.Collections.Generic;
using System.Text;
using Utils.Models;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using Services.Validators;

namespace Services
{
    public class ProdutoService
    {
        public Produto InsereProduto(Produto produto)
        {
            bool erro = false;
            var validator = new ProdutoValidator();


            erro = validator.ValidaProdutoNovo(produto);

            if (!erro)
                return null;

            var connectionString = ConnectionDb.connectionString;
            
            using(SqlConnection conexao = new SqlConnection(connectionString))
            {
                conexao.Open();

                var insere = "INSERT INTO Produtos (Empresa, Produto, Entrada, Saida, Estoque) Values (@EMPRESA, @PRODUTO, @ENTRADA, @SAIDA, @ESTOQUE)";

                var teste = conexao.Execute(insere, new { EMPRESA = produto.EMPRESA, PRODUTO = produto.PRODUTO, ENTRADA = produto.ENTRADA, SAIDA = produto.SAIDA, ESTOQUE = produto.ESTOQUE});

                return produto;

            }

            return produto;

        }
    }
}