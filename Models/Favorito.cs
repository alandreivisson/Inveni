namespace Inveni.Models {
    public class Favorito {
        public int Id { get; set; }
        public int AprendizId { get; set; }
        public int TematicaMestreId { get; set; }
        public bool Favoritado { get; set; }

        public virtual Usuario? Aprendiz { get; set; }
        public virtual TematicaMestre? TematicaMestre { get; set; }
    }

}
