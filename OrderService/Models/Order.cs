using static System.Net.WebRequestMethods;
using System.ComponentModel;

namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }

        public int CustomerId { get; set; }
        /*seguindo boas práticas:
        Evite usar public Customer Customer { get; set; }
        diretamente no Order.
        Use apenas public int CustomerId { get; set; }
        no Order.
        Se precisar dos dados do cliente (nome, CPF etc.), chame a API do CustomerService via HTTP no momento necessário.
       // public Customer Customer { get; set; }*/
    }

}
