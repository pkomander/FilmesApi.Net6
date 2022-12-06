using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class CreateFilmeDto
{
    [Required(ErrorMessage = "O Titulo do filme e obrigatorio")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "O Genero do filme e obrigatorio")]
    [StringLength(50, ErrorMessage = "O tamanho do Genero nao pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required]
    [Range(70, 600, ErrorMessage = "A Duracao do filme e obrigatorio")]
    public int Duracao { get; set; }
}
