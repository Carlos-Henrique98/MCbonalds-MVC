using System.Collections.Generic;
using McBonaldsMVC.Models;
//Função da tela
//Usuario LOGADO E pedido anonimo
namespace McBonaldsMVC.ViewModels
{
    public class PedidoViewModel : BaseViewModel
    {
        public List<Hamburguer> Hamburgueres {get;set;}
        public List<Shake> Shakes {get;set;}
        public string NomeUsuario {get;set;}
        public Cliente Cliente {get;set;}

        //Cliente Vazio garantido pelo construtor
        public PedidoViewModel()
        {
            this.Hamburgueres = new List<Hamburguer>();
            this.Shakes = new List<Shake>();
            //Cliente Vazio garantido pelo construtor
            this.NomeUsuario = "Jovem";
            this.Cliente = new Cliente();
        }

    }
}