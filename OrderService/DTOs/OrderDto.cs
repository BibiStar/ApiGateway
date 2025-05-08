namespace OrderService.DTOs
{
    //Usado para transmitir dados entre sistemas, camadas ou APIs.
    public class OrderDto
    {
        public int Id { get; set; }
        public required string Produto { get; set; }
        public int Quantidade { get; set; }
        public double Total { get; set; }
    }

}
