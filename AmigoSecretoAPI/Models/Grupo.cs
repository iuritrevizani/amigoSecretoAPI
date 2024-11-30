using System.Collections.Generic;

namespace AmigoSecretoAPI.Models
{
    public class Grupo
    {
        public int Id { get; set; } // ID do grupo
        public string Nome { get; set; } // Nome do grupo
        public List<string> Participantes { get; set; } = new List<string>();
        public Dictionary<string, string> Matches { get; set; } = new Dictionary<string, string>();
    }
}
