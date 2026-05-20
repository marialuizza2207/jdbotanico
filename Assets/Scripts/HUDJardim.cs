using UnityEngine;
using TMPro;

public class HUDJardim : MonoBehaviour
{
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoFlores;
    public TextMeshProUGUI textoMensagem;

    private Transform camTransform;
    [SerializeField] float distancia = 2f;
    [SerializeField] float alturaOffset = -0.2f;

    void Start()
    {
        var cam = Camera.main;
        if (cam == null) cam = FindObjectOfType<Camera>();
        if (cam != null) camTransform = cam.transform;
    }

    void LateUpdate()
    {
        if (camTransform == null) return;

        Vector3 forward = camTransform.forward;
        forward.y = 0;
        forward.Normalize();

        transform.position = camTransform.position
            + forward * distancia
            + Vector3.up * alturaOffset;

        transform.rotation = Quaternion.LookRotation(forward);
    }

    public void AtualizarPontuacao(int pontos)
    {
        if (textoPontuacao) textoPontuacao.text = $"Pontos: {pontos}";
    }

    public void AtualizarFlores(int coletadas, int total)
    {
        if (textoFlores) textoFlores.text = $"Flores: {coletadas}/{total}";
    }

    public void ExibirMensagem(string msg)
    {
        if (textoMensagem) textoMensagem.text = msg;
    }
}
