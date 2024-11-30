using Microsoft.AspNetCore.Mvc;
using AmigoSecretoAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace AmigoSecretoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrupoController : ControllerBase
    {
        private static List<Grupo> grupos = new List<Grupo>();
        private static int contadorId = 1;

        [HttpPost("criar")]
        public IActionResult CriarGrupo([FromBody] string nome)
        {
            var grupo = new Grupo
            {
                Id = contadorId++, // Incrementa o ID do grupo
                Nome = nome         // Define o nome do grupo
            };

            grupos.Add(grupo); // Adiciona o grupo à lista
            return Ok(grupo);  // Retorna o grupo criado
        }


        [HttpPost("{id}/add-participantes")]
        public IActionResult AdicionarParticipantes(int id, [FromBody] List<string> participantes)
        {
            var grupo = grupos.FirstOrDefault(g => g.Id == id);
            if (grupo == null) return NotFound("Grupo não encontrado.");

            grupo.Participantes.AddRange(participantes);
            return Ok(grupo);
        }

        [HttpPost("{id}/gerar-matches")]
        public IActionResult GerarMatcher(int id)
        {
            var grupo = grupos.FirstOrDefault(g =>g.Id == id);
            if (grupo == null) return NotFound("Grupo não encontrado");
            if (grupo.Participantes.Count < 2) return BadRequest("É necessário pelo menos 2 participantes.");

            var random = new Random();

            // Embbaralha os participantes
            var participantes = grupo.Participantes.OrderBy(p => random.Next()).ToList();

            // Gera os Matches fgechando o ciclo
            for (int i = 0; i < participantes.Count; i++)
            {
                var presenteador = participantes[i];
                var presenteado = participantes[(i + 1) % participantes.Count]; // O último presenteia o primeiro
                grupo.Matches[presenteador] = presenteado;
            }

            return Ok(grupo.Matches);
        }

        [HttpGet("{id}/quem-eu-presenteio")]
        public IActionResult QuemPresenteio(int id, [FromQuery] string participante)
        {
            var grupo = grupos.FirstOrDefault(g => g.Id == id);
            if (grupo == null) return NotFound("Grupo não encontrado");
            if (!grupo.Matches.ContainsKey(participante)) return NotFound("Participante não encontrado no grupo.");

            return Ok(new {Presenteado = grupo.Matches[participante]});
        }

    }
}
