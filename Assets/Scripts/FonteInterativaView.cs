using UnityEngine;

// Controla o feedback visual da fonte (cor da água conforme estado)
public class FonteInterativaView : MonoBehaviour
{
    private Renderer rend;

    private static readonly Color corNormal = new Color(0.2f, 0.6f, 1.0f);
    private static readonly Color corHover  = new Color(0.0f, 1.0f, 1.0f);
    private static readonly Color corAtiva  = new Color(0.0f, 0.4f, 1.0f);

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = corNormal;
    }

    // Muda a cor conforme o estado de hover
    public void SetHover(bool hover)
    {
        if (rend != null)
            rend.material.color = hover ? corHover : corNormal;
    }

    // Muda a cor para indicar que a fonte foi ativada
    public void SetAtivo()
    {
        if (rend != null)
            rend.material.color = corAtiva;
    }
}
