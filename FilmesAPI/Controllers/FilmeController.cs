using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
    }

    /// <summary>
    /// Recupera uma lista de Filmes
    /// </summary>
    /// <param name="skip">Pagina lista de filmes</param>
    /// <param name="take">Quantidade de itens da lista de filmes</param>
    /// <returns>IEnumerable</returns>
    /// <response code="200">Caso Consulta seja feita com sucesso</response>
    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        //aplicando paginacao
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Recupera Filme
    /// </summary>
    /// <param name="id">Recebe o Id do filme que ira ser recuperado</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso Consulta seja feita com sucesso</response>
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(x => x.Id == id);
        if(filme == null)
        {
            return NotFound();
        }

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualiza Filme
    /// </summary>
    /// <param name="id">Recebe o Id do filme que ira ser Atualizado</param>
    /// <param name="filmeDto">Corpo do filme que ira ser Atualizado</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso Atualizacao seja feita com sucesso</response>
    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.Where(x => x.Id == id).FirstOrDefault();
        
        if(filme == null)
        {
            return NotFound();
        }
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza Filme
    /// </summary>
    /// <param name="id">Recebe o Id do filme que ira ser Atualizado</param>
    /// <param name="patch">Atributo do Filme que ira ser atualizado com Patch</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso Atualizacao seja feita com sucesso</response>
    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument <UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.Where(x => x.Id == id).FirstOrDefault();

        if (filme == null)
        {
            return NotFound();
        }

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta Filme
    /// </summary>
    /// <param name="id">Recebe o Id do filme que ira ser Deletado</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso Atualizacao seja feita com sucesso</response>
    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.Where(x => x.Id == id).FirstOrDefault();

        if(filme == null)
        {
            return NotFound();
        }

        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
