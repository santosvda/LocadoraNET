using System;
using System.Collections.Generic;

namespace LocadoraNET.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public IEnumerable<Locacao> Locacoes { get; set; }
    }
}