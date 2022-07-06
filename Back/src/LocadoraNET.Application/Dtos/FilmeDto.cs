using System.ComponentModel.DataAnnotations;

namespace LocadoraNET.Application.Dtos
{
    public class FilmeDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field {0} is required"),
        StringLength(100, MinimumLength = 1, ErrorMessage = "Field {0} length must be between 1 and 100")]    
        public string Titulo { get; set; }
        [Range (1, 120, ErrorMessage = "{0} must be between 1 ans 120")]
        public int ClassificacaoIndicativa { get; set; }
        [Range (0, 1, ErrorMessage = "{0} must be 0 or 1")]
        public int Lancamento { get; set; }
    }
}