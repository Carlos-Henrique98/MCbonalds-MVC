using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using McBonaldsMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace McBonaldsMVC.Controllers
{
    public class HomeController : AbstractController
    {
        public IActionResult Index()
        {
            //Apagou o nomeView = Home
            // ViewData["NomeView"] = "Home";
            //Passar o a pasta BaseViewModel
            return View(new BaseViewModel()
            {
                NomeView = "Home",
                UsuarioEmail = ObterUsuarioSession(),
                UsuarioNome = ObterUsuarioNomeSession()
                
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
