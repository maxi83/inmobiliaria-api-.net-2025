namespace inmobiliaria_api_mobile.DTOs;

public class EditarInmuebleDTO {
    public int Id { get; set; }
    public decimal Precio { get; set; }
    public string Direccion { get; set; } = null!;
}
