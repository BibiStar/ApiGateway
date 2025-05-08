namespace OrderService.DTOs
{
    //Usado para transmitir dados entre sistemas, camadas ou APIs.
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
    }

}
