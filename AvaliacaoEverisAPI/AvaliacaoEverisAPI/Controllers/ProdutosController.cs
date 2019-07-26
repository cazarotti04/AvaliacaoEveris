﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AvaliacaoEverisAPI.Models;
using AvaliacaoEverisAPI.Services;

namespace AvaliacaoEverisAPI.Controllers
{
    public class ProdutosController : Controller
    {

        ProdutoService service = new ProdutoService();

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

        [HttpDelete]
        public ActionResult RemoverProduto(int cdgProduto)
        {
            try
            {
                service.RemoveProduto(cdgProduto);

                return Json("O produto foi deletado com sucesso");
            }
            catch (Exception)
            {
                throw new Exception("Erro ao deletar o produto");
            }
        }

        [HttpPut]
        public ActionResult EditaProduto(Produto produto)
        {
            try
            {
                var result = service.EditaProduto(produto);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao editar o produto");
            }
        }

        public ActionResult CadastrarProduto(Produto produto)
        {
            return View(produto);
        }
    }
}

