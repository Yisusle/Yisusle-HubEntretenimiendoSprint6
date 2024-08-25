namespace MovSharkApi
{
    public class FavoriteRequest
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int? PeliculaId { get; set; }
        public int? SerieId { get; set; }
        public string Type { get; set; } 
    }
    public class FavoriteRemoveRequest
    {
        public int UsuarioId { get; set; }
        public int? PeliculaId { get; set; }
        public int? SerieId { get; set; }
    }
}
