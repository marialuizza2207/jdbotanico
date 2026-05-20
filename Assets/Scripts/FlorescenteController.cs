using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlorescenteController : MonoBehaviour
{
    public string nomeFlor = "Flor";
    public int pontos = 10;

    private bool coletada = false;

    public void TentarColetar()
    {
        if (coletada) return;

        coletada = true;

        GerenciadorJardim gj = Object.FindObjectOfType<GerenciadorJardim>();
        if (gj != null)
            gj.RegistrarColeta(pontos);

        FlorescenteView view = GetComponent<FlorescenteView>();
        if (view != null)
            view.Coletar();
        else
            gameObject.SetActive(false);
    }

    public void OnInteracaoXR(SelectEnterEventArgs args)
    {
        TentarColetar();
    }
}
