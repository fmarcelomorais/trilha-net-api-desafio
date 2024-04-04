using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {

            var tarefa = _context.Find<Tarefa>(id);
            if(tarefa == null)
            {
                return NotFound(tarefa);
            }

            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.Where(tarefa => tarefa.Id > 0);
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            if(titulo.Length < 3) return BadRequest("A consulta deve ter no minimo 3 caracteres");
            var tarefas = _context.Tarefas.Where(tarefa => tarefa.Titulo.Contains(titulo));
            if(tarefas == null) return NoContent();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            if(tarefa == null) return NoContent();
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, string tiulo, string descricao, DateTime data, EnumStatusTarefa statusTarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            /* if (data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" }); */

            //_context.Update(tarefa);

            tarefaBanco.Titulo = tiulo == null ? tarefaBanco.Titulo : tiulo;
            tarefaBanco.Descricao = descricao == null ? tarefaBanco.Descricao : descricao;
            tarefaBanco.Data = data == null ? tarefaBanco.Data : data;
            tarefaBanco.Status = statusTarefa == null ? tarefaBanco.Status : statusTarefa ;

            _context.SaveChanges();

            return Ok();


        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            return NoContent();
        }
    }
}
