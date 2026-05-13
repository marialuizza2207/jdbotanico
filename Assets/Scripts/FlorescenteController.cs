using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Controla a lógica de coleta da flor interativa
public class FlorescenteController : MonoBehaviour
{
    [SerializeField] public string nomeFlor = "Flor";
    [SerializeField] public int pontos = 10;

    private bool coletada = false;

    // Tenta coletar a flor por proximidade (chamado pelo JogadorController)
    public void TentarColetar()
    {
        if (coletada) return;

        coletada = true;

        GerenciadorJardim gj = Object.FindFirstObjectByType<GerenciadorJardim>();
        if (gj != null)
            gj.RegistrarColeta(pontos);

        FlorescenteView view = GetComponent<FlorescenteView>();
        if (view != null)
            view.Coletar();
        else
            gameObject.SetActive(false);
    }

    // Interação via XR (XRSimpleInteractable — SelectEnterEventArgs)
    public void OnInteracaoXR(SelectEnterEventArgs args)
    {
        TentarColetar();
    }
}
