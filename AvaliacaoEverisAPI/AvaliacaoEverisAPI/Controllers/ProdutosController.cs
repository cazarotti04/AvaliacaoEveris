using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AvaliacaoEverisAPI.Models;
using AvaliacaoEverisAPI.Services;
using ClosedXML.Excel;

namespace AvaliacaoEverisAPI.Controllers
{
    public class ProdutosController : Controller
    {
        //Inicia a classe de serviços
        ProdutoService service = new ProdutoService();


        /**
        *   @brief Método para criar um novo produto e inseri-lo no banco de dados
        *
        *   @param produto Objeto enviado, por meio de uma requisição, que representa o novo produto cadastrado
        *
        *   @return Redirect para a view Index
        */
        [HttpPost]
        public ActionResult InsereProduto(Produto produto)
        {
            try
            {
                var result = service.InsereProduto(produto);

                //return Json(result, JsonRequestBehavior.AllowGet);
                return Redirect("/Home/Index");
            }
            catch (Exception)
            {
                throw new Exception("Erro ao criar o produto");
            }
        }


        /**
        *   @brief Método para listar os produtos existentes no banco de dados
        *
        *   @return List<Produto> A lista de objetos salvos no banco de dados
        */
        [HttpGet]
        public ActionResult ListaProdutos()
        {
            try
            {
                var result = service.ListaProdutos();

                //return Json(result, JsonRequestBehavior.AllowGet);

                return View(result);
            }
            catch (Exception)
            {

                throw new Exception("Erro ao listar os produtos");
            }
        }


        /**
        *   @brief Método para deletar produto no banco de dados
        *
        *   @param cdgProduto Numero inteiro que representa o código do produto a ser deletado
        *
        *   @return Redirect para a view Index
        */
        [HttpDelete]
        public ActionResult RemoverProduto(int cdgProduto)
        {
            try
            {
                service.RemoveProduto(cdgProduto);

                return Redirect("/Home/Index");
            }
            catch (Exception)
            {
                throw new Exception("Erro ao deletar o produto");
            }
        }


        /**
        *   @brief Método para editar um produto e salvar essas alterações no banco de dados
        *
        *   @param produto Objeto enviado, por meio de uma requisição, que representa o produto a ser editado
        *
        *   @return Redirect para a view Index
        */
        [HttpPost]
        public ActionResult EditaProduto(Produto produto)
        {
            try
            {
                var result = service.EditaProduto(produto, false);

                return Redirect("/Home/Index");
            }
            catch (Exception)
            {
                throw new Exception("Erro ao editar o produto");
            }
        }


        /**
        *   @brief Método para atualizar os produtos e sua informações atraves de um arquivo em excel
        *
        *   @param file Uma cadeia de caracteres em base64 que será posteriormente convertida em um arquivo excel
        *
        *   @return Redirect para a view Index
        */
        [HttpGet]
        public ActionResult AtualizaExcel(/*string file*/)
        {
            //var wb = new XLWorkbook(@"C:\dados.xlsx");

            service.AtualizaExcel(/*file*/);

            return Redirect("/Home/Index");
        }

        /**
        *   @brief Método usado para chamar a view de upload de arquivo
        *
        *   @return View /Produtos/UploadArquivo
        */
        public ActionResult UploadArquivo()
        {
            var file = new MyFile();
            return View(file);
        }


        /**
        *   @brief Método usado para chamar a view de cadastro de produto
        *
        *   @return View /Produtos/CadastrarProduto
        */
        public ActionResult CadastrarProduto()
        {
            return View();
        }


        /**
        *   @brief Método usado para chamar a view de editar o produto
        *
        *   @param produto Numero inteiro que representa o codigo do produto a ser editado
        *
        *   @return View /Produtos/EditProduto passando o objeto de produto com suas informaçoes, para serem editadas
        */
        public ActionResult EditProduto(int produto)
        {
            var objProduto = service.BuscaPorCodigo(produto);
            return PartialView(objProduto);
            //return View(objProduto);
        }

    }
}

