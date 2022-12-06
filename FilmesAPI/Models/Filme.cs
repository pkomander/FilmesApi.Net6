using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "O Titulo do filme e obrigatorio")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "O Genero do filme e obrigatorio")]
    [MaxLength(50, ErrorMessage = "O tamanho do Genero nao pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required]
    [Range(70, 600, ErrorMessage = "A Duracao do filme e obrigatorio")]
    public int Duracao { get; set; }
}
