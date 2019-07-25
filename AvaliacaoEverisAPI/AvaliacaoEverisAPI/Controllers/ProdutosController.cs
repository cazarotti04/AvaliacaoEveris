using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils.Models;
using Services;

namespace AvaliacaoEverisAPI.Controllers
{
    public class ProdutosController : Controller
    {
        // GET: Produtos
        //public ActionResult Index()
        //{
        //    return View();
        //}

        ProdutoService service = new ProdutoService();

        [HttpPost]
        public ActionResult InsereProduto(Produto produto)
        {
            try
            {

                var result = service.InsereProduto(produto);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao criar o produto");
            }
        }

        [HttpGet]
        public ActionResult ListaProdutos()
        {
            try
            {
                var result = service.ListaProdutos();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw new Exception("Erro ao listar os produtos");
            }
        }


    }
}

