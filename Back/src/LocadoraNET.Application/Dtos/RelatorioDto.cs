namespace LocadoraNET.Application.Dtos
{
    public class RelatorioDto
    {
        public string[] ClienteAtraso { get; set; }
        public string[] FilmesNaoAlugados { get; set; }
        public string[] Top5Filmesano { get; set; }
        public string[] Top3FilmesSemana { get; set; }
        public string SegundoCliente { get; set; }
    }
}