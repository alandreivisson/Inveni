using Inveni.Models;
using System.Collections.Generic;

namespace Inveni.ViewModel {
    public class CompartilharMaterialViewModel {
        public int MaterialId { get; set; }
        public string MaterialNome { get; set; }
        public List<TematicaMestre>? TematicaMestre { get; set; }
    }
}
