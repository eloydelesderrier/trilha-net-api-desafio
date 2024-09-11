using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using trilha_net_api_desafio.Context;
using trilha_net_api_desafio.Models;

namespace trilha_net_api_desafio.controllers
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
            var tarefa = _context.Tarefas.Find(id);// TODO: Buscar o Id no banco utilizando o EF
            if (tarefa == null)// TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
                return NotFound();
            
            return Ok(tarefa);// caso contrário retornar OK com a tarefa encontrada
            
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefa = _context.Tarefas.ToList();// TODO: Buscar todas as tarefas no banco utilizando o EF
            if (tarefa == null)
                return NotFound();
            return Ok(tarefa);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefa = _context.Tarefas.Where(x=>x.Titulo.Contains(titulo));// TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            if (tarefa == null) // Dica: Usar como exemplo o endpoint ObterPorData
                return NotFound();
            return Ok();
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
                if (tarefa == null)
                    return NotFound();
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            if (tarefa == null)
                return NotFound();  // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
                                    // Dica: Usar como exemplo o endpoint ObterPorData
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Add(tarefa);   // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo; // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;                                    
                                                 
            _context.Tarefas.Update(tarefaBanco);  // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            _context.SaveChanges();                                   
            
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Remove(tarefaBanco);// TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            _context.SaveChanges();
            return NoContent();
        }
    }
}
