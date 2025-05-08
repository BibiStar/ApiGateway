using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.Mappers
{
    public static class OrderMapper
    {
        //Model para DTO
        public static OrderDto ToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Produto = "DefaultProduto", // Valor padrão ou ajuste conforme necessário  
                Quantidade = 1, // Valor padrão ou ajuste conforme necessário  
                Total = (double)order.Total
            };
        }
        //DTO para Model
        public static Order ToModel(OrderDto dto)
        {
            return new Order
            {
                Id = dto.Id,
                Total = (decimal)dto.Total,
                CreatedAt = DateTime.UtcNow, // ou outro valor padrão  
                CustomerId = 0 // se precisar, pode vir do contexto ou ser passado separadamente  
            };
        }
    }

}
