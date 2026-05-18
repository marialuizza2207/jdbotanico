using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FonteInterativaController : MonoBehaviour
{
    private FonteInterativaView view;

    void Awake()
    {
        view = GetComponent<FonteInterativaView>();
    }

    public void AoEntrarHoverProximidade()
    {
        view?.SetHover(true);
        var gj = FindFirstObjectByType<GerenciadorJardim>();
        gj?.hudJardim?.ExibirMensagem("Pressione E para ativar a fonte!");
        Debug.Log("[Fonte] Hover enter");
    }

    public void AoSairHoverProximidade()
    {
        view?.SetHover(false);
        var gj = FindFirstObjectByType<GerenciadorJardim>();
        gj?.hudJardim?.ExibirMensagem("Explore o jardim e colete as flores!");
        Debug.Log("[Fonte] Hover exit");
    }

    private bool ativa = false;

    public void AoAtivarProximidade()
    {
        ativa = !ativa;
        view?.SetAtivo(ativa);
        var gj = FindFirstObjectByType<GerenciadorJardim>();
        string msg = ativa ? "Fonte ativada! A água jorra!" : "Fonte desligada.";
        gj?.hudJardim?.ExibirMensagem(msg);
        Debug.Log($"[Fonte] {(ativa ? "Ativada" : "Desligada")}");
    }

    public void AoEntrarHover(HoverEnterEventArgs args) => AoEntrarHoverProximidade();
    public void AoSairHover(HoverExitEventArgs args)    => AoSairHoverProximidade();
    public void AoAtivar(SelectEnterEventArgs args)     => AoAtivarProximidade();
}
