namespace LocadoraNET.API.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string DataNascimento { get; set; }
    }
}