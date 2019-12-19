using System;
using McBonaldsMVC.Enums;

namespace McBonaldsMVC.Models
{
    public class Pedido
    {
        public ulong Id {get;set;}
        public Cliente Cliente {get;set;}
        public Hamburguer Hamburguer {get;set;}

        public Shake Shake {get;set;}

        public DateTime DataDoPedido {get;set;}

        public double PrecoTotal {get;set;}

        public uint Status {get;set;}

        public Pedido()
        {
            this.Cliente = new Cliente();
            this.Hamburguer = new Hamburguer();
            this.Shake = new Shake();
            //O Id começa com zero
            this.Id = 0;
            // 0 pode signifcar pendente
            //tROCAR O ZERO PELO STATUS E METODO
            //ANTES
            // this.Status = 0;
            //DEPOIS
            //uint - guardar numero dentro de status, qualquer numero que esteja em pendente
            this.Status = (uint) StatusPedido.PENDENTE; 
        }
    }
}