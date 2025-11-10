namespace inmobiliaria_api_mobile.DTOs;

public class EditarInmuebleDTO {
    public int Id { get; set; }
    public string Direccion { get; set; } = "";
    public Uso Uso { get; set; }
    public Tipo Tipo { get; set; }
    public int NoAmbientes { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public decimal Precio { get; set; }
    public Disponibilidad Disponibilidad { get; set; }
}
