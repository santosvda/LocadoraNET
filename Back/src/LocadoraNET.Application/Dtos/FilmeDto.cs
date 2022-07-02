using System.ComponentModel.DataAnnotations;

namespace LocadoraNET.Application.Dtos
{
    public class FilmeDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field {0} is required"),
        StringLength(1, MinimumLength = 100, ErrorMessage = "Field {0} length must be between 1 and 100")]    
        public string Titulo { get; set; }
        [Range (1, 120, ErrorMessage = "{0} must be between 1 ans 120")]
        public int ClassificacaoIndicativa { get; set; }
        [Range (1800, 2050, ErrorMessage = "{0} must be between 1800 ans 2050")]
        public int Lancamento { get; set; }
    }
}