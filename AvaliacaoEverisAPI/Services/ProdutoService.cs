﻿using System;
using System.Collections.Generic;
using System.Text;
using Utils.Models;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using Services.Validators;
using Dapper.Extensions;
using System.Linq;

namespace Services
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

                var insere = "INSERT INTO Produtos (Empresa, Produto, Entrada, Saida, Estoque) Values (@EMPRESA, @PRODUTO, @ENTRADA, @SAIDA, @ESTOQUE)";
                var consulta = "SELECT * FROM Produtos WHERE produto = @PRODUTO";

                try
                {
                    conexao.Execute(insere, new { EMPRESA = produto.EMPRESA, PRODUTO = produto.PRODUTO, ENTRADA = produto.ENTRADA, SAIDA = produto.SAIDA, ESTOQUE = produto.ESTOQUE });
                }
                catch (Exception)
                {
                    throw new Exception("Erro ao inserir produto na tabela");
                }

                try
                {
                    produtoFinal = conexao.Query<Produto>(consulta, new { produto = produto.PRODUTO }).LastOrDefault();
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

                existe = validator.ProdutoExistente(cdgProduto);

                if (!existe)
                    throw new Exception("Produto não encontrado");

                try
                {
                    conexao.Execute(deleta);
                }
                catch (Exception)
                {
                    throw new Exception("Houve um erro ao deletar o produto selecionado");
                }
            }
        }



    }
}