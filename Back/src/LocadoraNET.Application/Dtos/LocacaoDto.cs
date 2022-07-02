using System.ComponentModel.DataAnnotations;

namespace LocadoraNET.Application.Dtos
{
    public class LocacaoDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field {0} is required")]
        public string DataLocacao { get; set; }
        [Required(ErrorMessage = "Field {0} is required")]
        public string DataDevolucao { get; set; }
        [Required(ErrorMessage = "Field {0} is required")]
        public int ClienteId { get; set; }
        public ClienteDto Cliente { get; set; }
        [Required(ErrorMessage = "Field {0} is required")]
        public int FilmeId { get; set; }
        public FilmeDto Filme { get; set; }
    }
}