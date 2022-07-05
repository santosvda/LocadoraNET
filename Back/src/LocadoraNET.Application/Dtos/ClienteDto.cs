using System.ComponentModel.DataAnnotations;

namespace LocadoraNET.Application.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field {0} is required"),
        StringLength(200, MinimumLength = 3, ErrorMessage = "Field {0} length must be between 3 and 200")]        
        public string Nome { get; set; }
        [Required(ErrorMessage = "Field {0} is required"),
        Range(0, long.MaxValue, ErrorMessage = "Please enter valid int Number"),
        StringLength(11, MinimumLength = 11, ErrorMessage = "Field {0} must have 11 characters")]        
        public string Cpf { get; set; }
        [Required(ErrorMessage = "Field {0} is required")]
        public string DataNascimento { get; set; }
    }
}