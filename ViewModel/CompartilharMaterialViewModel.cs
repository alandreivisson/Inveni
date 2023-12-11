using Inveni.Models;

namespace Inveni.ViewModel {
    public class CompartilharMaterialViewModel {
        public int MaterialId { get; set; }
        public List<Usuario> Aprendizes { get; set; }
        public List<int> AprendizesSelecionados { get; set; }

        public virtual Material? Material { get; set; }
    }
}
